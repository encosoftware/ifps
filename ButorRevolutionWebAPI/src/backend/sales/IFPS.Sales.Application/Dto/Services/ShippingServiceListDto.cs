using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class ShippingServiceListDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public PriceListDto Price { get; set; }

        public ShippingServiceListDto(Service service)
        {
            Id = service.Id;
            Description = service.Description;
            Price = new PriceListDto(service.CurrentPrice.Price);
        }
    }
}
