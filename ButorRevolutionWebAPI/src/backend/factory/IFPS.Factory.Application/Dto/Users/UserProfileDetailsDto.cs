using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class UserProfileDetailsDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AddressDetailsDto Address { get; set; }
        public ImageDetailsDto Image { get; set; }
        public LanguageTypeEnum Language { get; set; }

        public static UserProfileDetailsDto FromModel(User user)
        {
            return new UserProfileDetailsDto
            {
                Name = user.CurrentVersion.Name,
                PhoneNumber = user.CurrentVersion.Phone,
                Email = user.Email,
                Address = !user.CurrentVersion.ContactAddress.IsEmpty() ? new AddressDetailsDto(user.CurrentVersion.ContactAddress) : null,
                Image = user.Image != null ? new ImageDetailsDto(user.Image) : null,
                Language = user.Language
            };
        }
    }
}