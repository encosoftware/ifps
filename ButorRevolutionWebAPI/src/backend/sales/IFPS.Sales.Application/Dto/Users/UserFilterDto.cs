using ENCO.DDD.Application.Dto;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class UserFilterDto : OrderedPagedRequestDto
    {
        public string Name { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public DateTime? AddedOnFrom { get; set; }
        public DateTime? AddedOnTo { get; set; }

        public static Dictionary<string, string> GetColumnMappings()
        {
            var columnMappings = new Dictionary<string, string>
            {
                { nameof(Name), nameof(User.CurrentVersion.Name) },
                { nameof(Email), nameof(User.Email) },
                { nameof(IsActive), nameof(User.IsActive) },
                { nameof(PhoneNumber), nameof(User.CurrentVersion.Phone)},
                { nameof(CompanyName), nameof(User.Company.Name)}
            };

            return columnMappings;
        }
    }
}
        