using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class GroupingCategoryUpdateDto
    {        
        public IList<GroupingCategoryTranslationUpdateDto> Translations { get; set; }
        public int ParentGroupId { get; set; }
    }
}
