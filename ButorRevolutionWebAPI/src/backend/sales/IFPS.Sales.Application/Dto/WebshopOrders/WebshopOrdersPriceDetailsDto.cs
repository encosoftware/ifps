using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopOrdersPriceDetailsDto
    {
        public PriceListDto Subtotal { get; set; }
        public PriceListDto DeliveryPrice { get; set; }

        public WebshopOrdersPriceDetailsDto(Basket basket)
        {
            Subtotal = new PriceListDto(basket.SubTotal);
            DeliveryPrice = new PriceListDto(basket.DelieveryPrice);
        }
    }
}
