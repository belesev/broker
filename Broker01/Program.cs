using System;
using System.Globalization;
using System.IO;
using BrokerAlgo.Entities;
using BrokerAlgo.Helpers;
using BrokerAlgo.Interfaces;
using BrokerAlgo.Services;
using QuikSharp;
using QuikSharp.DataStructures;
using Spring.Context;
using Spring.Context.Support;

namespace BrokerAlgo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Logger.InitLogger(new FileInfo(".\\Config\\log4net.config"));
            ContextRegistry.RegisterContext(new XmlApplicationContext("Config\\Spring.xml"));

            IApplicationContext ctx = ContextRegistry.GetContext();

            var quik = (Quik)ctx.GetObject("quik");
            var isConnectedTask = quik.Service.IsConnected();
            var isServerConnected = isConnectedTask.Result;
            if (!isServerConnected)
            {
                Logger.Log.Error("Cannot connect to QUIK");
                return;
            }
            Logger.Log.Info("Connected to QUIK successfully");

            if (args.Length > 2 && args[0].ToLower() == "-f")
            {
                // -f LSNGP H4
                var toolcode = args[1];
                var interval = args[2].Parse<CandleInterval>();
                var dateStart = DateTime.ParseExact(args[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var tool = new Tool(quik, toolcode);
                var priceService = (IPriceService)ctx.GetObject("priceService");

                var ptfs = new PriceToFileLoader(tool, priceService, interval, dateStart, DateTime.Now);
                ptfs.Save();
            }

            //var prices1000 = priceService.GetLastPrices(tool, CandleInterval.H1, 1000);
            //var pricesAll = priceService.GetAllPrices(tool, CandleInterval.H1);


            //var breakThrough = new StrategyBreakThrough(10, priceService, CandleInterval.H1, 100, 1000);
            //ToolCode toolCode = new ToolCode("AFKS");
            //breakThrough.GetDeals(new Tool(quik, toolCode.Code));


            //var accountService = (IAccountService)ctx.GetObject("accountService");
            //accountService.GetCurrentAmount(tool);
            //accountService.GetCurrentMoney();

            //var engine = (Engine)ctx.GetObject("engine");
            //engine.Exec();
        }
    }
}