using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class OfferPreviewDto
    {
        public string OfferName { get; set; }
        public List<DocumentDetailsDto> Renderers { get; set; }
        public ProducerPreviewDto Producer { get; set; }
        public CustomerPreviewDto Customer { get; set; }
        public List<FurnitureUnitListByPreviewDto> TopCabinets { get; set; }
        public List<FurnitureUnitListByPreviewDto> BaseCabinets { get; set; }
        public List<FurnitureUnitListByPreviewDto> TallCabinets { get; set; }
        public List<ApplianceListByPreviewDto> Appliances { get; set; }
        public List<AccessoryMaterialsListByPreviewDto> Accessories { get; set; }
        public PriceInformationByOfferDto Prices { get; set; }

        public OfferPreviewDto(Order order)
        {
            OfferName = order.OrderName + " " + DateTime.Now.ToShortDateString();
            Producer = new ProducerPreviewDto(order.SalesPerson);
            Customer = new CustomerPreviewDto(order.Customer, order);
            TopCabinets = order.OrderedFurnitureUnits
                .Where(ent => ent.FurnitureUnit.FurnitureUnitType.Type == FurnitureUnitTypeEnum.Top)
                .Select(ent => new FurnitureUnitListByPreviewDto(ent)).ToList();

            BaseCabinets = order.OrderedFurnitureUnits
                .Where(ent => ent.FurnitureUnit.FurnitureUnitType.Type == FurnitureUnitTypeEnum.Base)
                .Select(ent => new FurnitureUnitListByPreviewDto(ent)).ToList();

            TallCabinets = order.OrderedFurnitureUnits
                .Where(ent => ent.FurnitureUnit.FurnitureUnitType.Type == FurnitureUnitTypeEnum.Tall)
                .Select(ent => new FurnitureUnitListByPreviewDto(ent)).ToList();

            Appliances = order.OrderedApplianceMaterials.Select(ent => new ApplianceListByPreviewDto(ent)).ToList();

            Accessories = new List<AccessoryMaterialsListByPreviewDto>();
            foreach (var ofu in order.OrderedFurnitureUnits.Where(ofu => ofu.FurnitureUnit.Accessories.Count() != 0).Select(ofu => ofu))
            {
                var list = ofu.FurnitureUnit.Accessories.Select(e => new AccessoryMaterialsListByPreviewDto(e)).ToList();
                Accessories.AddRange(list);
            }

            Prices = new PriceInformationByOfferDto();
        }
    }
}
