using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class EmployeeListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Role { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public EmployeeListDto(User user)
        {
            Id = user.Id;
            Name = user.CurrentVersion?.Name;
            Email = user.Email;
            Phone = user.CurrentVersion?.Phone;
            Role = user.Roles?.Select(ent => ent.Role.Name).ToList();
        }

        public EmployeeListDto() { }

        public static Expression<Func<User, EmployeeListDto>> Projection
        {
            get
            {
                return x => new EmployeeListDto
                {
                    Id = x.Id,
                    Name = x.CurrentVersion.Name,
                    Email = x.Email,
                    Phone = x.CurrentVersion.Phone,
                    Role = x.Roles.Select(ent => ent.Role.Name).ToList()
                };
            }
        }
    }
}