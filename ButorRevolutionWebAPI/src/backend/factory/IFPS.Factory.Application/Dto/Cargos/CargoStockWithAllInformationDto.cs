using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class CargoStockWithAllInformationDto
    {
        public string CargoName { get; set; }
        public ContractingPartyDto ContractingParty { get; set; }
        public SupplierCompanyDto Supplier { get; set; }
        public ShippingDetailsDto Shipping { get; set; }
        public List<OrderedPackagesByStockDto> OrderedPackages { get; set; }

        public CargoStockWithAllInformationDto(Cargo cargo)
        {
            CargoName = cargo.CargoName;
            ContractingParty = new ContractingPartyDto(cargo.BookedBy);
            Supplier = new SupplierCompanyDto(cargo.Supplier);
            Shipping = new ShippingDetailsDto(cargo);
            OrderedPackages = cargo.OrderedPackages.Select(ent => new OrderedPackagesByStockDto(ent)).ToList();
        }
    }
}
