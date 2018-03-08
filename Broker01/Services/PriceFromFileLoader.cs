using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BrokerAlgo.Entities;
using BrokerAlgo.Helpers;
using BrokerAlgo.Interfaces;
using QuikSharp.DataStructures;

namespace BrokerAlgo.Services
{
    class PriceFromFileLoader : IPriceService
    {
        private readonly ITool tool;
        private readonly string path;
        private readonly Lazy<List<Candle>> prices;
        private CandleInterval fileCandleInterval;
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
            fileCandleInterval = main[1].Parse<CandleInterval>();

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
                    Interval = fileCandleInterval,
                    SecCode = tool.SecurityCode,
                });
            }
            return candles;
        }

        public PricesBundle GetLastPrices(ITool tool, CandleInterval candleInterval, int intervalsCount)
        {
            GetPrices();

            if (!tool.Equals(this.tool))
                throw new ArgumentException(nameof(tool));
            return new PricesBundle(candleInterval, intervalsCount, GetPrices());
        }

        public PricesBundle GetAllPrices(ITool tool, CandleInterval candleInterval)
        {
            if (!tool.Equals(this.tool))
                throw new ArgumentException(nameof(tool));
            return new PricesBundle(candleInterval, null, GetPrices());
        }

        public decimal LastPrice(ITool tool)
        {
            return GetPrices().Last().Close;
        }

        /// <summary>
        /// Сдвигает указатель на следующую строку (имитирует наступление следующего тайм-фрейма).
        /// </summary>
        public bool ShiftPointer(int shiftAmount = 1)
        {
            currentRow += shiftAmount;
            return currentRow <= prices.Value.Count;
        }

        /// <summary>
        /// Сдвигает указатель на следующую строку (имитирует наступление следующего тайм-фрейма).
        /// </summary>
        public bool ShiftPointer(CandleInterval shiftInterval)
        {
            if (shiftInterval < fileCandleInterval)
                throw new InvalidOperationException($"Impossible to shift for {shiftInterval} while file contains time interval {fileCandleInterval}: too small shift");
            if ((int)shiftInterval % (int)fileCandleInterval != 0)
                throw new InvalidOperationException($"Impossible to shift for {shiftInterval}: it's not divisible by file time interval {fileCandleInterval}");

            var shiftRows = (int)shiftInterval / (int)fileCandleInterval;

            currentRow += shiftRows;
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
