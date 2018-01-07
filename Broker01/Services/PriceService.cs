using BrokerAlgo.Entities;
using BrokerAlgo.Interfaces;
using QuikSharp;
using QuikSharp.DataStructures;
using System;

namespace BrokerAlgo.Services
{
    internal class PriceService  : IPriceService
    {
        private readonly Quik quik;
        private readonly char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

        public PriceService(Quik quik)
        {
            this.quik = quik;
        }

        public PricesBundle GetLastPrices(ITool tool, CandleInterval candleInterval, int intervalsCount)
        {
            var prices = quik.Candles.GetLastCandles(tool.ClassCode, tool.SecurityCode, candleInterval, intervalsCount).Result;
            return new PricesBundle(candleInterval, intervalsCount, prices);
        }

        public PricesBundle GetAllPrices(ITool tool, CandleInterval candleInterval)
        {
            var prices = quik.Candles.GetAllCandles(tool.ClassCode, tool.SecurityCode, candleInterval).Result;
            return new PricesBundle(candleInterval, null, prices);
        }

        public decimal LastPrice(ITool tool)
        {
            return Convert.ToDecimal(quik.Trading.GetParamEx(tool.ClassCode, tool.SecurityCode, "LAST").Result.ParamValue.Replace('.', separator));
        }

        //var toolCandles = quik.Candles.GetAllCandles(tool.ClassCode, tool.SecurityCode, CandleInterval.H1).Result;
    }
}
