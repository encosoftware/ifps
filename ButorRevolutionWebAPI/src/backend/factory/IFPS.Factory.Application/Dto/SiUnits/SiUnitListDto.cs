using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class SiUnitListDto
    {
        public SiUnitEnum UnitType { get; set; }

        public SiUnitListDto(SiUnit siUnit)
        {
            UnitType = siUnit.UnitType;
        }
    }
}