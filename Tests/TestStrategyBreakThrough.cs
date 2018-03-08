using BrokerAlgo;
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
            var tool = new TestTool("LSNGP");
            var priceService = new PriceFromFileLoader(tool, "LSNGP_M5_20171201-20180307.txt");
            var strategy = new StrategyBreakThrough(1, priceService, CandleInterval.H1, 20, 5000, 5, 3);

            var dealProcessor = new DealProcessor(priceService);
            CurrentTime.SetFunction(priceService.GetCurrentDate);

            do
            {
                dealProcessor.TryProcessBacklog();

                var deals = strategy.GetDeals(tool);
                if (deals != null)
                    dealProcessor.Process(deals);

            } while (priceService.ShiftPointer(strategy.Interval));

            Assert.IsNotEmpty(dealProcessor.Backlog);
            Logger.Log.Debug(dealProcessor.Account.ToString(priceService));
        }
    }
}
