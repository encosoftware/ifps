using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
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
