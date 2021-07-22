using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class StorageFilterDto : OrderedPagedRequestDto
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(Name), nameof(Storage.Name) }
            };
        }

        public static Expression<Func<Storage, StorageFilterDto>> GetProjection()
        {
            return x => new StorageFilterDto
            {
                Name = x.Name,
                Address = x.Address.ToString()
            };
        }
    }
}
