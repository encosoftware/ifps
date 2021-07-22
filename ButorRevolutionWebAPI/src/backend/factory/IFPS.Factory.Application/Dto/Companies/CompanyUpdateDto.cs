using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class CompanyUpdateDto
    {
        public int CompanyTypeId { get; set; }
        public int? ContactPersonId { get; set; }
        public string TaxNumber { get; set; }
        public string RegisterNumber { get; set; }
        public AddressCreateDto Address { get; set; }
        public List<CompanyDateRangeUpdateDto> OpeningHours { get; set; }
        public List<UserTeamUpdateDto> UserTeams { get; set; }

        public CompanyUpdateDto()
        {
        }
    }
}