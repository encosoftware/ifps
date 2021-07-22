using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class StorageCellFilterDto : OrderedPagedRequestDto
    {
        public string Name { get; set; }
        public string StorageName { get; set; }
        public string Description { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(Name), nameof(StorageCell.Name) }
            };
        }
    }
}
