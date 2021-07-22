using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IFPS.Sales.Application.Dto.Users
{
    public class UserDropdownAvatarDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ImageThumbnailDetailsDto Image { get; set; }

        public static Expression<Func<Order, UserDropdownAvatarDto>> CustomerProjection
        {
            get
            {
                return x => new UserDropdownAvatarDto
                {
                    Id = x.Customer.UserId,
                    Name = x.Customer.User.CurrentVersion.Name,
                    Image = new ImageThumbnailDetailsDto(x.Customer.User.Image)
                };
            }
        }
    }
}
