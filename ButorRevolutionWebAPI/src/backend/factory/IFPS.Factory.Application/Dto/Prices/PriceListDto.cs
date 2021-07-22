using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class PriceListDto
    {
        public double? Value { get; set; }
        public int? CurrencyId { get; set; }
        public string Currency { get; set; }

        public PriceListDto(Price price)
        {
            Value = price?.Value;
            CurrencyId = price?.CurrencyId;
            Currency = price.Currency.Name;
        }
    }
}
