using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
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