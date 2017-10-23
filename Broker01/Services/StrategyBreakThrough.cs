using BrokerAlgo.Interfaces;
using QuikSharp.DataStructures;
using System.Collections.Generic;

namespace BrokerAlgo.Services
{
    internal sealed class StrategyBreakThrough : IStrategy
    {
        public bool IsFit(ITool tool)
        {
            return true;
        }

        public void Process(IList<Candle> lastPrices)
        {
        }
    }
}
