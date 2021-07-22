using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class UserDetailsDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AddressDetailsDto Address { get; set; }
        public LanguageTypeEnum Language { get; set; }
        public bool IsActive { get; set; }
        public List<int> OwnedRolesIds { get; set; }
        public List<int> OwnedClaimsIds { get; set; }
        public CompanyDto Company { get; set; }
        public bool IsEmployee { get; set; }
        public ImageDetailsDto Image { get; set; }

        public static UserDetailsDto FromModel(User user)
        {
            return new UserDetailsDto
            {
                Id = user.Id,
                Name = user.CurrentVersion.Name,
                Email = user.Email,
                PhoneNumber = user.CurrentVersion.Phone,
                Address = !user.CurrentVersion.ContactAddress.IsEmpty() ? new AddressDetailsDto(user.CurrentVersion.ContactAddress) : null,
                Language = user.Language,
                IsActive = user.IsActive,
                OwnedRolesIds = user.Roles.Select(x => x.RoleId).ToList(),
                OwnedClaimsIds = user.Claims.Select(x => x.ClaimId).ToList(),
                Company = user.Company != null ? CompanyDto.FromModel(user.Company) : null,
                Image = user.Image != null ? new ImageDetailsDto(user.Image) : null,
            };
        }
    }
}