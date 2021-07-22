using System;

namespace IFPS.Factory.Domain.Model
{
    public class MaterialPackage : Package
    {
        public virtual Material Material { get; set; }
        public Guid MaterialId { get; set; }

        public Price Price { get; set; }

        public virtual Company Supplier { get; set; }
        public int SupplierId { get; set; }

        public MaterialPackage(Guid materialId, int supplierId, Price price)
        {
            MaterialId = materialId;
            SupplierId = supplierId;
            Price = price;
        }

        public MaterialPackage(Guid materialId, int supplierId)
        {
            MaterialId = materialId;
            SupplierId = supplierId;
        }
    }
}
