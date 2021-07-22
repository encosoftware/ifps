using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CargoDetailsWithAllInformationDto
    {
        public CargoDetailsByAllInformationDto CargoDetails { get; set; }

        public CargoListDetailsByAllInformationDto CargoListDetails { get; set; }

        public CargoDetailsWithAllInformationDto(Cargo cargo, double vat)
        {
            CargoDetails = new CargoDetailsByAllInformationDto(cargo);
            CargoListDetails = new CargoListDetailsByAllInformationDto(cargo, vat);
        }
    }
}
