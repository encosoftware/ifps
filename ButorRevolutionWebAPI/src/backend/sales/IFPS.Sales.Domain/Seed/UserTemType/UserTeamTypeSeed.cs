using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class UserTeamTypeSeed : IEntitySeed<UserTeamType>
    {
        public UserTeamType[] Entities => new[]
        {
            new UserTeamType(UserTeamTypeEnum.ShippingGroup) { Id = 1 },
            new UserTeamType(UserTeamTypeEnum.InstallationGroup) { Id = 2 },
            new UserTeamType(UserTeamTypeEnum.SurveyGroup) { Id = 3 }
        };
    }
}
