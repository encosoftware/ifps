using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class SortingStrategyTypeListDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

        public SortingStrategyTypeListDto(SortingStrategyType type)
        {
            Id = type.Id;
            TypeName = type.CurrentTranslation?.Name;
        }
    }
}
