using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class OrderedPackagesByStockDto
    {
        public int MaterialPackageId { get; set; }
        public string MaterialCode { get; set; }
        public string Name { get; set; }
        public int Arrived { get; set; }
        public int Refused { get; set; }

        public StorageCellDropdownListDto Cell { get; set; }

        public OrderedPackagesByStockDto(OrderedMaterialPackage orderedPackage)
        {
            MaterialPackageId = orderedPackage.MaterialPackageId;
            MaterialCode = orderedPackage.MaterialPackage.Material.Code;
            Name = orderedPackage.MaterialPackage.Material.Description;
            Arrived = orderedPackage.OrderedAmount - orderedPackage.MissingAmount;
            Refused = orderedPackage.RefusedAmount;
            Cell = new StorageCellDropdownListDto();
        }
    }
}
