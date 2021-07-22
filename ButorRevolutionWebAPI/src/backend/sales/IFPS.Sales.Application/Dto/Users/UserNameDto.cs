using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class UserNameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsTechnicalAccount { get; set; }

        public UserNameDto()
        {
        }

        public static Expression<Func<User, UserNameDto>> Projection
        {
            get
            {
                return user => new UserNameDto
                {
                    Id = user.Id,
                    Name = user.CurrentVersion.Name,
                    IsTechnicalAccount = user.IsTechnicalAccount
                };
            }
        }
    }
}
