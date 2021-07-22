using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Domain.Dbos
{
    public class UserAvatarDbo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Image Image { get; set; }

        public static Expression<Func<User, UserAvatarDbo>> Projection
        {
            get
            {
                return x => new UserAvatarDbo
                {
                    Id = x.Id,
                    Name = x.CurrentVersion.Name,
                    Image = x.Image
                };
            }
        }

    }
}
