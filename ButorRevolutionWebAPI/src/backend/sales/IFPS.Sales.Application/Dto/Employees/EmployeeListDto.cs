using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
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
            Role = user.Roles?.Select(ent=> ent.Role.Name).ToList();
        }
    }
}
