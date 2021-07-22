using ENCO.DDD.Domain.Model.Entities;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class WebshopFurnitureUnitImage : Entity
    {
        public virtual WebshopFurnitureUnit WebshopFurnitureUnit { get; set; }
        public int WebshopFurnitureUnitId { get; set; }

        public virtual Image Image { get; set; }
        public Guid ImageId { get; set; }

        public WebshopFurnitureUnitImage()
        {

        }
    }
}
