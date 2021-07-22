using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class UserProfileUpdateDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AddressCreateDto Address { get; set; }
        public ImageCreateDto Image { get; set; }
        public LanguageTypeEnum Language { get; set; }

        public User UpdateModelObject(User user)
        {
            user.Email = Email;
            user.Language = Language;
            return user;
        }
    }
}