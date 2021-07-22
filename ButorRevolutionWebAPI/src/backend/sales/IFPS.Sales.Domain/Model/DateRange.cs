using ENCO.DDD;
using ENCO.DDD.Domain.Model.Values;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class DateRange : ValueObject<DateRange>
    {
        /// <summary>
        /// Start of the interval
        /// </summary>
        public DateTime From { get; private set; }

        /// <summary>
        /// End of the interval
        /// </summary>
        public DateTime To { get; private set; }

        private DateRange()
        {
        }

        public DateRange(DateTime from, DateTime to)
        {
            Ensure.That<ArgumentException>(
                from < to,
                $"Parameter {nameof(to)} should be greater than {nameof(from)}");

            this.From = from;
            this.To = to;
        }
    }
}
