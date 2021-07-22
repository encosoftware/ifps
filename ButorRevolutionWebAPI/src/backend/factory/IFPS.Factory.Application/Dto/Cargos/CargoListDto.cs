using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class CargoListDto
    {
        public int Id { get; set; }

        public string CargoName { get; set; }
        public CargoStatusTypeListDto Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public SupplierCompanyListDto SupplierName { get; set; }
        public CargoUserListDto BookedByUser { get; set; }

        public Price TotalCost { get; set; }

        public CargoListDto()
        {

        }

        public static Func<Cargo, CargoListDto> FromEntity = entity => new CargoListDto
        {
            Id = entity.Id,
            CargoName = entity.CargoName,
            Status = new CargoStatusTypeListDto(entity.Status),
            CreatedOn = entity.BookedOn,
            SupplierName = new SupplierCompanyListDto(entity.Supplier),
            BookedByUser = new CargoUserListDto(entity.BookedBy),
            TotalCost = entity.NetCost
        };
    }
}
