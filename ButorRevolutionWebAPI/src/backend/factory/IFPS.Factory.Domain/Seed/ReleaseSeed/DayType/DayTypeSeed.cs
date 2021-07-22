using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class DayTypeSeed : IEntitySeed<DayType>
    {
        public DayType[] Entities => new[]
        {
            new DayType(DayTypeEnum.Monday, 9) {Id = 1},
            new DayType(DayTypeEnum.Tuesday, 8) {Id = 2},
            new DayType(DayTypeEnum.Wednesday, 7) {Id = 3},
            new DayType(DayTypeEnum.Thursday, 6) {Id = 4},
            new DayType(DayTypeEnum.Friday, 5) {Id = 5},
            new DayType(DayTypeEnum.Saturday, 4) {Id = 6},
            new DayType(DayTypeEnum.Sunday, 3) {Id = 7},
            new DayType(DayTypeEnum.Weekdays, 2) {Id = 8},
            new DayType(DayTypeEnum.Weekend, 1) {Id = 9}
        };
    }
}
