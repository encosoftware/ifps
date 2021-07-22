using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{ 
    public class CargoCostDetailsDto
    {
        public PriceDetailsDto Shipping { get; set; }
        public PriceDetailsDto SubTotal { get; set; }
        public PriceDetailsDto Total { get; set; }
        public PriceDetailsDto Vat { get; set; }

        public CargoCostDetailsDto(Cargo cargo, double vat)
        {
            Shipping = new PriceDetailsDto(cargo.ShippingCost);
            SubTotal = new PriceDetailsDto(new Price(cargo.ShippingCost.Value + cargo.NetCost.Value, cargo.ShippingCost.CurrencyId) { Currency = cargo.NetCost.Currency });
            Vat = new PriceDetailsDto(Math.Round(cargo.NetCost.Value * vat), cargo.Vat.Currency.Name);
            Total = new PriceDetailsDto(SubTotal.Value.Value + Vat.Value.Value, cargo.Vat.Currency.Name);
        }
    }
}
