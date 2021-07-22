using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Enums;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class FurnitureComponent : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the component (e.g. right door)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Width of the component, measured in millimeters
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Length of the component, measured in millimeters
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Amount of the component
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Description of the furniture unit
        /// </summary>
        public FurnitureComponentTypeEnum Type { get; set; }

        /// <summary>
        /// Picture of the furniture component
        /// </summary>
        public Image Image { get; set; }
        public Guid ImageId { get; set; }

        /// <summary>
        /// Material of the component
        /// </summary>
        public virtual BoardMaterial BoardMaterial { get; set; }
        public Guid? BoardMaterialId { get; set; }

        /// <summary>
        /// Foil material of the top edge
        /// </summary>
        public virtual FoilMaterial TopFoil { get; set; }
        public Guid? TopFoilId { get; set; }

        /// <summary>
        /// Foil material of the right edge
        /// </summary>
        public virtual FoilMaterial RightFoil { get; set; }
        public Guid? RightFoilId { get; set; }

        /// <summary>
        /// Foil material of the bottom edge
        /// </summary>
        public virtual FoilMaterial BottomFoil { get; set; }
        public Guid? BottomFoilId { get; set; }

        /// <summary>
        /// Foil material of the left edge
        /// </summary>
        public virtual FoilMaterial LeftFoil { get; set; }
        public Guid? LeftFoilId { get; set; }

        /// <summary>
        /// Furniture unit, which contains the component
        /// </summary>
        public virtual FurnitureUnit FurnitureUnit { get; set; }
        public Guid FurnitureUnitId { get; set; }

        private FurnitureComponent()
        {
            Id = Guid.NewGuid();
        }

        public FurnitureComponent(string name, double width, double height, int amount) : this()
        {
            Name = name;
            Width = width;
            Length = height;
            Amount = amount;
        }

        public FurnitureComponent(string name, int amount) : this()
        {
            Name = name;
            Amount = amount;
        }
    }
}
