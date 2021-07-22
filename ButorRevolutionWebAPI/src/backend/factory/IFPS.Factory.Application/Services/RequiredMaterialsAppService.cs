using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using ENCO.DDD.Repositories;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.IO;
using IFPS.Factory.Domain.Dbos;
using IFPS.Factory.Domain.Services.Interfaces;

namespace IFPS.Factory.Application.Services
{
    public class RequiredMaterialsAppService : ApplicationService, IRequiredMaterialsAppService
    {
        private readonly IRequiredMaterialsRepository requiredMaterialsRepository;
        private readonly IStockedMaterialRepository stockedMaterialRepository;
        private readonly IUserRepository userRepository;
        private readonly IOrderRepository orderRepository;
        private readonly ApplicationSettings applicationSettings;
        private readonly IOrderAppService orderAppService;
        private readonly IFileHandlerService fileHandlerService;

        public RequiredMaterialsAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IRequiredMaterialsRepository requiredMaterialsRepository,
            IStockedMaterialRepository stockedMaterialRepository,
            IUserRepository userRepository,
            IOrderAppService orderAppService,
            IOrderRepository orderRepository,
            IFileHandlerService fileHandlerService,
            IOptions<ApplicationSettings> options
            ) : base(aggregate)
        {
            this.requiredMaterialsRepository = requiredMaterialsRepository;
            this.stockedMaterialRepository = stockedMaterialRepository;
            this.userRepository = userRepository;
            this.orderRepository = orderRepository;
            this.applicationSettings = options.Value;
            this.orderAppService = orderAppService;
            this.fileHandlerService = fileHandlerService;
        }

        public async Task<PagedListDto<RequiredMaterialsListDto>> GetRequiredMaterialsListAsync(RequiredMaterialsFilterDto filterDto)
        {
            Expression<Func<RequiredMaterial, bool>> filter = CreateFilter(filterDto);

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<RequiredMaterial>(
                RequiredMaterialsFilterDto.GetOrderingMapping(), nameof(RequiredMaterial.Id));

            var allRequiredMaterials = await requiredMaterialsRepository.GetPagedRequiredMaterialAsync(
                filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);

            return allRequiredMaterials.ToPagedList(RequiredMaterialsListDto.FromEntity);
        }

        public async Task<TempCargoDetailsForRequiredMaterialsDto> CreateSelectedRequiredMaterials(SelectedRequiredMaterialsDto dto)
        {
            var requiredMaterials = await requiredMaterialsRepository.GetRequiredMaterialsWithIncludeAsync(dto.RequiredMaterialsIds);
            if (requiredMaterials.Count == 0)
            {
                throw new EntityNotFoundException("There should be at least one chosen required material");
            }

            var tempCargo = new TempCargoDetailsForRequiredMaterialsDto(double.Parse(applicationSettings.VAT, CultureInfo.InvariantCulture));

            var requiredMaterialsByCode = requiredMaterials
                .GroupBy(entity => entity.Material.Code);

            var requiredMaterialListDtos = new List<MaterialsListDto>();
            foreach (var requiredMatByCode in requiredMaterialsByCode)
            {
                var stockedMat = await stockedMaterialRepository.SingleAsync(ent => ent.Material.Code == requiredMatByCode.Key);
                var requiredMaterialsListDto = new MaterialsListDto(requiredMatByCode, stockedMat, dto.SupplierId);
                requiredMaterialListDtos.Add(requiredMaterialsListDto);
            }

            tempCargo.Materials = requiredMaterialListDtos;

            var additionals = new List<AdditionalsListDto>();

            var stockedMaterialsUnderRequiredAmount = await stockedMaterialRepository.GetAllListIncludingAsync(ent => ent.OrderedAmount + ent.StockedAmount < ent.RequiredAmount, ent => ent.Material.Packages);
            foreach (var stockedMaterial in stockedMaterialsUnderRequiredAmount)
            {
                var additionalListDto = new AdditionalsListDto(stockedMaterial, dto.SupplierId);
                additionals.Add(additionalListDto);
            }

            tempCargo.Additionals = additionals;

            var bookedByUser = await userRepository.SingleIncludingAsync(ent => ent.Id == dto.BookedById, ent => ent.CurrentVersion);
            var firstPackage = requiredMaterials.First().Material.Packages.First();
            tempCargo.CargoDetailsBeforeSaveCargo.CreateCargoDetails(firstPackage, bookedByUser);

            return tempCargo;
        }

        public async Task<List<MaterialCodeListDto>> GetMaterialCodesAsync()
        {
            var materials = await requiredMaterialsRepository.GetMaterialsForCodeListAsync();

            var codeList = new List<MaterialCodeListDto>();

            foreach(var material in materials)
            {
                var code = new MaterialCodeListDto(material.Material.Id, material.Material.Code);
                codeList.Add(code);
            }

            return codeList;
        }

        public async Task<int> CreateRequiredMaterialByOrderIdAsync(Guid orderId, int userId)
        {
            var requiredMaterialDbos = await orderAppService.CalculateRequiredAmount(orderId);
            var stockedMaterials = stockedMaterialRepository.GetStockedMaterialsByIds(requiredMaterialDbos.Keys.ToList()).Result;
            var order = await orderRepository.SingleAsync(ent => ent.Id == orderId);

            int newRequiredMaterialId = 0;

            foreach (var stockedMat in stockedMaterials)
            {
                var requiredDbo = requiredMaterialDbos[stockedMat.MaterialId];
                double reservableAmount = requiredDbo.RequiredAmount;

                if (stockedMat.StockedAmount < requiredDbo.RequiredAmount)
                {
                    double shortage = Math.Ceiling(requiredDbo.RequiredAmount - stockedMat.StockedAmount);
                    var requiredMaterial = new RequiredMaterial(orderId, stockedMat.MaterialId, (int)shortage);
                    await requiredMaterialsRepository.InsertAsync(requiredMaterial);
                    newRequiredMaterialId = requiredMaterial.Id;
                    reservableAmount -= shortage;
                }
                //order.SetUnderMaterialReservationState(userId);
                //stockedMat.StockedAmount -= reservableAmount;
            }

            await unitOfWork.SaveChangesAsync();
            return newRequiredMaterialId;
        }

        public async Task<string> ExportCsvAsync(Stream stream, RequiredMaterialsFilterDto filterDto)
        {
            Expression<Func<RequiredMaterial, bool>> filter = CreateFilter(filterDto);
            var requiredMaterials = await requiredMaterialsRepository.GetAllListIncludingAsync(filter, ent => ent.Order, ent => ent.Material);
            var requiredMaterialDatas = requiredMaterials.Select(ent => new RequiredMaterialCsvDataDbo(ent)).ToList();
            return await fileHandlerService.ExportCsv(requiredMaterialDatas, "requiredMaterials", stream);
        }

        private static Expression<Func<RequiredMaterial, bool>> CreateFilter(RequiredMaterialsFilterDto filterDto)
        {
            Expression<Func<RequiredMaterial, bool>> filter = (RequiredMaterial entity) => entity.Material.SiUnit != null;

            if (!string.IsNullOrEmpty(filterDto.OrderName))
            {
                filter = filter.And(ent => ent.Order.OrderName.ToLower().Contains(filterDto.OrderName.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(filterDto.WorkingNumber))
            {
                filter = filter.And(ent => ent.Order.WorkingNumber.ToLower().Contains(filterDto.WorkingNumber.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(filterDto.MaterialCode))
            {
                filter = filter.And(ent => ent.Material.Code.Equals(filterDto.MaterialCode));
            }
            if (!string.IsNullOrEmpty(filterDto.Name))
            {
                filter = filter.And(ent => ent.Material.Description.ToLower().Contains(filterDto.Name.ToLower().Trim()));
            }
            if (filterDto.From.HasValue)
            {
                filter = filter.And(ent => ent.Order.Deadline >= filterDto.From);
            }
            if (filterDto.To.HasValue)
            {
                filter = filter.And(ent => ent.Order.Deadline <= filterDto.To);
            }

            return filter;
        }
    }
}
