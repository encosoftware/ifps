using ENCO.DDD.Domain.Model.Enums;
using System.Collections.Generic;
using System.Security.Claims;

namespace IFPS.Sales.Application.Dto
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public AddressDetailsDto Address { get; set; }
        public int? CompanyId { get; set; }
        public LanguageTypeEnum Language { get; set; }
        public ImageThumbnailDetailsDto Image { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        public int? BasketId { get; set; }

        public AccountDto()
        {
            Claims = new List<Claim>();
        }
    }
}
