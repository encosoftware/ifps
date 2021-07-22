using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class RoleDetailsDto
    {
        public List<int> ClaimIds { get; set; }

        public RoleDetailsDto(Role role)
        {
            ClaimIds = role.DefaultRoleClaims.Select(ent => ent.ClaimId).ToList();
        }
    }
}