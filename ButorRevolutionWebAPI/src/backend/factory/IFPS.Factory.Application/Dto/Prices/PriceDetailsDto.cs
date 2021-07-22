using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class PriceDetailsDto
    {
        public double? Value { get; set; }
        public string Currency { get; set; }

        public PriceDetailsDto(Price price)
        {
            Value = price?.Value;
            Currency = price?.Currency.Name;
        }

        public PriceDetailsDto(Price vat, double? subTotal)
        {
            Value = vat?.Value * subTotal;
            Currency = vat?.Currency.Name;
        }

        public PriceDetailsDto(PriceDetailsDto price)
        {
            Value = price?.Value;
            Currency = price?.Currency;
        }

        public PriceDetailsDto(List<Price> prices)
        {
            Value = 0;
            foreach (var price in prices)
            {
                Value += price.Value;
            }
            Currency = prices.First().Currency.Name;
        }

        public PriceDetailsDto(double value, string currency)
        {
            Value = value;
            Currency = currency;
        }
    }
}
