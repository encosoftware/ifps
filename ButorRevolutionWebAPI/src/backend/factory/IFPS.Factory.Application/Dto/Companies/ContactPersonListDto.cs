using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class ContactPersonListDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public ContactPersonListDto(User user)
        {
            Id = user?.Id;
            Email = user?.Email;
            Name = user?.CurrentVersion?.Name;
            PhoneNumber = user?.CurrentVersion?.Phone;
        }

        public ContactPersonListDto()
        {
        }
    }
}