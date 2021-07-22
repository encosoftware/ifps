using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class PriceDetailsDto
    {
        public double? Value { get; set; }
        public int? CurrencyId { get; set; }
        
        public PriceDetailsDto(Price price)
        {
            Value = price?.Value;
            CurrencyId = price?.CurrencyId;
        }
    }
}
