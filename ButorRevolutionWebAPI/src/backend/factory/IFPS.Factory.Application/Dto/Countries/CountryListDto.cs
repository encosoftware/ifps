using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CountryListDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Translation { get; set; }


        public CountryListDto(Country country)
        {
            Id = country.Id;
            Code = country.Code;
            Translation = country.CurrentTranslation?.Name;
        }
    }
}
