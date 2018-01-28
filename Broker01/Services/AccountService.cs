using System;
using BrokerAlgo.Entities;
using BrokerAlgo.Interfaces;
using QuikSharp;

namespace BrokerAlgo.Services
{
    class AccountService : IAccountService
    {
        private readonly Quik quik;
        private readonly string clientCode;

        public AccountService(Quik quik, string clientCode)
        {
            this.quik = quik;
            this.clientCode = clientCode;
        }

        public double? GetCurrentMoney()
        {
            var q1 = quik.Trading.GetMoney(clientCode, Consts.OpenBrokerFirmId, Consts.Eqtv, Consts.Rub).Result;
            return q1?.MoneyCurrentBalance;
        }

        public int? GetCurrentAmount(ITool tool)
        {
            var accountId = quik.Class.GetTradeAccount(tool.ClassCode).Result;
            var q1 = quik.Trading.GetDepoEx(tool.FirmID, clientCode, tool.SecurityCode, accountId, 2).Result;
            return q1 != null ? Convert.ToInt32(q1.CurrentBalance) : (int?)null;
        }
    }
}
