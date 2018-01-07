using BrokerAlgo.Interfaces;

namespace BrokerAlgo.Entities
{
    class DealAll : IDeal
    {
        public DealAll(DealType type, ITool tool)
        {
            Type = type;
            Tool = tool;
        }

        public ITool Tool { get; }
        public DealType Type { get; }

        public int LotsAmount(IAccountService accountService)
        {
            return accountService.GetCurrentAmount(Tool) ?? 0;
        }
    }
}
