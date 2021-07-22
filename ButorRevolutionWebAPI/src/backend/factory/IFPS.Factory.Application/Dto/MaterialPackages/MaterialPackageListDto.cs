using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class MaterialPackageListDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string SupplierName { get; set; }
        public int Size { get; set; }
        public PriceListDto Price { get; set; }

        public MaterialPackageListDto(MaterialPackage materialPackage)
        {
            Id = materialPackage.Id;
            Code = materialPackage.PackageCode;
            Description = materialPackage.PackageDescription;
            SupplierName = materialPackage.Supplier.Name;
            Size = materialPackage.Size;
            Price = new PriceListDto(materialPackage.Price);
        }

        public MaterialPackageListDto() { }

        public static Func<MaterialPackage, MaterialPackageListDto> FromEntity = entity => new MaterialPackageListDto(entity);
    }
}
