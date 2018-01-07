using BrokerAlgo.Entities;
using BrokerAlgo.Interfaces;
using BrokerAlgo.Services;
using BrokerAlgo.Strategies;
using QuikSharp;
using QuikSharp.DataStructures;
using Spring.Context;
using Spring.Context.Support;

namespace BrokerAlgo
{
    internal class Program
    {
        public static void Main()
        {
            Logger.InitLogger();
            ContextRegistry.RegisterContext(new XmlApplicationContext("Config\\Spring.xml"));

            IApplicationContext ctx = ContextRegistry.GetContext();

            var quik = (Quik)ctx.GetObject("quik");
            var isServerConnected = quik.Service.IsConnected().Result;
            if (!isServerConnected)
            {
                Logger.Log.Error("Cannot connect to QUIK");
                return;
            }
            Logger.Log.Info("Connected to QUIK successfully");

            var tool = new Tool(quik, "AFKS");
            var priceService = (IPriceService)ctx.GetObject("priceService");
            var prices1000 = priceService.GetLastPrices(tool, CandleInterval.H1, 1000);
            var pricesAll = priceService.GetAllPrices(tool, CandleInterval.H1);


            var breakThrough = new StrategyBreakThrough(10, priceService, CandleInterval.H1, 100, 1000, 5);
            breakThrough.GetDeal(quik, new ToolCode("AFKS"));


            //var accountService = (IAccountService)ctx.GetObject("accountService");
            //accountService.GetCurrentAmount(tool);
            //accountService.GetCurrentMoney();

            var engine = (Engine)ctx.GetObject("engine");
            engine.Exec();
        }
    }
}