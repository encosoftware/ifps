using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class UserTeamListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserDto> Users { get; set; }

        public UserTeamListDto()
        {
        }

        public static UserTeamListDto FromModel(UserTeam userTeam)
        {
            return new UserTeamListDto()
            {
                Name = userTeam.TechnicalUser.CurrentVersion.Name,
            };
        }
    }
}