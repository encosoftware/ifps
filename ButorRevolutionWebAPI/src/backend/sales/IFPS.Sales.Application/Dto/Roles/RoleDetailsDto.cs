using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
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
