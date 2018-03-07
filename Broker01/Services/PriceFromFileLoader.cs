using BrokerAlgo.Entities;
using BrokerAlgo.Helpers;
using BrokerAlgo.Interfaces;
using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BrokerAlgo.Services
{
    class PriceFromFileLoader : IPriceService
    {
        private readonly ITool tool;
        private readonly string path;
        private readonly Lazy<List<Candle>> prices;
        private CandleInterval candleInterval;
        private int currentRow = 1;

        public PriceFromFileLoader(ITool tool, string filename)
        {
            this.tool = tool;
            this.path = $"{FileHelper.GetAppdataFolder()}\\{filename}";

            prices = new Lazy<List<Candle>>(ReadFile);
        }

        private List<Candle> ReadFile()
        {
            var lines = File.ReadAllLines(path);

            var main = lines[0].Split(';');
            candleInterval = main[1].Parse<CandleInterval>();

            var headers = lines[1].Split(';');

            var closePos = Array.IndexOf(headers, Const.Close);
            var openPos = Array.IndexOf(headers, Const.Open);
            var highPos = Array.IndexOf(headers, Const.High);
            var lowPos = Array.IndexOf(headers, Const.Low);
            var dateTimePos = Array.IndexOf(headers, Const.DateTime);
            var volumePos = Array.IndexOf(headers, Const.Volume);

            var candles = new List<Candle>();
            foreach (var line in lines.Skip(2))
            {
                var splitted = line.Split(';');
                candles.Add(new Candle()
                {
                    ClassCode = tool.ClassCode,
                    Close = decimal.Parse(splitted[closePos]),
                    Open = decimal.Parse(splitted[openPos]),
                    High = decimal.Parse(splitted[highPos]),
                    Low = decimal.Parse(splitted[lowPos]),
                    Datetime = (QuikDateTime) DateTime.ParseExact(splitted[dateTimePos], Const.DateTimeFormat,
                        CultureInfo.InvariantCulture),
                    Volume = int.Parse(splitted[volumePos]),
                    Interval = candleInterval,
                    SecCode = tool.SecurityCode,
                });
            }
            return candles;
        }

        public PricesBundle GetLastPrices(ITool tool, CandleInterval candleInterval, int intervalsCount)
        {
            GetPrices();

            if (tool != this.tool)
                throw new ArgumentException(nameof(tool));
            if (candleInterval != this.candleInterval)
                throw new ArgumentException(nameof(candleInterval));
            return new PricesBundle(candleInterval, intervalsCount, GetPrices());
        }

        public PricesBundle GetAllPrices(ITool tool, CandleInterval candleInterval)
        {
            

            if (tool != this.tool)
                throw new ArgumentException(nameof(tool));
            if (candleInterval != this.candleInterval)
                throw new ArgumentException(nameof(candleInterval));
            return new PricesBundle(candleInterval, null, GetPrices());
        }

        public decimal LastPrice(ITool tool)
        {
            return GetPrices().Last().Close;
        }

        public bool ShiftPointer(int shiftAmount = 1)
        {
            currentRow += shiftAmount;
            return currentRow <= prices.Value.Count;
        }

        public QuikDateTime GetCurrentDate()
        {
            return GetPrices().Last().Datetime;
        }

        private IEnumerable<Candle> GetPrices()
        {
            return prices.Value.Take(currentRow);
        }
    }
}
