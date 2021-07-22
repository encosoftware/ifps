namespace IFPS.Sales.Domain.Model
{
    public class ShippingService : Service
    {
        public Price BasicFee { get; set; }

        public ShippingService(Price basicFee, string description) : base(description)
        {
            BasicFee = basicFee;
        }
    }
}
