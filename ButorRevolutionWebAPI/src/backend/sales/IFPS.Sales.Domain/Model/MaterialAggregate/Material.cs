using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Exceptions;
using IFPS.Sales.Domain.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class Material : FullAuditedAggregateRoot<Guid>, IMultiLingualEntity<MaterialTranslation>, IPricedEntity<MaterialPrice>
    {
        /// <summary>
        /// Unique code of the material
        /// </summary>  
        public string Code { get; set; }

        /// <summary>
        /// Description of the material
        /// </summary>  
        public string Description { get; set; }

        /// <summary>
        /// Picture of the material
        /// </summary>
        public Image Image { get; set; }
        public Guid? ImageId { get; set; }


        /// <summary>
        /// Integer, which indicates how much percent of the price is calculated as transportation and logistics overhead
        /// </summary>
        public int TransactionMultiplier { get; set; }

        /// <summary>
        /// Category the material belongs to
        /// </summary>
        public virtual GroupingCategory Category { get; set; }
        public int? CategoryId { get; set; }

        private List<MaterialTranslation> _translations;
        public virtual ICollection<MaterialTranslation> Translations
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

        public MaterialTranslation CurrentTranslation => (MaterialTranslation)Translations.GetCurrentTranslation();

        private List<MaterialPrice> _prices;
        public IEnumerable<MaterialPrice> Prices => _prices.AsReadOnly();

        public int? CurrentPriceId { get; set; }
        public MaterialPrice CurrentPrice { get; set; }

        protected Material()
        {
            Id = Guid.NewGuid();
            _translations = new List<MaterialTranslation>();
            _prices = new List<MaterialPrice>();
        }

        public Material(string code, int transactionMultiplier = 10) : this()
        {
            Code = code;
            TransactionMultiplier = transactionMultiplier;
        }


        /// <summary>
        /// Add a new price. The previous price will be unvalidated automatically
        /// </summary>
        /// <param name="newPrice"></param>
        public void AddPrice(MaterialPrice newPrice)
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
