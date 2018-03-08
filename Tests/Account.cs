using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BrokerAlgo;
using BrokerAlgo.Entities;
using BrokerAlgo.Helpers;
using BrokerAlgo.Interfaces;

namespace Tests
{
    internal class Account
    {
        private decimal money;
        private int dealCounter = 0;
        private readonly IDictionary<ITool, decimal> portfolio = new Dictionary<ITool, decimal>();

        public Account(decimal money)
        {
            this.money = money;
        }

        public bool CanProcess(IDeal deal)
        {
            var amount = deal.LotsAmount * deal.Tool.Lot;

            return deal.Type == DealType.Buy
                ? money >= deal.DealPrice * amount
                : portfolio.GetValueOrDefault(deal.Tool, 0m) >= amount;
        }

        public void Process(IDeal deal)
        {
            var toolSign = deal.Type == DealType.Buy ? 1m : -1m;
            var amount = deal.LotsAmount * deal.Tool.Lot;

            money -= deal.DealPrice * amount * toolSign;
            portfolio[deal.Tool] = portfolio.GetValueOrDefault(deal.Tool, 0m) + amount * toolSign;

            Logger.Log.Debug($"Processing deal #{++dealCounter}: {CurrentTime.GetTime()}, {deal.ToString()}");

            Debug.Assert(money >= 0, "Money amount cannot become negative");
            Debug.Assert(portfolio[deal.Tool] >= 0, $"Tool {deal.Tool.SecurityCode} amount cannot become negative");
        }

        public string ToString(IPriceService priceService)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Money: {money}");
            decimal totalAssetEstimation = 0m;
            foreach (var kvp in portfolio)
            {
                var tool = kvp.Key;
                var lastPrice = priceService.LastPrice(tool);

                var amount = kvp.Value;
                var assetEstimation = lastPrice * amount;
                totalAssetEstimation += assetEstimation;
                sb.AppendLine($"{tool.SecurityCode}: {amount} = ~{assetEstimation} RUB");
            }
            sb.AppendLine($"Total (money+security): {money + totalAssetEstimation} RUB");
            return sb.ToString();
        }
    }
}