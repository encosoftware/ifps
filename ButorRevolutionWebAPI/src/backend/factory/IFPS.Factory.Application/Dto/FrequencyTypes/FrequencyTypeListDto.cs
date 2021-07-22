using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class FrequencyTypeListDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Translation { get; set; }


        public FrequencyTypeListDto(FrequencyType frequencyType)
        {
            Id = frequencyType.Id;
            Translation = frequencyType.CurrentTranslation?.Name;
        }
    }
}
