using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class MaterialPackageFilterDto : OrderedPagedRequestDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string SupplierName { get; set; }
        public int Size { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(Code), nameof(MaterialPackage.PackageCode) },
                { nameof(Description), nameof(MaterialPackage.PackageDescription) },
                { nameof(SupplierName), nameof(MaterialPackage.Supplier.Name) },
                { nameof(Size), nameof(MaterialPackage.Size) }
            };
        }
    }
}
