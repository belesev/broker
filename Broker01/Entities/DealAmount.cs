using BrokerAlgo.Interfaces;
using System;
using System.Collections.Generic;

namespace BrokerAlgo.Entities
{
    class DealAmount : IDeal
    {
        public DealAmount(DealType type, decimal dealPrice, int lotsAmount, ITool tool, IList<IDeal> linkedDeals)
        {
            if (lotsAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(lotsAmount));

            Type = type;
            DealPrice = dealPrice;
            this.LotsAmount = lotsAmount;
            Tool = tool;
            LinkedDeals = linkedDeals;
        }

        public ITool Tool { get; }

        public DealType Type { get; }

        public decimal DealPrice { get; }

        public int LotsAmount { get; }

        public IList<IDeal> LinkedDeals { get; }
    }
}
