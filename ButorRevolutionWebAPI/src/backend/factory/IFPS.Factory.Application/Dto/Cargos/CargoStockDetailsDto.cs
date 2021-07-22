using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CargoStockDetailsDto
    {
        public CargoDetailsByAllInformationDto CargoDetails { get; set; }

        public CargoStockWithAllInformationDto MoreCargoDetails { get; set; }

        public CargoStockDetailsDto(Cargo cargo)
        {
            CargoDetails = new CargoDetailsByAllInformationDto(cargo);
            MoreCargoDetails = new CargoStockWithAllInformationDto(cargo);
        }
    }
}
