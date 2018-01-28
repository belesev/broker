using BrokerAlgo.Interfaces;
using System;
using System.Collections.Generic;

namespace BrokerAlgo.Entities
{
    class DealAmount : IDeal
    {
        private readonly int lotsAmount;

        public DealAmount(DealType type, int lotsAmount, ITool tool, IList<IDeal> linkedDeals)
        {
            if (lotsAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(lotsAmount));

            Type = type;
            this.lotsAmount = lotsAmount;
            Tool = tool;
            LinkedDeals = linkedDeals;
        }

        public ITool Tool { get; }
        public DealType Type { get; }

        public int LotsAmount(IAccountService accountService)
        {
            return lotsAmount;
        }

        public IList<IDeal> LinkedDeals { get; }
    }
}
