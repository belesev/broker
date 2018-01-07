namespace BrokerAlgo.Interfaces
{
    public interface IAccountService
    {
        double? GetCurrentMoney();
        int? GetCurrentAmount(ITool tool);
    }
}