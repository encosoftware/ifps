using IFPS.Sales.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class UserTeamListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserDto> Users { get; set; }
        public string UserTeamTypeName { get; set; }

        public UserTeamListDto()
        {

        }

        public static UserTeamListDto FromModel(UserTeam userTeam)
        {
            return new UserTeamListDto()
            {
                Name = userTeam.TechnicalUser.CurrentVersion.Name,
                UserTeamTypeName = userTeam.UserTeamType.CurrentTranslation.Name
            };
        }
    }
}
