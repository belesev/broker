using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BrokerAlgo.Entities;
using BrokerAlgo.Helpers;
using BrokerAlgo.Interfaces;

namespace Tests
{
    /// <summary>
    /// Эмулятор счёта клиента: сколько осталось ценных бумаг (portfolio) и денег.
    /// </summary>
    internal class DealProcessor
    {
        private readonly IPriceService priceService;
        private decimal money;
        private readonly IDictionary<ITool, decimal> portfolio = new Dictionary<ITool, decimal>();

        public DealProcessor(IPriceService priceService)
        {
            this.priceService = priceService;

            money = 15000;
        }

        public void Process(IList<IDeal> deals)
        {
            foreach (var deal in deals)
            {
                Process(deal);
            }

            Backlog.AddRange(deals.SelectMany(d => d.LinkedDeals));
        }

        public List<IDeal> Backlog { get; } = new List<IDeal>();

        public void TryProcessBacklog()
        {
            foreach (var dealGrouping in Backlog.GroupBy(bd => bd.Tool))
            {
                var tool = dealGrouping.Key;
                var lastPrice = priceService.LastPrice(tool);

                foreach (var deal in dealGrouping)
                {
                    if (deal.Type == DealType.SellTakeProfit && lastPrice > deal.DealPrice)
                        Process(deal);
                    else if (deal.Type == DealType.SellStopLoss && lastPrice < deal.DealPrice)
                        Process(deal);
                    else if (deal.Type == DealType.Buy)
                        throw new NotImplementedException("Not implemented yet, when linked deal is BUY");
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Money: {money}");
            foreach (var kvp in portfolio)
            {
                var tool = kvp.Key;
                var lastPrice = priceService.LastPrice(tool);

                var amount = kvp.Value;
                var assetEstimation = lastPrice * amount;
                sb.AppendLine($"{tool.SecurityCode}: {amount} = ~{assetEstimation}руб.");
            }
            sb.AppendLine();
            return sb.ToString();
        }

        private void Process(IDeal deal)
        {
            var toolSign = deal.Type == DealType.Buy ? 1m : -1m;
            var amount = deal.LotsAmount * deal.Tool.Lot;

            money -= deal.DealPrice * amount * toolSign;
            portfolio[deal.Tool] = portfolio.GetValueOrDefault(deal.Tool, 0m) + amount * toolSign;

            Debug.Assert(money >= 0, "Money amount cannot become negative");
            Debug.Assert(portfolio[deal.Tool] >= 0, $"Tool {deal.Tool.SecurityCode} amount cannot become negative");

            Backlog.Remove(deal);
        }
    }
}