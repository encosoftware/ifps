using IFPS.Factory.Domain.Dbos;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class CargoListByStockDto
    {
        public int Id { get; set; }

        public string CargoName { get; set; }
        public CargoStatusTypeListDto Status { get; set; }
        public DateTime? ArrivedOn { get; set; }
        public SupplierCompanyListDto SupplierName { get; set; }
        public CargoUserListDto BookedByUser { get; set; }

        public CargoListByStockDto()
        {

        }

        public static Func<Cargo, CargoListByStockDto> FromEntity = entity => new CargoListByStockDto
        {
            Id = entity.Id,
            CargoName = entity.CargoName,
            Status = new CargoStatusTypeListDto(entity.Status),
            ArrivedOn = entity.ArrivedOn,
            SupplierName = new SupplierCompanyListDto(entity.Supplier),
            BookedByUser = new CargoUserListDto(entity.BookedBy),
        };

    }
}
