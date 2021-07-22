using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class CameraFilterDto : OrderedPagedRequestDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string IPAddress { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(Name), nameof(Camera.Name) },
                { nameof(Type), nameof(Camera.Type) },
                { nameof(IPAddress), nameof(Camera.IPAddress) }
            };
        }
    }
}
