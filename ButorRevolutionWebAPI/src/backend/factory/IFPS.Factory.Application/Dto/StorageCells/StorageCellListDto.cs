using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class StorageCellListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StorageName { get; set; }
        public string Description { get; set; }

        public StorageCellListDto() { }

        public static Expression<Func<StorageCell, StorageCellListDto>> Projection
        {
            get
            {
                return entity => new StorageCellListDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    StorageName = entity.Warehouse.Name,
                    Description = entity.Metadata
                };
            }
        }
    }
}
