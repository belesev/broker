using BrokerAlgo.Services;
using BrokerAlgo.Strategies;
using NUnit.Framework;
using QuikSharp.DataStructures;

namespace Tests
{
    class TestStrategyBreakThrough
    {
        [Test]
        public void Test()
        {
            var tool = new TestTool("AFKS");
            var priceService = new PriceFromFileLoader(tool, CandleInterval.M15, "AFKS_M15_20171201-20180128.txt");
            var strategy = new StrategyBreakThrough(1, priceService, CandleInterval.M15, 20, 5000);

            do
            {
                var deals = strategy.GetDeals(tool);
            } while (priceService.ShiftPointer());
        }
    }
}
