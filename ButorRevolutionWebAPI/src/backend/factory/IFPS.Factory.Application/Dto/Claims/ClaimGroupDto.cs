using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class ClaimGroupDto
    {
        public string Name { get; set; }
        public string Detail { get; set; }
        public DivisionTypeEnum Division { get; set; }
        public List<ClaimDto> Claims { get; set; }

        public ClaimGroupDto(Division division)
        {
            Name = division.CurrentTranslation?.Name;
            Detail = division.CurrentTranslation?.Description;
            Division = division.DivisionType;
            Claims = division.Claims.Select(ent => new ClaimDto(ent)).ToList();
        }
    }
}