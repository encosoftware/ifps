using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class UserListDto
    {
        private ICollection<string> _roles = new List<string>();

        public int Id { get; set; }
        public string Name { get; set; }
        public string Role => string.Join(", ", _roles.ToArray());
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime AddedOn { get; set; }
        public bool IsActive { get; set; }
        public ImageThumbnailListDto Image { get; set; }

        public UserListDto() {}

        public UserListDto(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.CurrentVersion.Name;
            Phone = user.CurrentVersion.Phone;
            AddedOn = user.CreationTime;
            IsActive = user.IsActive;
            Image = user.Image != null ? new ImageThumbnailListDto(user.Image) : null;
            Company = user.Company?.Name;
            _roles = user.Roles.Select(x => x.Role.CurrentTranslation.Name).ToList();
        }

        public static Func<User, UserListDto> FromEntity = entity => new UserListDto(entity) { };

        public void AddRole(string roleName)
        {
            _roles.Add(roleName);
        }
    }
}