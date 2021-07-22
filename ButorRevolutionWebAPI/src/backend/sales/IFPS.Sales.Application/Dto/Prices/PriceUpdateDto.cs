using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class PriceUpdateDto
    {
        public double Value { get; set; }
        public int CurrencyId { get; set; }

        public Price CreateModelObject()
        {
            return new Price(Value, CurrencyId);
        }
    }
}
