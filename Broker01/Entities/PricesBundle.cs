using System;
using QuikSharp.DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace BrokerAlgo.Entities
{
    class PricesBundle
    {
        public CandleInterval Interval { get; }
        public int? IntervalsCount { get; }
        public IEnumerable<Candle> Prices { get; }

        public PricesBundle(CandleInterval candleInterval, int? intervalsCount, IEnumerable<Candle> prices)
        {
            this.Interval = candleInterval;
            this.IntervalsCount = intervalsCount;
            this.Prices = prices;
        }

        public string ToString(Func<Candle, decimal> func)
        {
            return "[" + string.Join(";", Prices.Select(func)) + "]";
        }
    }
}
