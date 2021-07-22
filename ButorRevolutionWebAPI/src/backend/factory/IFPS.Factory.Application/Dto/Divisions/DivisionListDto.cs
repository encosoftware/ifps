using IFPS.Factory.Domain.Model;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class DivisionListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DivisionListDto(Division division)
        {
            Id = division.Id;
            Name = division.CurrentTranslation.Name;
        }
    }
}