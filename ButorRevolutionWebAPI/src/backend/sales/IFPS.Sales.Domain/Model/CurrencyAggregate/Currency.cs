using ENCO.DDD.Domain.Model.Entities;

namespace IFPS.Sales.Domain.Model
{
    public class Currency : AggregateRoot
    {
        /// <summary>
        /// International name of the currency
        /// </summary>
        public string Name { get; private set; }

        private Currency() { }

        public Currency(string name)
        {
            Name = name;
        }

        public static Currency GetDeafultCurrency() => new Currency("HUF");
    }
}
