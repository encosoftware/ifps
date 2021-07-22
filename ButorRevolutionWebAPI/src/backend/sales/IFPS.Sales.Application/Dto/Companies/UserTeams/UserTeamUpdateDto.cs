using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class UserTeamUpdateDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public List<int> UserIds { get; set; }
        public int UserTeamTypeId { get; set; }

        public UserTeamUpdateDto()
        {

        }
    }
}
