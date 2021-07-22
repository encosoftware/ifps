using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
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
