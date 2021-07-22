using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CargoStatusTypeListDto
    {
        public int Id { get; set; }
        public CargoStatusEnum CargoStatus { get; set; }
        public string Translation { get; set; }

        public CargoStatusTypeListDto(CargoStatusType cargoStatusType)
        {
            Id = cargoStatusType.Id;
            CargoStatus = cargoStatusType.Status;
            Translation = cargoStatusType.CurrentTranslation.Name;
        }
    }
}
