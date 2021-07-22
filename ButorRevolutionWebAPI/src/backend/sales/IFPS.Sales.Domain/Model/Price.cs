using ENCO.DDD.Domain.Model.Values;

namespace IFPS.Sales.Domain.Model
{
    public class Price : ValueObject<Price>
    {
        public double Value { get; set; }

        /// <summary>
        /// Currency, in which the material type price is measured
        /// </summary>
        public Currency Currency { get; set; }
        public int CurrencyId { get; set; }        

        public Price(double value, int currencyId)
        {
            Value = value;
            CurrencyId = currencyId;
        }


        public static Price GetDefaultPrice() => new Price(0,1);
    }
}