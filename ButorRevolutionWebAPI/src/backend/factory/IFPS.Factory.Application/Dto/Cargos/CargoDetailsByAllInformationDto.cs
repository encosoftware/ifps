using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class CargoDetailsByAllInformationDto
    {
        public string CargoName { get; set; }
        public CargoStatusTypeListDto Status { get; set; }
        public DateTime? StockedOn { get; set; }
        public CargoUserListDto BookedBy { get; set; }
        public DateTime Created { get; set; }
        public SupplierCompanyListDto Supplier { get; set; }
        public PriceListDto TotalGrossCost { get; set; }

        public CargoDetailsByAllInformationDto(Cargo cargo)
        {
            CargoName = cargo.CargoName;
            Status = new CargoStatusTypeListDto(cargo.Status);
            StockedOn = cargo.StockedOn;
            BookedBy = new CargoUserListDto(cargo.BookedBy);
            Created = cargo.BookedOn;
            Supplier = new SupplierCompanyListDto(cargo.Supplier);
            TotalGrossCost = new PriceListDto(new Price(cargo.NetCost.Value + cargo.Vat.Value, cargo.NetCost.CurrencyId) { Currency = cargo.NetCost.Currency });
        }
    }
}
