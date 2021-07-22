using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class CargoDetailsByProductsDto
    {
        public string CargoName { get; set; }
        public CargoStatusTypeListDto Status { get; set; }
        public CargoUserListDto BookedByUser { get; set; }
        public DateTime Created { get; set; }
        public string ContractingParty { get; set; }
        public SupplierCompanyListDto SupplierName { get; set; }
        public PriceListDto NetCost { get; set; }
        public PriceListDto Vat { get; set; }
        public PriceListDto TotalGrossCost { get; set; }

        public CargoDetailsByProductsDto(Cargo cargo)
        {
            CargoName = cargo.CargoName;
            Status = new CargoStatusTypeListDto(cargo.Status);
            BookedByUser = new CargoUserListDto(cargo.BookedBy);
            Created = cargo.BookedOn;
            ContractingParty = cargo.BookedBy.Company.Name;
            SupplierName = new SupplierCompanyListDto(cargo.Supplier);
            NetCost = new PriceListDto(cargo.NetCost);
            Vat = new PriceListDto(cargo.Vat);
            TotalGrossCost = new PriceListDto(new Price(NetCost.Value.Value + Vat.Value.Value, cargo.NetCost.CurrencyId) { Currency = cargo.NetCost.Currency });
        }
    }
}
