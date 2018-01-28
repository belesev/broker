using BrokerAlgo.Interfaces;

namespace Tests
{
    class TestTool : ITool
    {
        public TestTool(string securityCode)
        {
            SecurityCode = securityCode;
        }

        public string ClassCode { get; }
        public string FirmID { get; }
        public double GuaranteeProviding { get; }
        public int Lot { get; }
        public string Name { get; }
        public int PriceAccuracy { get; }
        public string SecurityCode { get; }
        public decimal Step { get; }
    }
}
