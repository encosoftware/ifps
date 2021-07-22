using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class MaterialPackageCreateDto
    {
        public Guid MaterialId { get; set; }
        public int SupplierId { get; set; }
        public PriceCreateDto Price { get; set; }
        public int Size { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public MaterialPackage CreateModelObject()
        {
            return new MaterialPackage(MaterialId, SupplierId, Price.CreateModelObject())
            {
                Size = Size,
                PackageCode = Code,
                PackageDescription = Description
            };
        }
    }
}
