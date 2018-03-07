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
        public int Lot { get; } = 1;
        public string Name { get; }
        public int PriceAccuracy { get; }
        public string SecurityCode { get; }
        public decimal Step { get; }

        protected bool Equals(TestTool other)
        {
            return string.Equals(SecurityCode, other.SecurityCode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TestTool) obj);
        }

        public override int GetHashCode()
        {
            return (SecurityCode != null ? SecurityCode.GetHashCode() : 0);
        }
    }
}
