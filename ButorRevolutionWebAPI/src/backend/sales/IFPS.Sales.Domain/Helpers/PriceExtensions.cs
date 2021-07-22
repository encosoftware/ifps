using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Helpers
{
    public static class PriceExtensions
    {
        public static Price Add(this Price input, double amount)
        {
            input.Value += amount;
            return input;
        }

        public static Price Add(this Price input, Price price)
        {
            input.Value += price.Value;
            return input;
        }

        public static Price Div(this Price input, double amount)
        {
            input.Value -= amount;
            return input;
        }

        public static Price Div(this Price input, Price price)
        {
            input.Value -= price.Value;
            return input;
        }
    }
}
