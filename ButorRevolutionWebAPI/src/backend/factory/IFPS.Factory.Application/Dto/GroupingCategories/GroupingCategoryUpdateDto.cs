using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class GroupingCategoryUpdateDto
    {
        public IList<GroupingCategoryTranslationUpdateDto> Translations { get; set; }
    }
}