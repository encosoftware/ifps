using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using LinqKit;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class CncMachineAppService : ApplicationService, ICncMachineAppService
    {
        private readonly ICncMachineRepository cncMachineRepository;

        public CncMachineAppService(
            ICncMachineRepository cncMachineRepository,
            IApplicationServiceDependencyAggregate aggregate) : base(aggregate)
        {
            this.cncMachineRepository = cncMachineRepository;
        }

        public async Task<int> CreateCncMachineAsync(CncMachineCreateDto dto)
        {
            var cncMachine = dto.CreateModelObject();
            await cncMachineRepository.InsertAsync(cncMachine);
            await unitOfWork.SaveChangesAsync();
            return cncMachine.Id;
        }

        public async Task<CncMachineDetailsDto> GetCncMachineByIdAsync(int id)
        {
            var machine = await cncMachineRepository.SingleAsync(ent => ent.Id == id);
            return new CncMachineDetailsDto(machine);
        }

        public async Task<PagedListDto<CncMachineListDto>> GetCncMachinesAsync(CncMachineFilterDto filterDto)
        {
            Expression<Func<CncMachine, bool>> filter = (CncMachine ent) => true;
            
            filter = AddOptionsToFilter(filterDto, filter);
            var orderingQuery = filterDto.Orderings.ToOrderingExpression<CncMachine>(MachineFilterDto.GetOrderingMapping(), nameof(CncMachine.Id));

            var machines = await cncMachineRepository.GetPagedListAsync(filter, CncMachineListDto.Projection, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return machines.ToPagedList();
        }

        public async Task UpdateCncMachineAsync(int id, CncMachineUpdateDto dto)
        {
            var cncMachine = await cncMachineRepository.SingleAsync(ent => ent.Id == id);
            cncMachine = dto.UpdateModelObject(cncMachine);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCncMachineAsync(int id)
        {
            await cncMachineRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        private static Expression<Func<CncMachine, bool>> AddOptionsToFilter(CncMachineFilterDto filterDto, Expression<Func<CncMachine, bool>> filter)
        {
            if (filterDto != null)
            {
                if (!string.IsNullOrWhiteSpace(filterDto.Code))
                {
                    filter = filter.And(ent => ent.Code.ToLower().Contains(filterDto.Code.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(filterDto.MachineName))
                {
                    filter = filter.And(ent => ent.Name.ToLower().Contains(filterDto.MachineName.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(filterDto.SerialNumber))
                {
                    filter = filter.And(ent => ent.SerialNumber.ToLower().Contains(filterDto.SerialNumber.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(filterDto.SoftwareVersion))
                {
                    filter = filter.And(ent => ent.SoftwareVersion.ToLower().Contains(filterDto.SoftwareVersion.ToLower()));
                }
                if (filterDto.YearOfManufacture > 0)
                {
                    filter = filter.And(ent => ent.YearOfManufacture.Equals(filterDto.YearOfManufacture));
                }
                if (filterDto.BrandId > 0)
                {
                    filter = filter.And(ent => ent.BrandId.Equals(filterDto.BrandId));
                }
            }

            return filter;
        }
    }
}