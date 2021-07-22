using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class ExchangeRate : FullAuditedAggregateRoot
    {
        /// <summary>
        /// Date on which the exchange rate was registered
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Base currency, to which the other currencies are compared to
        /// </summary>
        public virtual Currency BaseCurrency { get; set; }
        public int BaseCurrencyId { get; set; }

        /// <summary>
        /// Currency type, which one unit is measured by the base currency
        /// </summary>
        public virtual Currency ChangeCurrency { get; set; }
        public int ChangeCurrencyId { get; set; }
        
        /// <summary>
        /// Shows the amount of base currency required to buy one unit of change currency
        /// </summary>
        public double Rate { get; set; }

        private ExchangeRate()
        {

        }

        public ExchangeRate(DateTime date, int baseCurrencyId, int changeCurrencyId, double exchangeRate)
        {
            this.Date = date;
            this.BaseCurrencyId = baseCurrencyId;
            this.ChangeCurrencyId = changeCurrencyId;
            this.Rate = exchangeRate;
        }
    }
}
