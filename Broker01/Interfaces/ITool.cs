namespace BrokerAlgo.Interfaces
{
    public interface ITool
    {
        string ClassCode { get; }
        string FirmID { get; }
        double GuaranteeProviding { get; }
        decimal LastPrice { get; }
        int Lot { get; }
        string Name { get; }
        int PriceAccuracy { get; }
        string SecurityCode { get; }
        decimal Step { get; }
    }
}