using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class UserProfileUpdateDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AddressCreateDto Address { get; set; }
        public ImageCreateDto Image { get; set; }
        public LanguageTypeEnum Language { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        public User UpdateModelObject(User user)
        {
            user.Email = Email;
            user.Language = Language;
            return user;
        }
    }
}