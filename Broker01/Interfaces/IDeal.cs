using BrokerAlgo.Entities;

namespace BrokerAlgo.Interfaces
{
    public interface IDeal
    {
        ITool Tool { get; }
        DealType Type { get; }
        int LotsAmount(IAccountService accountService);
    }
}