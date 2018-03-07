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
            var priceService = new PriceFromFileLoader(tool, "LSNGP_H1_20180101-20180308.txt");
            var strategy = new StrategyBreakThrough(1, priceService, CandleInterval.H1, 20, 5000, 5, 2);

            var dealProcessor = new DealProcessor(priceService);

            bool newPricesExist;
            do
            {
                var deals = strategy.GetDeals(tool);
                if (deals != null)
                    dealProcessor.Process(deals);

                newPricesExist = priceService.ShiftPointer();
                if (newPricesExist)
                {
                    dealProcessor.TryProcessBacklog();
                }
            } while (newPricesExist);

            Assert.IsNotEmpty(dealProcessor.Backlog);
            Logger.Log.Debug(dealProcessor.ToString());
        }
    }
}
