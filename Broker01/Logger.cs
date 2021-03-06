﻿using System.IO;
using log4net;
using log4net.Config;

namespace BrokerAlgo
{
    public static class Logger
    {
        private static ILog log = LogManager.GetLogger("LOGGER");

        public static ILog Log
        {
            get { return log; }
        }

        public static void InitLogger(FileInfo file)
        {
            XmlConfigurator.Configure(file);
        }
    }
}
