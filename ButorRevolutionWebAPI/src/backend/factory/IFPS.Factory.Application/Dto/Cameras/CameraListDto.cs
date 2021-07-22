using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class CameraListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string IPAddress { get; set; }
        public bool IsActive { get; set; }

        public CameraListDto() { }

        public static Expression<Func<Camera, CameraListDto>> Projection
        {
            get
            {
                return entity => new CameraListDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Type = entity.Type,
                    IPAddress = entity.IPAddress,
                    IsActive = entity.IsActive
                };
            }
        }
    }
}
