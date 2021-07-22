using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class UserTeamTypeListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserTeamTypeListDto(UserTeamType userTeamType)
        {
            this.Id = userTeamType.Id;
            this.Name = userTeamType.CurrentTranslation.Name;
        }
    }
}
