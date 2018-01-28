using System;
using System.IO;
using System.Linq;
using System.Text;
using BrokerAlgo.Helpers;
using BrokerAlgo.Interfaces;
using QuikSharp.DataStructures;

namespace BrokerAlgo.Services
{
    internal class PriceToFileLoader : IPriceToFileLoader
    {
        private readonly ITool tool;
        private readonly IPriceService priceService;
        private readonly CandleInterval interval;
        private readonly DateTime minDate;
        private readonly DateTime maxDate;

        public PriceToFileLoader(ITool tool, IPriceService priceService, CandleInterval interval, DateTime minDate, DateTime maxDate)
        {
            this.tool = tool;
            this.priceService = priceService;
            this.interval = interval;
            this.minDate = minDate;
            this.maxDate = maxDate;

            if (maxDate <= minDate)
                throw new ArgumentException(nameof(maxDate));
        }

        public void Save()
        {
            var path = $"{tool.SecurityCode}_{interval}_{minDate:yyyyMMdd}-{maxDate:yyyyMMdd}.txt";
            Save(path);
        }

        public void Save(string filename)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{tool.SecurityCode};{interval}");
            sb.AppendLine($"{Const.Open};{Const.Close};{Const.Low};{Const.High};{Const.DateTime};{Const.Volume}");

            var prices = priceService.GetAllPrices(tool, interval);
            foreach (var candle in prices.Prices.Where(p => ((DateTime)p.Datetime) > minDate && ((DateTime)p.Datetime) < maxDate))
            {
                sb.AppendLine(
                    $"{candle.Open};{candle.Close};{candle.Low};{candle.High};{((DateTime) candle.Datetime).ToString(Const.DateTimeFormat)};{candle.Volume}");
            }
            var path = $"{FileHelper.GetAppdataFolder()}\\{filename}";
            File.WriteAllText(path, sb.ToString());
        }
    }
}
