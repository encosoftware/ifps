using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class TempCargoDetailsForRequiredMaterialsDto
    {
        public CargoDetailBeforeSaveCargoDto CargoDetailsBeforeSaveCargo { get; set; }

        public List<MaterialsListDto> Materials { get; set; }

        public List<AdditionalsListDto> Additionals { get; set; } 

        public TempCargoDetailsForRequiredMaterialsDto(double vat)
        {
            CargoDetailsBeforeSaveCargo = new CargoDetailBeforeSaveCargoDto(vat);
            Materials = new List<MaterialsListDto>();
            Additionals = new List<AdditionalsListDto>();
        }
    }
}
