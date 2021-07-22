using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class CargoCreateDto
    {
        public string CargoName { get; set; }
        public int BookedById { get; set; }
        public int SupplierId { get; set; }
        public PriceCreateDto ShippingCost { get; set; }
        public AddressCreateDto ShippingAddress { get; set; }
        public string Notes { get; set; }
        public List<OrderedMaterialPackageCreateDto> Additionals { get; set; }
        public List<OrderedMaterialPackageCreateDto> Materials { get; set; }

        public Cargo CreateCargo()
        {
            return new Cargo(CargoName, SupplierId, BookedById,
                Price.GetDefaultPrice(), Price.GetDefaultPrice(),
                ShippingAddress.CreateModelObject(), ShippingCost.CreateModelObject(), 
                Notes, Clock.Now, null, null);
        }

        public void SetVat(Price netCost, Price vat, double vatValue, int currencyId)
        {
            vat.Value = netCost.Value * vatValue;
            if (vat.CurrencyId != netCost.CurrencyId)
            {
                vat.CurrencyId = currencyId;
            }
        }
    }
}
