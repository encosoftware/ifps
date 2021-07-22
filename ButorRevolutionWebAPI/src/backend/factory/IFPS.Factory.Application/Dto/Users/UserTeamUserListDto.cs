using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class UserTeamUserListDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }

        public UserTeamUserListDto(User user)
        {
            Name = user.CurrentVersion.Name;
        }
    }
}