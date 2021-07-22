using IFPS.Sales.Domain.Model;
using System.Linq;

namespace IFPS.Sales.Application.Dto
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