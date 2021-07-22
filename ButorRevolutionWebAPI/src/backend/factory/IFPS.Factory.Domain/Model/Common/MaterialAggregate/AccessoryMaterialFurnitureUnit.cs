using ENCO.DDD.Domain.Model.Entities;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class AccessoryMaterialFurnitureUnit : AggregateRoot
    {
        public virtual AccessoryMaterial Accessory { get; set; }
        public Guid AccessoryId { get; set; }
        public virtual FurnitureUnit FurnitureUnit { get; set; }
        public Guid FurnitureUnitId { get; set; }
        public int AccessoryAmount { get; set; }
        public string Name { get; set; }

        public AccessoryMaterialFurnitureUnit(Guid furnitureUnitId, Guid accessoryId)
        {
            FurnitureUnitId = furnitureUnitId;
            AccessoryId = accessoryId;
        }

        public AccessoryMaterialFurnitureUnit(Guid furnitureUnitId, Guid accessoryId, string name, int amount) : this(furnitureUnitId, accessoryId)
        {
            Name = name;
            AccessoryAmount = amount;
        }
    }
}
