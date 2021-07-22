using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class GroupingCategory : FullAuditedAggregateRoot, IMultiLingualEntity<GroupingCategoryTranslation>
    {
        /// <summary>
        /// There is an opportunity to create an hierarchical tree of groups
        /// </summary>
        public virtual GroupingCategory ParentGroup { get; set; }
        public int? ParentGroupId { get; set; }

        public virtual Image Image { get; set; }
        public Guid? ImageId { get; set; }

        public ICollection<GroupingCategory> Children { get; set; }

        /// <summary>
        /// Type of the category, which indicates, what kind of items are represented by.
        /// </summary>
        public GroupingCategoryEnum CategoryType { get; set; }

        private List<GroupingCategoryTranslation> _translations;
        public ICollection<GroupingCategoryTranslation> Translations => _translations.AsReadOnly();

        public GroupingCategoryTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        private GroupingCategory()
        {
            _translations = new List<GroupingCategoryTranslation>();
            Children = new List<GroupingCategory>();
        }

        public GroupingCategory(GroupingCategoryEnum groupType) : this()
        {
            if (groupType == GroupingCategoryEnum.None || groupType == GroupingCategoryEnum.Other)
            {
                throw new IFPSDomainException($"Invalid GroupingCategoryEnum type: {groupType}");
            }

            CategoryType = groupType;
        }

        public GroupingCategory(GroupingCategory parent) : this()
        {
            Ensure.NotNull(parent);

            CategoryType = parent.CategoryType;
            ParentGroup = parent;
        }

        public void AddTranslation(GroupingCategoryTranslation translation)
        {
            Ensure.NotNull(translation);

            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding GroupingCategoryTranslation: duplicate language: {translation.Language}");
            }
                        
            _translations.Add(translation);
        }
    }
}
