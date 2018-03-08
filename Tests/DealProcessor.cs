using System;
using System.Collections.Generic;
using System.Linq;
using BrokerAlgo.Entities;
using BrokerAlgo.Interfaces;

namespace Tests
{
    /// <summary>
    /// Эмулятор счёта клиента: сколько осталось ценных бумаг (portfolio) и денег.
    /// </summary>
    internal class DealProcessor
    {
        private readonly IPriceService priceService;
        private readonly Account account = new Account(15000);

        public DealProcessor(IPriceService priceService)
        {
            this.priceService = priceService;
        }

        public List<IDeal> Backlog { get; } = new List<IDeal>();

        public Account Account => account;

        public void Process(IList<IDeal> deals)
        {
            foreach (var deal in deals)
            {
                account.Process(deal);
                Backlog.Remove(deal);
            }

            Backlog.AddRange(deals.SelectMany(d => d.LinkedDeals));
        }

        public void TryProcessBacklog()
        {
            foreach (var dealGrouping in Backlog.GroupBy(bd => bd.Tool))
            {
                var tool = dealGrouping.Key;
                var lastPrice = priceService.LastPrice(tool);

                foreach (var deal in dealGrouping)
                {
                    if (deal.Type == DealType.SellTakeProfit && lastPrice > deal.DealPrice && account.CanProcess(deal))
                        account.Process(deal);
                    else if (deal.Type == DealType.SellStopLoss && lastPrice < deal.DealPrice && account.CanProcess(deal))
                        account.Process(deal);
                    else if (deal.Type == DealType.Buy)
                        throw new NotImplementedException("Not implemented yet, when linked deal is BUY");
                }
            }
        }
    }
}