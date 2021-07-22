using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CargoUserListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CargoUserListDto(User user)
        {
            Id = user.Id;
            Name = user.CurrentVersion.Name;
        }
    }
}
