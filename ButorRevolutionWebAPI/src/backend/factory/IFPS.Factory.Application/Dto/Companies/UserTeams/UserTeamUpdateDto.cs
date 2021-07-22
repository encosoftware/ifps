using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class UserTeamUpdateDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public List<int> UserIds { get; set; }

        public UserTeamUpdateDto()
        {
        }
    }
}