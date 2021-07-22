using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class MaterialPackageDetailsDto
    {
        public Guid MaterialId { get; set; }
        public int SupplierId { get; set; }
        public PriceListDto Price { get; set; }
        public int Size { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public MaterialPackageDetailsDto(MaterialPackage materialPackage)
        {
            MaterialId = materialPackage.MaterialId;
            SupplierId = materialPackage.SupplierId;
            Price = new PriceListDto(materialPackage.Price);
            Size = materialPackage.Size;
            Code = materialPackage.PackageCode;
            Description = materialPackage.PackageDescription;
        }
    }
}
