using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class OfferProductsDetailsDto
    {
        public PriceInformationByOfferDto Prices { get; set; }
        public List<FurnitureUnitListByOfferDto> TopCabinets { get; set; }
        public List<FurnitureUnitListByOfferDto> BaseCabinets { get; set; }
        public List<FurnitureUnitListByOfferDto> TallCabinets { get; set; }
        public List<ApplianceListByOfferDto> Appliances { get; set; }
        public List<AccessoryMaterialsListByOfferDto> Accessories { get; set; }
        public ShippingServiceListByOfferDto ShippingService { get; set; }
        public AssemblyServiceListByOfferDto AssemblyService { get; set; }
        public InstallationServiceListByOfferDto InstallationService { get; set; }

        public OfferProductsDetailsDto(Order order, double shippingBasicFee, double installationBasicFee)
        {
            Prices = new PriceInformationByOfferDto();
            TopCabinets = new List<FurnitureUnitListByOfferDto>();
            BaseCabinets = new List<FurnitureUnitListByOfferDto>();
            TallCabinets = new List<FurnitureUnitListByOfferDto>();
            Appliances = new List<ApplianceListByOfferDto>();
            Accessories = new List<AccessoryMaterialsListByOfferDto>();

            if (order.Services.Count() != 0)
            {
                if (order.Services.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Shipping))
                {
                    ShippingService = order.Services
                        .Where(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Shipping)
                        .Select(ent => new ShippingServiceListByOfferDto(ent, shippingBasicFee))
                        .SingleOrDefault();
                }
                if (order.Services.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Assembly))
                {
                    AssemblyService = order.Services
                        .Where(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Assembly)
                        .Select(ent => new AssemblyServiceListByOfferDto(ent))
                        .SingleOrDefault();
                }
                if (order.Services.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Installation))
                {
                    InstallationService = order.Services
                        .Where(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Installation)
                        .Select(ent => new InstallationServiceListByOfferDto(ent, installationBasicFee))
                        .SingleOrDefault();
                }
            }
        }
    }
}
