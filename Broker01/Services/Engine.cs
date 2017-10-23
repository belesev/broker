using System;
using System.Collections.Generic;
using BrokerAlgo.Interfaces;
using QuikSharp;
using QuikSharp.DataStructures;

namespace BrokerAlgo.Services
{
    internal class Engine : IEngine
    {
        private readonly IEnumerable<IStrategy> strategies;
        private const string ClassesList = "SPBFUT,TQBR,TQBS,TQNL,TQLV,TQNE,TQOB";

        private Quik quik;
        private List<string> instruments;

        public Engine(IEnumerable<IStrategy> strategies)
        {
            this.strategies = strategies;
            instruments = new XmlConfigReader("Config\\instruments.xml").ReadList();

            quik = new Quik(Quik.DefaultPort, new InMemoryStorage());
            var isServerConnected = quik.Service.IsConnected().Result;
            if (!isServerConnected)
            {
                Logger.Log.Error("Cannot connect to QUIK");
                return;
            }
            Logger.Log.Info("Connected to QUIK successfully");
        }

        public void Exec()
        {
            var toolCode = instruments[0]; //"KRKNP";
            var classCode = quik.Class.GetSecurityClass(ClassesList, toolCode).Result;
            var clientCode = quik.Class.GetClientCode().Result;

            var tool = new Tool(quik, toolCode, classCode);
            var lastPrice = tool.LastPrice;

            //var toolCandles = quik.Candles.GetAllCandles(tool.ClassCode, tool.SecurityCode, CandleInterval.H1).Result;
            var x = quik.Candles.GetLastCandles(tool.ClassCode, tool.SecurityCode, CandleInterval.H1, 10).Result;

            foreach (var strategy in strategies)
            {
                strategy.Process(x);
            }

            //DepoLimitEx q1 = quik.Trading.GetDepoEx(tool.FirmID, clientCode, tool.SecurityCode, tool.AccountID, 2).Result;
            //if (q1 != null)
            //{
            //    var qty = Convert.ToInt32(q1.CurrentBalance);
            //}

            //var q2 = quik.Trading.GetDepo(clientCode, tool.FirmID, tool.SecurityCode, tool.AccountID).Result;
        }

    }
}
