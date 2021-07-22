using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class RoleListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DivisionListDto Division { get; set; }

        public RoleListDto(Role role)
        {
            Id = role.Id;
            Name = role.CurrentTranslation?.Name;
            Division = new DivisionListDto(role.Division);
        }
    }
}