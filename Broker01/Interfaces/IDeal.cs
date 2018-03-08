using System.Collections.Generic;
using BrokerAlgo.Entities;

namespace BrokerAlgo.Interfaces
{
    public interface IDeal
    {
        /// <summary>
        /// Инструмент.
        /// </summary>
        ITool Tool { get; }

        /// <summary>
        /// Тип сделки: продажа или покупка.
        /// </summary>
        DealType Type { get; }

        /// <summary>
        /// Цена сделки.
        /// </summary>
        decimal DealPrice { get; }

        /// <summary>
        /// Количество лотов для покупки/продажи.
        /// </summary>
        int LotsAmount { get; }

        /// <summary>
        /// Связанные заявки, то есть такие, которые надо выставить, если сработает текущая.
        /// Например, TakeProfit и/или StopLoss.
        /// </summary>
        IList<IDeal> LinkedDeals { get; }

        string ToString();
    }
}