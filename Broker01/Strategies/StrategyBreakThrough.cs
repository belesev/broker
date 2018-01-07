using System;
using BrokerAlgo.Entities;
using BrokerAlgo.Helpers;
using BrokerAlgo.Interfaces;
using JetBrains.Annotations;
using QuikSharp;
using QuikSharp.DataStructures;
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
        private readonly int count;
        /// <summary>
        /// Процент, при пробитии верхней границы на размер которого совершается сделка.
        /// В процентах от последней цены.
        /// </summary>
        private readonly decimal breakThroughPercent;
        /// <summary>
        /// Ширина коридора (чувствительность) вверх+вниз, при выходе за который совершается сделка.
        /// В процентах от последней цены.
        /// </summary>
        private readonly decimal corridorPercent;
        /// <summary>
        /// Максимальная сумма покупки по сделке.
        /// </summary>
        private readonly decimal moneyMaxAmount;

        [UsedImplicitly]
        public StrategyBreakThrough(decimal breakThroughPercent, IPriceService priceService, CandleInterval interval, int count, decimal moneyMaxAmount, decimal corridorPercent)
        {
            this.priceService = priceService;
            this.interval = interval;
            this.count = count;
            this.moneyMaxAmount = moneyMaxAmount;
            this.corridorPercent = corridorPercent;
            this.breakThroughPercent = breakThroughPercent;
        }

        public IDeal GetDeal(Quik quik, ToolCode toolCode)
        {
            var tool = new Tool(quik, toolCode.Code);
            var pricesBundle = priceService.GetLastPrices(tool, interval, count);

            //var avgClose = pricesBundle.Prices.Sum(p => p.Close) / pricesBundle.Prices.Count();
            //var max = pricesBundle.Prices.Max(p => p.Close);

            // TODO использовать не персентиль, а другую усредняющую функцию
            decimal percentileClose = MathHelper.Percentile(pricesBundle.Prices.Select(c => c.Close), 0.95m);
            decimal upBorder = percentileClose.AddPercent(corridorPercent / 2);
            decimal downBorder = percentileClose.SubtractPercent(corridorPercent / 2);

            bool withinCorridor = pricesBundle.Prices.All(p => p.Close < upBorder && p.Close > downBorder);

            if (!withinCorridor)
            {
                Logger.Log.Debug($"StrategyBreakThrough for {toolCode} is not within corridor: {pricesBundle.ToString(c => c.Close)}, upBorder={upBorder}, downBorder={downBorder}");
                return null;
            }

            decimal lastPrice = priceService.LastPrice(tool);
            decimal percentileHigh = MathHelper.Percentile(pricesBundle.Prices.Select(p => p.High), 0.95m);
            decimal breakThrough = percentileHigh.AddPercent(breakThroughPercent);
            if (lastPrice > breakThrough)
            {
                var amount = moneyMaxAmount / lastPrice;
                var lotsAmount = Convert.ToInt32(Math.Floor(amount / tool.Lot));
                return new DealAmount(DealType.Buy, lotsAmount, tool);
            }
            else
            {
                Logger.Log.Debug($"StrategyBreakThrough for {toolCode} does not work: lastPrice = {lastPrice} while percentileHigh={percentileClose}, breakThrough={breakThrough}");
                return null;
            }
        }
    }
}
