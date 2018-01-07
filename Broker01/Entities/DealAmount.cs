using BrokerAlgo.Interfaces;
using System;

namespace BrokerAlgo.Entities
{
    class DealAmount : IDeal
    {
        private readonly int lotsAmount;

        public DealAmount(DealType type, int lotsAmount, ITool tool)
        {
            if (lotsAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(lotsAmount));

            Type = type;
            this.lotsAmount = lotsAmount;
            Tool = tool;
        }

        public ITool Tool { get; }
        public DealType Type { get; }

        public int LotsAmount(IAccountService accountService)
        {
            return lotsAmount;
        }
    }
}
