using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class PriceCreateDto
    {
        public decimal Value { get; set; }
        public int CurrencyId { get; set; }

        public Price CreateModelObject()
        {
            return new Price((double)Value, CurrencyId);
        }
    }
}
