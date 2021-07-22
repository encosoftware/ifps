using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using LinqKit;

namespace IFPS.Sales.Application.Services
{
    public class AccessoryMaterialAppService : ApplicationService, IAccessoryMaterialAppService
    {
        private readonly IAccessoryMaterialRepository accessoryMaterialRepository;
        private readonly IFileHandlerService fileHandlerService;

        public AccessoryMaterialAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IAccessoryMaterialRepository accessoryMaterialRepository,
            IFileHandlerService fileHandlerService)
            : base(aggregate)
        {
            this.accessoryMaterialRepository = accessoryMaterialRepository;
            this.fileHandlerService = fileHandlerService;
        }
        
        public async Task<Guid> CreateAccessoryMaterialAsync(AccessoryMaterialCreateDto accessoryMaterialCreateDto)
        {
            var accessoryMaterial = accessoryMaterialCreateDto.CreateModelObject();
            accessoryMaterial.AddPrice(new MaterialPrice() { Price = accessoryMaterialCreateDto.Price.CreateModelObject() });
            accessoryMaterial.ImageId = await fileHandlerService.InsertImage(accessoryMaterialCreateDto.ImageCreateDto.ContainerName, accessoryMaterialCreateDto.ImageCreateDto.FileName);

            await accessoryMaterialRepository.InsertAsync(accessoryMaterial);
            await unitOfWork.SaveChangesAsync();

            return accessoryMaterial.Id;
        }
        public async Task DeleteMaterialAsync(Guid id)
        {
            await accessoryMaterialRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<AccessoryMaterialDetailsDto> GetAccessoryMaterialAsync(Guid id)
        {
            var accessoryMaterial = await accessoryMaterialRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentPrice, ent => ent.Image, ent => ent.Category);
            return new AccessoryMaterialDetailsDto(accessoryMaterial);
        }

        public async Task<List<AccessoryMaterialCodesDto>> GetAccessoryMaterialCodesAsync()
        {
            return await accessoryMaterialRepository.GetAllListAsync(ent => ent.Code != null, AccessoryMaterialCodesDto.Projection);
        }

        public async Task<PagedListDto<AccessoryMaterialListDto>> GetAccessoryMaterialsAsync(AccessoryMaterialFilterDto filterDto)
        {
            Expression<Func<AccessoryMaterial, bool>> filter = (AccessoryMaterial c) => c.Code != null;

            filter = AddOptionsToFilter(filterDto, filter);
           
            var orderingQuery = filterDto.Orderings.ToOrderingExpression<AccessoryMaterial>(AccessoryMaterialFilterDto.GetOrderingMapping(), nameof(AccessoryMaterial.Id));

            var accessoryMaterials = await accessoryMaterialRepository.GetAccessoryMaterialsAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return accessoryMaterials.ToPagedList(AccessoryMaterialListDto.FromEntity);
        }       

        public async Task UpdateAccessoryMaterialAsync(Guid id, AccessoryMaterialUpdateDto accessoryMaterialUpdateDto)
        {
            var accessoryMaterial = await accessoryMaterialRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentPrice, ent => ent.Image, ent => ent.Category);
            accessoryMaterial = accessoryMaterialUpdateDto.UpdateModelObject(accessoryMaterial);
            accessoryMaterial.Image = await fileHandlerService.UpdateImage(accessoryMaterial.ImageId.Value, accessoryMaterialUpdateDto.ImageUpdateDto.ContainerName, accessoryMaterialUpdateDto.ImageUpdateDto.FileName);

            await unitOfWork.SaveChangesAsync();
        }

        private static Expression<Func<AccessoryMaterial, bool>> AddOptionsToFilter(AccessoryMaterialFilterDto filterDto, Expression<Func<AccessoryMaterial, bool>> filter)
        {
            if (filterDto != null)
            {
                if (!string.IsNullOrWhiteSpace(filterDto.Code))
                {
                    filter = filter.And(ent => ent.Code.ToLower().Contains(filterDto.Code.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Description))
                {
                    filter = filter.And(ent => ent.Description.ToLower().Contains(filterDto.Description.ToLower()));
                }

                if (filterDto.CategoryId.HasValue)
                {
                    filter = filter.And(ent => ent.CategoryId.Equals(filterDto.CategoryId.Value));
                }

                if (filterDto.IsOptional.HasValue)
                {
                    filter = filter.And(ent => ent.IsOptional.Equals(filterDto.IsOptional.Value));
                }

                if (filterDto.IsRequiredForAssembly.HasValue)
                {
                    filter = filter.And(ent => ent.IsRequiredForAssembly.Equals(filterDto.IsRequiredForAssembly.Value));
                }
            }

            return filter;
        }
    }
}
