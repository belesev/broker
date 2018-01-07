using BrokerAlgo.Entities;
using BrokerAlgo.Interfaces;
using QuikSharp;
using System.Collections.Generic;
using System.Linq;

namespace BrokerAlgo.Services
{
    internal class Engine : IEngine
    {
        private readonly IEnumerable<IStrategy> strategies;

        private readonly Quik quik;
        private readonly List<string> instruments;

        public Engine(Quik quik, IEnumerable<IStrategy> strategies)
        {
            this.strategies = strategies;
            this.quik = quik;
            instruments = new XmlConfigReader("Config\\Business\\Instruments.xml").ReadList<string>();
        }

        public void Exec()
        {
            var deals = new List<IDeal>();
            foreach (var strategy in strategies)
            {
                foreach (var toolCode in instruments.Select(i => new ToolCode(i)))
                {
                    var deal = strategy.GetDeal(quik, toolCode);
                    if (deal != null)
                        deals.Add(deal);
                }
            }
            // TODO notify by email

        }

    }
}
