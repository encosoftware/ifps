using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class InspectionUserListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public InspectionUserListDto(User user)
        {
            Id = user.Id;
            Name = user.CurrentVersion.Name;
        }
    }
}
