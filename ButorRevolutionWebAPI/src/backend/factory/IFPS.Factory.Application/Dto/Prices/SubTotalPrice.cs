using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class SubTotalPrice
    {
        public double Value { get; set; }
        public string Currency { get; set; }

        public SubTotalPrice(Price price, int quantity)
        {
            Value = price.Value * quantity;
            Currency = price.Currency.Name;
        }
    }
}
