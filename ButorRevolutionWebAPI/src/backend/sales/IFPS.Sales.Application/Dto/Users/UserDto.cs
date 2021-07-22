using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public ImageThumbnailListDto Image { get; set; }

        public UserDto()
        {

        }
        public UserDto(User user)
        {
            Id = user.Id;
            Name = user.CurrentVersion.Name;
            PhoneNumber = user.CurrentVersion.Phone;
            Email = user.Email;
            Image = user.Image != null ? new ImageThumbnailListDto(user.Image) : null;
        }

    }
}
