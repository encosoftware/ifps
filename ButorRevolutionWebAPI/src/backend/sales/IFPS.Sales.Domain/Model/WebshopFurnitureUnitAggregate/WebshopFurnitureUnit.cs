using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Domain.Model
{
    public class WebshopFurnitureUnit : FullAuditedAggregateRoot
    {
        public virtual FurnitureUnit FurnitureUnit { get; set; }
        public Guid FurnitureUnitId { get; set; }

        public Price Price { get; set; }
        public double Value { get; set; }


        private List<WebshopFurnitureUnitImage> _images;
        public IEnumerable<WebshopFurnitureUnitImage> Images => _images.AsReadOnly();

        private WebshopFurnitureUnit()
        {
            _images = new List<WebshopFurnitureUnitImage>();
        }

        public WebshopFurnitureUnit(Guid FurnitureUnitId) : this()
        {
            this.FurnitureUnitId = FurnitureUnitId;
        }

        public void AddWebshopFurnitureUnitImage(WebshopFurnitureUnitImage image)
        {
            Ensure.NotNull(image);
            _images.Add(image);
        }

        public void RemoveWebshopFurnitureUnitImage(WebshopFurnitureUnitImage image)
        {
            Ensure.NotNull(image);
            _images.Remove(image);
        }
    }
}
