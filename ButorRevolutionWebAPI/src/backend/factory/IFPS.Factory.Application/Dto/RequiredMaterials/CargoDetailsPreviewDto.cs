using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class CargoDetailsPreviewDto
    {
        public string CargoName { get; set; }
        public ContractingPartyDto ContractingParty { get; set; }
        public SupplierCompanyDto Supplier { get; set; }
        public ShippingDetailsDto Shipping { get; set; }
        public List<OrderedMaterialPackageListDto> OrderedPackages { get; set; }
        public PreviewCostDetailsDto PreviewCost { get; set; }

        public CargoDetailsPreviewDto(Cargo cargo)
        {
            CargoName = cargo.CargoName;
            ContractingParty = new ContractingPartyDto(cargo.BookedBy);
            Supplier = new SupplierCompanyDto(cargo.Supplier);
            Shipping = new ShippingDetailsDto(cargo);
            OrderedPackages = cargo.OrderedPackages.Select(ent => new OrderedMaterialPackageListDto(ent)).ToList();
            PreviewCost = new PreviewCostDetailsDto(cargo);
        }
    }
}
