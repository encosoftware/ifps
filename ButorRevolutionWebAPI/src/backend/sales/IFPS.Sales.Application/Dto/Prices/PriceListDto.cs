using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class PriceListDto
    {
        public double? Value { get; set; }
        public int? CurrencyId { get; set; }
        public string Currency { get; set; }

        public PriceListDto() { }

        public PriceListDto(Price price)
        {
            Value = price?.Value;
            CurrencyId = price?.CurrencyId;
            Currency = price.Currency.Name;
        }

        public PriceListDto(Price price, int amount)
        {
            Value = price?.Value * amount;
            CurrencyId = price?.CurrencyId;
            Currency = price.Currency.Name;
        }

        public PriceListDto(double price, int currencyId, string currencyName)
        {
            Value = price;
            CurrencyId = currencyId;
            Currency = currencyName;
        }
    }
}
