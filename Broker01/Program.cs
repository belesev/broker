using BrokerAlgo.Services;
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

            var engine = (Engine)ctx.GetObject("engine");
            engine.Exec();
        }
    }
}