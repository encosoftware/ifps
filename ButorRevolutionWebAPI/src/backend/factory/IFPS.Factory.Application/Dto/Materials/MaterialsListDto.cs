using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class MaterialsListDto
    {
        public string MaterialCode { get; set; }
        public string Name { get; set; }
        public List<MaterialPackageByMaterialsListDto> MaterialPackages { get; set; }
        public int RequiredAmount { get; set; }
        public double StockedAmount { get; set; }
        public int MinAmount { get; set; }
        public int AdvisedAmount { get; set; }
        public int UnderOrderAmount { get; set; }

        public MaterialsListDto(IGrouping<string, RequiredMaterial> requiredMaterial, StockedMaterial stockedMaterial, int supplierId)
        {
            MaterialCode = requiredMaterial.First().Material.Code;
            Name = requiredMaterial.First().Material.Description;
            MaterialPackages = requiredMaterial.First().Material.Packages.Where(ent => ent.SupplierId == supplierId).Select(ent => new MaterialPackageByMaterialsListDto(ent)).ToList();
            RequiredAmount = requiredMaterial.Sum(e => e.RequiredAmount);
            StockedAmount = stockedMaterial.StockedAmount;
            MinAmount = stockedMaterial.MinAmount;
            AdvisedAmount = stockedMaterial.RequiredAmount;
            UnderOrderAmount = stockedMaterial.OrderedAmount;
        }
    }
}
