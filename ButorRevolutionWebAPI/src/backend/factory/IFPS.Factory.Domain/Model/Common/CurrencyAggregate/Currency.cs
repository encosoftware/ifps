using ENCO.DDD.Domain.Model.Entities;

namespace IFPS.Factory.Domain.Model
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
    }
}
