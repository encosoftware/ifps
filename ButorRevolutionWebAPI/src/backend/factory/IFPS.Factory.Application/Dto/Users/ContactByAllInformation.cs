using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class ContactByAllInformation
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public ContactByAllInformation(User contact)
        {
            Name = contact.CurrentVersion.Name;
            Phone = contact.CurrentVersion.Phone;
            Email = contact.Email;
        }
    }
}
