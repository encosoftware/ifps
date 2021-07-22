using ENCO.DDD.Domain.Model.Entities;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class AccessoryMaterialFurnitureUnit : AggregateRoot
    {
        public virtual AccessoryMaterial Accessory { get; set; }
        public Guid AccessoryId { get; set; }
        public virtual FurnitureUnit FurnitureUnit { get; set; }
        public Guid FurnitureUnitId { get; set; }
        public int AccessoryAmount { get; set; }
        public string Name { get; set; }

        public AccessoryMaterialFurnitureUnit()
        {

        }

        public AccessoryMaterialFurnitureUnit(string name, int amount)
        {
            Name = name;
            AccessoryAmount = amount;
        }
    }
}
