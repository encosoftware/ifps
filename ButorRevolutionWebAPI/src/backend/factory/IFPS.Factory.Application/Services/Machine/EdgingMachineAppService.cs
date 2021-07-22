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
    public class EdgingMachineAppService : ApplicationService, IEdgingMachineAppService
    {
        private readonly IEdgingMachineRepository repository;

        public EdgingMachineAppService(IApplicationServiceDependencyAggregate aggregate,
            IEdgingMachineRepository repository) : base(aggregate)
        {
            this.repository = repository;
        }

        public async Task<int> CreateEdgingMachineAsync(EdgingMachineCreateDto dto)
        {
            var edgingMachine = dto.CreateModelObject();
            await repository.InsertAsync(edgingMachine);
            await unitOfWork.SaveChangesAsync();
            return edgingMachine.Id;
        }

        public async Task DeleteEdgingMachineAsync(int id)
        {
            await repository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<EdgingMachineDetailsDto> GetEdgingMachineByIdAsync(int id)
        {
            var machine = await repository.SingleAsync(ent => ent.Id == id);
            return new EdgingMachineDetailsDto(machine);
        }

        public async Task<PagedListDto<EdgingMachineListDto>> GetEdgingMachinesAsync(EdgingMachineFilterDto filterDto)
        {
            Expression<Func<EdgingMachine, bool>> filter = ent => ent.Code != null;
            filter = AddOptionsToFilter(filterDto, filter);
            var orderingQuery = filterDto.Orderings.ToOrderingExpression<EdgingMachine>(MachineFilterDto.GetOrderingMapping(), nameof(EdgingMachine.Id));

            var machines = await repository.GetPagedListAsync(filter, EdgingMachineListDto.Projection, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return machines.ToPagedList();
        }

        public async Task UpdateEdgingMachineAsync(int id, EdgingMachineUpdateDto dto)
        {
            EdgingMachine edgingMachine = await repository.SingleAsync(ent => ent.Id == id);
            edgingMachine = dto.UpdateModelObject(edgingMachine);

            await unitOfWork.SaveChangesAsync();
        }

        private static Expression<Func<EdgingMachine, bool>> AddOptionsToFilter(EdgingMachineFilterDto filterDto, Expression<Func<EdgingMachine, bool>> filter)
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