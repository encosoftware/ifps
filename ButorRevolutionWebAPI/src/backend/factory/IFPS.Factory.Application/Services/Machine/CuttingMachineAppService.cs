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
    public class CuttingMachineAppService : ApplicationService, ICuttingMachineAppService
    {
        private readonly ICuttingMachineRepository repository;

        public CuttingMachineAppService(
            IApplicationServiceDependencyAggregate aggregate,
            ICuttingMachineRepository repository) : base(aggregate)
        {
            this.repository = repository;
        }

        public async Task<int> CreateCuttingMachineAsync(CuttingMachineCreateDto dto)
        {
            var cuttingMachine = dto.CreateModelObject();
            await repository.InsertAsync(cuttingMachine);
            await unitOfWork.SaveChangesAsync();
            return cuttingMachine.Id;
        }

        public async Task DeleteCuttingMachineAsync(int id)
        {
            await repository.DeleteAsync(ent => ent.Id == id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<CuttingMachineDetailsDto> GetCuttingMachineByIdAsync(int id)
        {
            var machine = await repository.SingleAsync(ent => ent.Id == id);
            return new CuttingMachineDetailsDto(machine);
        }

        public async Task<PagedListDto<CuttingMachineListDto>> GetCuttingMachinesAsync(CuttingMachineFilterDto filterDto)
        {
            Expression<Func<CuttingMachine, bool>> filter = ent => ent.Code != null;

            filter = AddOptionsToFilter(filterDto, filter);
            var orderingQuery = filterDto.Orderings.ToOrderingExpression<CuttingMachine>(MachineFilterDto.GetOrderingMapping(), nameof(CuttingMachine.Id));

            var machines = await repository.GetPagedListAsync(filter, CuttingMachineListDto.Projection,  orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return machines.ToPagedList();
        }

        public async Task UpdateCuttingMachineAsync(int id, CuttingMachineUpdateDto dto)
        {
            var cuttingMachine = await repository.SingleAsync(ent => ent.Id == id);
            cuttingMachine = dto.UpdateModelObject(cuttingMachine);

            await unitOfWork.SaveChangesAsync();
        }

        private static Expression<Func<CuttingMachine, bool>> AddOptionsToFilter(CuttingMachineFilterDto filterDto, Expression<Func<CuttingMachine, bool>> filter)
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
