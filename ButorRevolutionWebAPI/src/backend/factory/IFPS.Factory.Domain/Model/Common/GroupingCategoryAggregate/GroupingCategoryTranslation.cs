using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class GroupingCategoryTranslation : FullAuditedEntity, IEntityTranslation<GroupingCategory>
    {
        /// <summary>
        /// Language specific group name
        /// </summary>
        public string GroupingCategoryName { get; set; }

        public GroupingCategory Core { get; set; }
        public int CoreId { get; set; }

        public LanguageTypeEnum Language { get; set; }

        private GroupingCategoryTranslation() { }

        public GroupingCategoryTranslation(string groupName, LanguageTypeEnum language)
        {
            GroupingCategoryName = groupName;
            Language = language;
        }

        public GroupingCategoryTranslation(int coreId, string groupName, LanguageTypeEnum language)
        {
            CoreId = coreId;
            GroupingCategoryName = groupName;
            Language = language;
        }
    }
}
