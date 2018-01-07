using System;
using BrokerAlgo.Interfaces;
using BrokerAlgo.Services;
using QuikSharp;

namespace BrokerAlgo.Entities
{
    public class Tool : ITool
    {
        private readonly char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

        private string clientCode;

        private decimal lastPrice;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="_quik"></param>
        /// <param name="securityCode">Код инструмента</param>
        /// <param name="classCode">Код класса</param>
        public Tool(Quik quik, string securityCode)
        {
            var classCode = quik.Class.GetSecurityClass(Consts.ClassesList, securityCode).Result;
            GetBaseParam(quik, securityCode, classCode);
        }

        #region Свойства

        /// <summary>
        /// Краткое наименование инструмента (бумаги)
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Код инструмента (бумаги)
        /// </summary>
        public string SecurityCode { get; private set; }

        /// <summary>
        /// Код класса инструмента (бумаги)
        /// </summary>
        public string ClassCode { get; private set; }

        /// <summary>
        /// Счет клиента
        /// </summary>
        public string AccountID { get; private set; }

        /// <summary>
        /// Код фирмы
        /// </summary>
        public string FirmID { get; private set; }

        /// <summary>
        /// Количество акций в одном лоте
        /// Для инструментов класса SPBFUT = 1
        /// </summary>
        public int Lot { get; private set; }

        /// <summary>
        /// Точность цены (количество знаков после запятой)
        /// </summary>
        public int PriceAccuracy { get; private set; }

        /// <summary>
        /// Шаг цены
        /// </summary>
        public decimal Step { get; private set; }

        /// <summary>
        /// Гарантийное обеспечение (только для срочного рынка)
        /// для фондовой секции = 0
        /// </summary>
        public double GuaranteeProviding { get; private set; }

        #endregion

        public override string ToString()
        {
            return $"{SecurityCode} ({Name})";
        }

        private void GetBaseParam(Quik quik, string secCode, string classCode)
        {
            try
            {
                SecurityCode = secCode;
                ClassCode = classCode;
                if (quik != null)
                {
                    if (!string.IsNullOrEmpty(ClassCode))
                    {
                        try
                        {
                            Name = quik.Class.GetSecurityInfo(ClassCode, SecurityCode).Result.ShortName;
                            AccountID = quik.Class.GetTradeAccount(ClassCode).Result;
                            FirmID = quik.Class.GetClassInfo(ClassCode).Result.FirmId;
                            Step = Convert.ToDecimal(quik.Trading.GetParamEx(ClassCode, SecurityCode, "SEC_PRICE_STEP").Result.ParamValue.Replace('.', separator));
                            PriceAccuracy = Convert.ToInt32(Convert.ToDouble(quik.Trading.GetParamEx(ClassCode, SecurityCode, "SEC_SCALE").Result.ParamValue.Replace('.', separator)));
                        }
                        catch (Exception e)
                        {
                            Logger.Log.Error("Tool.GetBaseParam. Ошибка получения наименования для " + SecurityCode + ": " + e.Message);
                        }

                        if (ClassCode == "SPBFUT")
                        {
                            Logger.Log.Error("Получаем 'guaranteeProviding'.");
                            Lot = 1;
                            GuaranteeProviding = Convert.ToDouble(quik.Trading.GetParamEx(ClassCode, SecurityCode, "BUYDEPO").Result.ParamValue.Replace('.', separator));
                        }
                        else
                        {
                            Logger.Log.Error("Получаем 'lot'.");
                            Lot = Convert.ToInt32(Convert.ToDouble(quik.Trading.GetParamEx(ClassCode, SecurityCode, "LOTSIZE").Result.ParamValue.Replace('.', separator)));
                            GuaranteeProviding = 0;
                        }
                    }
                    else
                    {
                        Logger.Log.Error("Tool.GetBaseParam. Ошибка: classCode не определен.");
                        Lot = 0;
                        GuaranteeProviding = 0;
                    }
                }
                else
                {
                    Logger.Log.Error("Tool.GetBaseParam. quik = null !");
                }
            }
            catch (NullReferenceException e)
            {
                Logger.Log.Error("Ошибка NullReferenceException в методе GetBaseParam: " + e.Message);
            }
            catch (Exception e)
            {
                Logger.Log.Error("Ошибка в методе GetBaseParam: " + e.Message);
            }
        }
    }
}