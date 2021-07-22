using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class GroupingCategory : FullAuditedAggregateRoot, IMultiLingualEntity<GroupingCategoryTranslation>
    {
        /// <summary>
        /// There is an opportunity to create an hierarchical tree of groups
        /// </summary>
        public virtual GroupingCategory ParentGroup { get; set; }
        public int? ParentGroupId { get; set; }

        public ICollection<GroupingCategory> Children { get; set; }

        /// <summary>
        /// Type of the category, which indicates, what kind of items are represented by.
        /// </summary>
        public GroupingCategoryEnum CategoryType { get; set; }

        public Image Image { get; set; }
        public Guid? ImageId { get; set; }

        private List<GroupingCategoryTranslation> _translations;
        public ICollection<GroupingCategoryTranslation> Translations
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

        public GroupingCategoryTranslation CurrentTranslation => (GroupingCategoryTranslation)Translations.GetCurrentTranslation();

        private GroupingCategory()
        {
            this._translations = new List<GroupingCategoryTranslation>();
            this.Children = new List<GroupingCategory>();
        }

        public GroupingCategory(GroupingCategoryEnum groupType) : this()
        {
            if (groupType == GroupingCategoryEnum.None || groupType == GroupingCategoryEnum.Other)
            {
                throw new IFPSDomainException($"Invalid GroupingCategoryEnum type: {groupType}");
            }

            this.CategoryType = groupType;
        }

        public GroupingCategory(GroupingCategory parent) : this()
        {
            Ensure.NotNull(parent);

            this.CategoryType = parent.CategoryType;
            this.ParentGroup = parent;
        }

        public void AddTranslation(GroupingCategoryTranslation translation)
        {
            Ensure.NotNull(translation);

            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding GroupingCategoryTranslation: duplicate language: {translation.Language}");
            }
                        
            this._translations.Add(translation);
        }
    }
}
