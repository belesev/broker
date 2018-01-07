﻿using BrokerAlgo.Entities;
using QuikSharp.DataStructures;

namespace BrokerAlgo.Interfaces
{
    interface IPriceService
    {
        PricesBundle GetLastPrices(ITool tool, CandleInterval candleInterval, int intervalsCount);

        PricesBundle GetAllPrices(ITool tool, CandleInterval candleInterval);

        decimal LastPrice(ITool tool);
    }
}
