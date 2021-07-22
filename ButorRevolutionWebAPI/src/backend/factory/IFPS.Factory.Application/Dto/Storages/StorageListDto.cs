using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class StorageListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressDetailsDto Address { get; set; }

        public StorageListDto() { }

        public static Expression<Func<Storage, StorageListDto>> Projection
        {
            get
            {
                return entity => new StorageListDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Address = new AddressDetailsDto(entity.Address)
                };
            }
        }
    }
}
