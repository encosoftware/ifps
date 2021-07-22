using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class AdditionalsListDto
    {
        public string MaterialCode { get; set; }
        public string Name { get; set; }
        public List<MaterialPackageByMaterialsListDto> MaterialPackages { get; set; }
        public double StockedAmount { get; set; }
        public int MinAmount { get; set; }
        public int AdvisedAmount { get; set; }
        public int UnderOrderAmount { get; set; }

        public AdditionalsListDto(StockedMaterial stockedMaterial, int supplierId)
        {
            MaterialCode = stockedMaterial.Material.Code;
            Name = stockedMaterial.Material.Description;
            MaterialPackages = stockedMaterial.Material.Packages.Where(ent => ent.SupplierId == supplierId).Select(ent => new MaterialPackageByMaterialsListDto(ent)).ToList();
            StockedAmount = stockedMaterial.StockedAmount;
            MinAmount = stockedMaterial.MinAmount;
            AdvisedAmount = stockedMaterial.RequiredAmount;
            UnderOrderAmount = stockedMaterial.OrderedAmount;
        }
    }
}
