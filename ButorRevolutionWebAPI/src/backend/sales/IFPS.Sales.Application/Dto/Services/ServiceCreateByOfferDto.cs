using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class ServiceCreateByOfferDto
    {
        public bool IsAdded { get; set; }
        public ServiceTypeEnum ServiceType { get; set; }
        public int? ShippingServicePriceId { get; set; }

        public ServiceCreateByOfferDto()
        {

        }

        public OrderedService CreateModelObject(Guid orderId, int serviceId)
        {
            return new OrderedService() { OrderId = orderId, ServiceId = serviceId };
        }
    }
}
