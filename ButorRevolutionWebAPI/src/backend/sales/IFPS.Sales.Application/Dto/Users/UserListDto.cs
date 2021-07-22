using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role => string.Join(", ", _roles.ToArray());
        private ICollection<string> _roles = new List<string>();        
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime AddedOn { get; set; }
        public bool IsActive { get; set; }
        public ImageThumbnailListDto Image { get; set; }

        public UserListDto()
        {

        }

        public void AddRole(string roleName)
        {
            _roles.Add(roleName);
        }

        public UserListDto(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.CurrentVersion.Name;
            Phone = user.CurrentVersion.Phone;
            AddedOn = user.CreationTime;
            IsActive = user.IsActive;
            Image = user.Image != null ? new ImageThumbnailListDto(user.Image) : null;
            Company = user.Company.Name;
            foreach (var role in user.Roles)
            {
                _roles.Add(role.Role.Name);
            }
        }

        /// <summary>
        /// Gets how to map the DBO to this DTO
        /// </summary>
        public static Expression<Func<User, UserListDto>> Projection
        {
            get
            {
                return x => new UserListDto
                {
                    Id = x.Id,
                    Name = x.CurrentVersion.Name,
                    Email = x.Email,
                    Phone = x.CurrentVersion.Phone,
                    AddedOn = x.CreationTime,
                    IsActive = x.IsActive,
                    Company = x.Company.Name,
                    _roles = x.Roles.Select(r => r.Role.Name).ToList(),
                    Image = x.Image != null ? new ImageThumbnailListDto(x.Image) : null
                };
            }
        }

        public static Func<User, UserListDto> FromEntity = entity =>
        {
            var dto = new UserListDto
            {
                Id = entity.Id,
                Email = entity.Email,
                Name = entity.CurrentVersion.Name,
                Phone = entity.CurrentVersion.Phone,
                AddedOn = entity.CreationTime,
                IsActive = entity.IsActive,
                Company = entity.Company?.Name,
                Image = entity.Image != null ? new ImageThumbnailListDto(entity.Image) : null,               
            };            

            return dto;
        };
    }
}
