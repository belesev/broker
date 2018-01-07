using System;
using BrokerAlgo.Entities;
using BrokerAlgo.Interfaces;
using JetBrains.Annotations;
using QuikSharp;
using System.Collections.Generic;
using System.Linq;

namespace BrokerAlgo.Strategies
{
    class FixIncomeStrategy : IStrategy
    {
        private readonly IPriceService priceService;
        private readonly IDictionary<string, decimal> fixPrices;

        [UsedImplicitly]
        public FixIncomeStrategy(IPriceService priceService)
        {
            this.priceService = priceService;
            fixPrices = new XmlConfigReader("Config\\Business\\FixIncome.xml").ReadDictionary<string, double>("key").ToDictionary(p => p.Key, p => Convert.ToDecimal(p.Value));
        }

        public IDeal GetDeal(Quik quik, ToolCode toolCode)
        {
            var tool = new Tool(quik, toolCode.Code);
            var lastPrice = priceService.LastPrice(tool);
            if (fixPrices.TryGetValue(tool.ClassCode, out var fixPrice) && lastPrice >= fixPrice)
                return new DealAll(DealType.Sell, tool);
            return null;
        }
    }
}
