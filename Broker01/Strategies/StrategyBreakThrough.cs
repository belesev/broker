using BrokerAlgo.Entities;
using BrokerAlgo.Helpers;
using BrokerAlgo.Interfaces;
using JetBrains.Annotations;
using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [UsedImplicitly]
        public StrategyBreakThrough(decimal breakThroughPercent, IPriceService priceService, CandleInterval interval, int candlesCount, decimal moneyMaxAmount)
        {
            this.priceService = priceService;
            this.interval = interval;
            this.candlesCount = candlesCount;
            this.moneyMaxAmount = moneyMaxAmount;
            this.breakThroughPercent = breakThroughPercent;
        }

        public IList<IDeal> GetDeals(ITool tool)
        {
            var pricesBundle = priceService.GetLastPrices(tool, interval, candlesCount);

            decimal upBorder = pricesBundle.Prices.Last().Close;
            // в COUNT последних интвервалов цена High ниже на BREAKTHROUGHPERCENT%, чем нынешняя цена закрытия
            bool isBreakThrough = pricesBundle.Prices.All(p => p.High.AddPercent(breakThroughPercent) < upBorder);

            if (!isBreakThrough)
            {
                Logger.Log.Debug($"StrategyBreakThrough for {tool} is not breakthrough: {pricesBundle.ToString(c => c.High)}, upBorder={upBorder}");
                return null;
            }

            decimal lastPrice = priceService.LastPrice(tool);
            // количество к покупке
            var amount = moneyMaxAmount / lastPrice;
            var lotsAmount = Convert.ToInt32(Math.Floor(amount / tool.Lot));

            var takeProfitDeal = new DealAmount(DealType.Sell, lotsAmount, tool, new List<IDeal>());
            return new List<IDeal>
            {
                new DealAmount(DealType.Buy, lotsAmount, tool, new List<IDeal> {takeProfitDeal})
            };
        }
    }
}
