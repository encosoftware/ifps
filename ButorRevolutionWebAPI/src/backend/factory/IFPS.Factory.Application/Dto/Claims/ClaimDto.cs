using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class ClaimDto
    {
        public int Id { get; set; }
        public ClaimPolicyEnum Name { get; set; }
        public DivisionTypeEnum Division { get; set; }

        public ClaimDto(Claim claim)
        {
            Id = claim.Id;
            Name = claim.Name;
            Division = claim.Division.DivisionType;
        }
    }
}