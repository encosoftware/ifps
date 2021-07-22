using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    internal class DayTypeAppService : ApplicationService, IDayTypeAppService
    {
        private readonly DayTypeEnum[] WeekdaysEnums =  { DayTypeEnum.Monday,DayTypeEnum.Tuesday, DayTypeEnum.Wednesday,
            DayTypeEnum.Thursday, DayTypeEnum.Friday, DayTypeEnum.Saturday, DayTypeEnum.Sunday};


        private readonly IDayTypeRepository dayTypeRepository;
        public DayTypeAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IDayTypeRepository dayTypeRepository) : base(aggregate)
        {
            this.dayTypeRepository = dayTypeRepository;
        }

        public async Task<List<DayTypeListDto>> GetDayTypesAsync()
        {
            var dayTypes = await dayTypeRepository.GetAllListAsync();
            return dayTypes.Select(ent => new DayTypeListDto(ent)).ToList();
        }

        public async Task<List<DayTypeListDto>> GetWeekDaysAsync()
        {
            var dayTypes = await dayTypeRepository.GetAllListAsync();

            return dayTypes.Where(ent => WeekdaysEnums.Contains(ent.Type))
                    .Select(ent => new DayTypeListDto(ent)).ToList();
        }
    }
}
