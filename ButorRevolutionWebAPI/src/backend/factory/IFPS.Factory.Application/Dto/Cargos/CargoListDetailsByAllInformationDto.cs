using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class CargoListDetailsByAllInformationDto
    {
        public string CargoName { get; set; }
        public ContractingPartyDto ContractingParty { get; set; }
        public SupplierCompanyDto Supplier { get; set; }
        public ShippingDetailsDto Shipping { get; set; }
        public List<ProductListWithAllInfrmationDto> Products { get; set; }
        public CargoCostDetailsDto Cost { get; set; }

        public CargoListDetailsByAllInformationDto(Cargo cargo, double vat)
        {
            CargoName = cargo.CargoName;
            ContractingParty = new ContractingPartyDto(cargo.BookedBy);
            Supplier = new SupplierCompanyDto(cargo.Supplier);
            Shipping = new ShippingDetailsDto(cargo);
            Products = cargo.OrderedPackages.Select(ent => new ProductListWithAllInfrmationDto(ent)).ToList();
            Cost = new CargoCostDetailsDto(cargo, vat);
        }
    }
}
