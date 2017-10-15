using QuikSharp;
using QuikSharp.DataStructures;
using System;
using System.Configuration;
using System.Timers;

namespace Broker01
{
    internal class Program
    {
        private static Quik quik;

        public static void Main()
        {
            Logger.InitLogger();
            var instruments = new XmlConfigReader("..\\..\\Config\\instruments.xml").ReadList();

            quik = new Quik(Quik.DefaultPort, new InMemoryStorage());
            var isServerConnected = quik.Service.IsConnected().Result;
            if (!isServerConnected)
            {
                Logger.Log.Error("Cannot connect to Quik");
                return;
            }
            Logger.Log.Info("Connected to QUIK successfully");

            var secCode = "KRKNP";
            var classCode = quik.Class.GetSecurityClass("SPBFUT,TQBR,TQBS,TQNL,TQLV,TQNE,TQOB", secCode).Result;
            var clientCode = quik.Class.GetClientCode().Result;

            //ConfigurationManager.AppSettings[""];

            var tool = new Tool(quik, secCode, classCode);
            var lastPrice = tool.LastPrice;

            //var toolCandles = quik.Candles.GetAllCandles(tool.ClassCode, tool.SecurityCode, CandleInterval.H1).Result;
            var x = quik.Candles.GetLastCandles(tool.ClassCode, tool.SecurityCode, CandleInterval.H1, 10).Result;

            DepoLimitEx q1 = quik.Trading.GetDepoEx(tool.FirmID, clientCode, tool.SecurityCode, tool.AccountID, 2).Result;
            if (q1 != null)
            {
                var qty = Convert.ToInt32(q1.CurrentBalance);
            }

            //var q2 = quik.Trading.GetDepo(clientCode, tool.FirmID, tool.SecurityCode, tool.AccountID).Result;
            var intervalSeconds = 60;
            var timer = new Timer(intervalSeconds);
            timer.Elapsed += Timer_Elapsed;
            Logger.Log.Info("Running timer...");
            timer.Start();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
        }
    }
}