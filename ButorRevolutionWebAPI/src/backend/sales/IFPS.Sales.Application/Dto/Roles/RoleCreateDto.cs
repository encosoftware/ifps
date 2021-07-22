using IFPS.Sales.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class RoleCreateDto
    {
        public string Name { get; set; }
        public int DivisionId{ get; set; }
        public List<int> ClaimIdList { get; set; }

        public Role CreateModelObject()
        {
            return new Role(Name, DivisionId);
        }
    }
}
