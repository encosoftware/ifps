using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CargoStatusTypeDropdownListDto
    {
        public int Id { get; set; }
        public string StatusName { get; set; }

        public CargoStatusTypeDropdownListDto(CargoStatusType cargoStatusType)
        {
            Id = cargoStatusType.Id;
            StatusName = cargoStatusType.CurrentTranslation.Name;
        }
    }
}
