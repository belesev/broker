using System.Collections.Generic;
using BrokerAlgo.Interfaces;

namespace BrokerAlgo.Entities
{
    class DealAll : IDeal
    {
        public DealAll(DealType type, ITool tool, IList<IDeal> linkedDeals)
        {
            Type = type;
            Tool = tool;
            LinkedDeals = linkedDeals;
        }

        public ITool Tool { get; }
        public DealType Type { get; }

        public int LotsAmount(IAccountService accountService)
        {
            return accountService.GetCurrentAmount(Tool) ?? 0;
        }

        public IList<IDeal> LinkedDeals { get; }
    }
}
