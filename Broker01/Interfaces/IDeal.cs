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
        /// Метод расчёта количества покупки, исходя из состояния счёта.
        /// </summary>
        int LotsAmount(IAccountService accountService);

        /// <summary>
        /// Связанные заявки, то есть такие, которые надо выставить, если сработает текущая.
        /// Например, TakeProfit и/или StopLoss.
        /// </summary>
        IList<IDeal> LinkedDeals { get; }
    }
}