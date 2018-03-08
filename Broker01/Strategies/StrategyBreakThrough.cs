using System;
using System.Collections.Generic;
using System.Linq;
using BrokerAlgo.Entities;
using BrokerAlgo.Helpers;
using BrokerAlgo.Interfaces;
using JetBrains.Annotations;
using QuikSharp.DataStructures;

namespace BrokerAlgo.Strategies
{
    internal sealed class StrategyBreakThrough : IStrategy
    {
        private readonly IPriceService priceService;
        /// <summary>
        /// На сколько назад анализировать цены.
        /// </summary>
        private readonly CandleInterval interval;
        /// <summary>
        /// На сколько назад анализировать цены.
        /// </summary>
        private readonly int candlesCount;
        /// <summary>
        /// Процент, при пробитии верхней границы на размер которого совершается сделка.
        /// В процентах от последней цены.
        /// </summary>
        private readonly decimal breakThroughPercent;
        /// <summary>
        /// Максимальная сумма покупки по сделке.
        /// </summary>
        private readonly decimal moneyMaxAmount;
        /// <summary>
        /// Процент от цены начальной сделки, при достижении которого должна произойти сделка take profit.
        /// Т.е. ожидаемая прибыль по сделке.
        /// </summary>
        private readonly decimal takeProfitPercent;
        /// <summary>
        /// Процент от цены начальной сделки, при падении на который должна произойти сделка stop loss.
        /// Т.е. максимально допустимая потеря по сделке.
        /// </summary>
        private readonly decimal stopLossPercent;

        [UsedImplicitly]
        public StrategyBreakThrough(decimal breakThroughPercent, [NotNull] IPriceService priceService, CandleInterval interval, int candlesCount, decimal moneyMaxAmount, decimal takeProfitPercent, decimal stopLossPercent)
        {
            if (breakThroughPercent <= 0) throw new ArgumentOutOfRangeException(nameof(breakThroughPercent));
            if (candlesCount <= 0) throw new ArgumentOutOfRangeException(nameof(candlesCount));
            if (moneyMaxAmount <= 0) throw new ArgumentOutOfRangeException(nameof(moneyMaxAmount));
            if (takeProfitPercent <= 0) throw new ArgumentOutOfRangeException(nameof(takeProfitPercent));
            if (stopLossPercent <= 0 || stopLossPercent >= 100) throw new ArgumentOutOfRangeException(nameof(stopLossPercent));

            this.priceService = priceService ?? throw new ArgumentNullException(nameof(priceService));
            this.interval = interval;
            this.candlesCount = candlesCount;
            this.moneyMaxAmount = moneyMaxAmount;
            this.takeProfitPercent = takeProfitPercent;
            this.stopLossPercent = stopLossPercent;
            this.breakThroughPercent = breakThroughPercent;
        }

        public IList<IDeal> GetDeals(ITool tool)
        {
            var pricesBundle = priceService.GetLastPrices(tool, interval, candlesCount);

            decimal upBorder = pricesBundle.Prices.Last().Close;
            // в COUNT последних интвервалов цена High ниже на BREAKTHROUGHPERCENT%, чем нынешняя цена закрытия
            bool isBreakThrough = pricesBundle.Prices.ExceptLast().All(p => p.High.AddPercent(breakThroughPercent) < upBorder);

            if (!isBreakThrough)
            {
                //Logger.Log.Debug($"StrategyBreakThrough for {tool} is not breakthrough: {pricesBundle.ToString(c => c.High)}, upBorder={upBorder}");
                return null;
            }

            decimal lastPrice = priceService.LastPrice(tool);
            // количество к покупке
            var amount = moneyMaxAmount / lastPrice;
            var lotsAmount = Convert.ToInt32(Math.Floor(amount / tool.Lot));

            var takeProfitDeal = new DealAmount(DealType.SellTakeProfit, lastPrice.AddPercent(takeProfitPercent), lotsAmount, tool, new List<IDeal>());
            var stopLossDeal = new DealAmount(DealType.SellStopLoss, lastPrice.SubtractPercent(stopLossPercent), lotsAmount, tool, new List<IDeal>());
            return new List<IDeal>
            {
                new DealAmount(DealType.Buy, lastPrice, lotsAmount, tool, new List<IDeal> {takeProfitDeal, stopLossDeal})
            };
        }

        public CandleInterval Interval => interval;
    }
}
