using ENCO.DDD.Domain.Model.Enums;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class UserUpdateDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AddressCreateDto Address { get; set; }
        public bool? isEmpleoyee { get; set; }
        public LanguageTypeEnum Language { get; set; }        
        public List<int> OwnedRolesIds { get; set; }
        public List<int> OwnedClaimsIds { get; set; }
        public int? CompanyId { get; set; }
        public UpdateWorkingInfoDto WorkingInfo { get; set; }
        public NotificationsDto Notifications { get; set; }
        public ImageUpdateDto ImageUpdateDto { get; set; }
    }
}
