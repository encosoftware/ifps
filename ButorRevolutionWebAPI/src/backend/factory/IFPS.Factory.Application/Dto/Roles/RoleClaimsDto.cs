using System.Collections.Generic;
using System.Linq;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class RoleClaimsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ClaimDto> Claims { get; set; }

        public RoleClaimsDto(Role role)
        {
            Id = role.Id;
            Name = role.Name;
            Claims = role.DefaultRoleClaims.Select(ent=> new ClaimDto(ent.Claim)).ToList();
        }
    }
}
