using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class ServiceListDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ServiceTypeEnum Type { get; set; }
        public PriceListDto Price { get; set; }

        public ServiceListDto(Service service)
        {
            Id = service.Id;
            Type = service.ServiceType.Type;
            if (service.CurrentPrice != null)
            {
                Description = service.Description;
                Price = new PriceListDto(service.CurrentPrice.Price);
            }
        }
    }
}
