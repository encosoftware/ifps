using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class MaterialPackageUpdateDto
    {
        public Guid MaterialId { get; set; }
        public int SupplierId { get; set; }
        public PriceUpdateDto Price { get; set; }
        public int Size { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public MaterialPackage UpdateModelObject(MaterialPackage materialPackage)
        {
            materialPackage.MaterialId = MaterialId;
            materialPackage.SupplierId = SupplierId;
            materialPackage.Price = Price.CreateModelObject();
            materialPackage.Size = Size;
            materialPackage.PackageCode = Code;
            materialPackage.PackageDescription = Description;
            return materialPackage;
        }
    }
}
