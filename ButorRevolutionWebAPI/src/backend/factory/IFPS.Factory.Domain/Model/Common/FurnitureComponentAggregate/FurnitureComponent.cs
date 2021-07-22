using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Factory.Domain.Enums;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
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
        /// The centerpoint's location in the unit.
        /// </summary>
        public AbsolutePoint CenterPoint { get; set; }

        /// <summary>
        /// The positnion type of the component.
        /// </summary>
        public ComponentPositionTypeEnum PositionType { get; set; }

        /// <summary>
        /// The sequences which are member of the component.
        /// </summary>
        private List<Sequence> _sequences;
        public IEnumerable<Sequence> Sequences => _sequences.AsReadOnly();

        /// <summary>
        /// Picture of the furniture component
        /// </summary>
        public Image Image { get; set; }
        public Guid? ImageId { get; set; }

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

        public void AddSequence(Sequence sequence)
        {
            Ensure.NotNull(sequence);
            _sequences.Add(sequence);
        }

        private FurnitureComponent()
        {
            Id = Guid.NewGuid();
            _sequences = new List<Sequence>();
        }

        public FurnitureComponent(string name, double width, double length, int amount) : this()
        {
            Name = name;
            Width = width;
            Length = length;
            Amount = amount;
        }

        public FurnitureComponent(string name, double width, double length) : this()
        {
            Name = name;
            Width = width;
            Length = length;
        }

        public FurnitureComponent(Guid furnitureUnitId, string name, double width, int amount) : this()
        {
            FurnitureUnitId = furnitureUnitId;
            Name = name;
            Width = width;
            Amount = amount;
        }

        public FurnitureComponent(string name, int amount) : this()
        {
            Name = name;
            Amount = amount;
        }

        public double CalculateDistance()
        {
            double distance = 0;
            foreach (Sequence seq in Sequences)
            {
                distance += seq.GetDistance(Width,Length); // TODO width-height csere
            }
            return 0;
        }
    }
}
