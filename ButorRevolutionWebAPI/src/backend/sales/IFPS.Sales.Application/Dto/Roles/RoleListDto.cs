using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class RoleListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }

        public RoleListDto(Role role)
        {
            Id = role.Id;
            Name = role.Name;
            DivisionId = role.DivisionId;
            DivisionName = role.Division.CurrentTranslation.Name;
        }
    }
}
