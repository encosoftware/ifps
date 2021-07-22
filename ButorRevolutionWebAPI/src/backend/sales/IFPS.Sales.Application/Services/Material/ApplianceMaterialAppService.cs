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
    public class ApplianceMaterialAppService : ApplicationService, IApplianceMaterialAppService
    {
        private readonly IApplianceMaterialRepository applianceMaterialRepository;
        private readonly IFileHandlerService fileHandlerService;

        public ApplianceMaterialAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IApplianceMaterialRepository applianceMaterialRepository,
            IFileHandlerService fileHandlerService)
            : base(aggregate)
        {
            this.applianceMaterialRepository = applianceMaterialRepository;
            this.fileHandlerService = fileHandlerService;
        }

        public async Task<Guid> CreateApplianceMaterialAsync(ApplianceMaterialCreateDto applianceMaterialCreateDto)
        {
            var applianceMaterial = applianceMaterialCreateDto.CreateModelObject();
            applianceMaterial.AddPrice(new MaterialPrice() { Price = applianceMaterialCreateDto.PurchasingPrice.CreateModelObject() });
            applianceMaterial.ImageId = await fileHandlerService.InsertImage(applianceMaterialCreateDto.ImageCreateDto.ContainerName, applianceMaterialCreateDto.ImageCreateDto.FileName);

            await applianceMaterialRepository.InsertAsync(applianceMaterial);
            await unitOfWork.SaveChangesAsync();

            return applianceMaterial.Id;
        }

        public async Task DeleteMaterialAsync(Guid id)
        {
            await applianceMaterialRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<ApplianceMaterialDetailsDto> GetApplianceMaterialAsync(Guid id)
        {
            var applianceMaterial = await applianceMaterialRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentPrice, ent => ent.Image, ent => ent.Category);
            return new ApplianceMaterialDetailsDto(applianceMaterial);
        }

        public async Task<PagedListDto<ApplianceMaterialListDto>> GetApplianceMaterialsAsync(ApplianceMaterialFilterDto filterDto)
        {
            Expression<Func<ApplianceMaterial, bool>> filter = (ApplianceMaterial c) => c.Code != null;

            filter = AddOptionsToFilter(filterDto, filter);
            
            var orderingQuery = filterDto.Orderings.ToOrderingExpression<ApplianceMaterial>(ApplianceMaterialFilterDto.GetColumnMappings(), nameof(ApplianceMaterial.Id));

            var applianceMaterials = await applianceMaterialRepository.GetApplianceMaterialsAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return applianceMaterials.ToPagedList(ApplianceMaterialListDto.FromEntity);
        }

        public async Task UpdateApplianceMaterialAsync(Guid id, ApplianceMaterialUpdateDto applianceMaterialUpdateDto)
        {
            var applianceMaterial = await applianceMaterialRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentPrice, ent => ent.Image, ent => ent.Category);
            applianceMaterial = applianceMaterialUpdateDto.UpdateModelObject(applianceMaterial);
            applianceMaterial.Image = await fileHandlerService.UpdateImage(applianceMaterial.ImageId.Value, applianceMaterialUpdateDto.ImageUpdateDto.ContainerName, applianceMaterialUpdateDto.ImageUpdateDto.FileName);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ApplianceMaterialsListForDropdownDto>> GetApplianceMaterialsForDropdownAsync()
        {
            var appmats = await applianceMaterialRepository.GetAllListAsync();
            return appmats.Select(ent => new ApplianceMaterialsListForDropdownDto(ent)).ToList();
        }

        private static Expression<Func<ApplianceMaterial, bool>> AddOptionsToFilter(ApplianceMaterialFilterDto filterDto, Expression<Func<ApplianceMaterial, bool>> filter)
        {
            if (filterDto != null)
            {
                if (!string.IsNullOrWhiteSpace(filterDto.Brand))
                {
                    filter = filter.And(ent => ent.Brand.Name.ToLower().Contains(filterDto.Brand.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Description))
                {
                    filter = filter.And(ent => ent.Description.ToLower().Contains(filterDto.Description.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Code))
                {
                    filter = filter.And(ent => ent.Code.ToLower().Contains(filterDto.Code.ToLower()));
                }

                if (filterDto.CategoryId != 0)
                {
                    filter = filter.And(ent => ent.CategoryId.Equals(filterDto.CategoryId));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.HanaCode))
                {
                    filter = filter.And(ent => ent.HanaCode.ToLower().Contains(filterDto.HanaCode.ToLower()));
                }
            }

            return filter;
        }

    }
}
