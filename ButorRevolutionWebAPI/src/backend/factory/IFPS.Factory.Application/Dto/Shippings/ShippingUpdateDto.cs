namespace IFPS.Factory.Application.Dto
{
    public class ShippingUpdateDto
    {
        public PriceUpdateDto ShippingCost { get; set; }

        public AddressUpdateDto ShippingAddress { get; set; }

        public string Note { get; set; }

        public ShippingUpdateDto()
        {

        }
        
    }
}
