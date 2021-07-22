using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Exceptions;
using IFPS.Factory.Domain.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class FurnitureUnit : FullAuditedAggregateRoot<Guid>, IPricedEntity<FurnitureUnitPrice>, IMultiLingualEntity<FurnitureUnitTranslation>
    {
        /// <summary>
        /// Unique code of the furniture unit
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Description of the furniture unit
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Picture of the furniture unit
        /// </summary>
        public Image Image { get; set; }
        public Guid? ImageId { get; set; }

        /// <summary>
        /// Width parameter of the furniture unit
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Height parameter of the furniture unit
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Depth parameter of the furniture unit
        /// </summary>
        public double Depth { get; set; }

        /// <summary>
        /// Hierarchy level of the unit in the webshop
        /// </summary>
        public virtual GroupingCategory Category { get; set; }
        public int? CategoryId { get; set; }

        /// <summary>
        /// The user has already uploaded cnc files for furniture components
        /// </summary>
        public bool HasCncFile { get; set; }

        /// <summary>
        /// If the furniture unit is ordered, a full copy is created from it. 
        /// The base furniture unit is stored in this property
        /// </summary>
        public FurnitureUnit BaseFurnitureUnit { get; set; }
        public Guid? BaseFurnitureUnitId { get; set; }

        private List<FurnitureUnitTranslation> _translations;
        public ICollection<FurnitureUnitTranslation> Translations
        {
            get
            {
                return _translations;
            }
            set
            {
                if (value == null)
                {
                    throw new IFPSDomainException($"Error setting Translations, value is null.");
                }
                _translations = value.ToList();
            }
        }

        public FurnitureUnitTranslation CurrentTranslation => (FurnitureUnitTranslation)Translations.GetCurrentTranslation();

        private List<FurnitureComponent> _components;
        public IEnumerable<FurnitureComponent> Components => _components.AsReadOnly();

        private List<AccessoryMaterialFurnitureUnit> _accessories;
        public IEnumerable<AccessoryMaterialFurnitureUnit> Accessories => _accessories.AsReadOnly();

        private List<FurnitureUnitPrice> _prices;
        public IEnumerable<FurnitureUnitPrice> Prices => _prices.AsReadOnly();
        public int? CurrentPriceId { get; set; }
        public FurnitureUnitPrice CurrentPrice { get; set; }

        private FurnitureUnit()
        {
            Id = Guid.NewGuid();
            _translations = new List<FurnitureUnitTranslation>();
            _components = new List<FurnitureComponent>();
            _accessories = new List<AccessoryMaterialFurnitureUnit>();
            _prices = new List<FurnitureUnitPrice>();
        }
        public FurnitureUnit(string code, double width, double height, double depth) : this()
        {
            Code = code;
            Width = width;
            Height = height;
            Depth = depth;
        }

        public FurnitureUnit(string code, string description) : this()
        {
            Code = code;
            Description = description;
        }

        public FurnitureUnit(string code) : this()
        {
            Code = code;
        }

        public void AddTranslation(FurnitureUnitTranslation translation)
        {
            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding FurnitureUnitTranslation: duplicate language: {translation.Language}");
            }

            _translations.Add(translation);
        }

        /// <summary>
        /// Add new furniture component
        /// </summary>
        /// <param name="furnitureComponent"></param>
        public void AddFurnitureComponent(FurnitureComponent furnitureComponent)
        {
            Ensure.NotNull(furnitureComponent);
            _components.Add(furnitureComponent);
        }

        /// <summary>
        /// Add new accessory
        /// </summary>
        /// <param name="furnitureComponent"></param>
        public void AddAccessory(AccessoryMaterialFurnitureUnit accessoryMaterialFurnitureUnit)
        {
            Ensure.NotNull(accessoryMaterialFurnitureUnit);
            _accessories.Add(accessoryMaterialFurnitureUnit);
        }

        /// <summary>
        /// Add a new price. The previous price will be unvalidated automatically
        /// </summary>
        /// <param name="newPrice"></param>
        public void AddPrice(FurnitureUnitPrice newPrice)
        {
            Ensure.NotNull(newPrice);

            if (_prices.Any(ent => ent.ValidTo == null))
            {
                throw new IFPSDomainException("Exception at retrieving valid price version");
            }

            if (CurrentPrice != null)
            {
                CurrentPrice.SetValidTo(Clock.Now);
                _prices.Add(CurrentPrice);
                CurrentPrice = null;
            }

            newPrice.SetValidTo(null);
            CurrentPrice = newPrice;
        }
    }
}
