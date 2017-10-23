using QuikSharp.DataStructures;
using System.Collections.Generic;

namespace BrokerAlgo.Interfaces
{
    public interface IStrategy
    {
        bool IsFit(ITool tool);
        void Process(IList<Candle> lastPrices);
    }
}