using ENCO.DDD;
using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using LinqKit;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Dbos.Orders;
using IFPS.Factory.Application.Exceptions;
using CsvHelper;
using IFPS.Factory.Domain.FileHandling;
using IFPS.Factory.Domain.Services.Interfaces;
using Microsoft.Extensions.Options;
using ENCO.DDD.Extensions;

namespace IFPS.Factory.Application.Services
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUserRepository userRepository;
        private readonly IConcreteFurnitureUnitRepository concreteFurnitureUnitRepository;
        private readonly IConcreteFurnitureComponentRepository concreteFurnitureComponentRepository;
        private readonly IFurnitureUnitRepository furnitureUnitRepository;
        private readonly IApplianceMaterialRepository applianceMaterialRepository;
        private readonly IOrderStateRepository orderStateRepository;
        private readonly IStockedMaterialRepository stockedMaterialRepository;
        private readonly IDecorBoardMaterialRepository decorBoardMaterialRepository;
        private readonly IFileHandlerService fileHandlerService;
        private readonly ApplicationSettings appSettings;

        public OrderAppService(IApplicationServiceDependencyAggregate aggregate,
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IConcreteFurnitureUnitRepository concreteFurnitureUnitRepository,
            IConcreteFurnitureComponentRepository concreteFurnitureComponentRepository,
            IFurnitureUnitRepository furnitureUnitRepository,
            IApplianceMaterialRepository applianceMaterialRepository,
            IOrderStateRepository orderStateRepository,
            IStockedMaterialRepository stockedMaterialRepository,
            IDecorBoardMaterialRepository decorBoardMaterialRepository,
            IFileHandlerService fileHandlerService,
            IOptions<ApplicationSettings> options
            ) : base(aggregate)
        {
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
            this.concreteFurnitureComponentRepository = concreteFurnitureComponentRepository;
            this.concreteFurnitureUnitRepository = concreteFurnitureUnitRepository;
            this.furnitureUnitRepository = furnitureUnitRepository;
            this.applianceMaterialRepository = applianceMaterialRepository;
            this.orderStateRepository = orderStateRepository;
            this.stockedMaterialRepository = stockedMaterialRepository;
            this.decorBoardMaterialRepository = decorBoardMaterialRepository;
            this.fileHandlerService = fileHandlerService;
            this.appSettings = options.Value;
        }

        public async Task<Guid> CreateOrderAsync(OrderCreateDto dto)
        {
            var newOrder = dto.CreateModelObject();
            await orderRepository.InsertAsync(newOrder);

            var tempAddress = new TempAddressCreateDto();
            newOrder.ShippingAddress = tempAddress.CreateModelObject();

            var tempUserDto = new TempUserCreateDto();
            var tempUser = tempUserDto.CreateModelObject();
            await userRepository.InsertAsync(tempUser);

            foreach (var id in dto.FurnitureUnitIds)
            {
                var furnitureUnit = await furnitureUnitRepository.SingleIncludingAsync(ent => ent.Id == id, x => x.Components);

                var concreteFurnitureUnit = new ConcreteFurnitureUnit(newOrder.Id);
                concreteFurnitureUnit.FurnitureUnitId = furnitureUnit.Id;
                await concreteFurnitureUnitRepository.InsertAsync(concreteFurnitureUnit);

                foreach (var component in furnitureUnit.Components)
                {
                    var concreteFurnitureComponent = new ConcreteFurnitureComponent(concreteFurnitureUnit.Id, component.Id);
                    await concreteFurnitureComponentRepository.InsertAsync(concreteFurnitureComponent);
                }

            }

            foreach (var id in dto.ApplianceIds)
            {
                var appliance = await applianceMaterialRepository.SingleAsync(ent => ent.Id == id);

                var concreteApplianceMaterial = new ConcreteApplianceMaterial() { OrderId = newOrder.Id, ApplianceMaterialId = appliance.Id };
                newOrder.AddConcreteApplianceMaterial(concreteApplianceMaterial);
            }

            await unitOfWork.SaveChangesAsync();

            return newOrder.Id;
        }

        public async Task<List<OrderNameListDto>> GetOrderNamesAsync()
        {
            var orders = await orderRepository.GetAllListAsync();
            return orders.Select(ent => new OrderNameListDto(ent)).ToList();
        }

        public async Task AddOrderPaymentAsync(Guid orderId, OrderFinanceCreateDto orderFinanceCreateDto, int userId)
        {
            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId, ent => ent.FirstPayment, ent => ent.SecondPayment, ent => ent.CurrentTicket);
            var orderStates = await orderStateRepository.GetAllListAsync();

            switch (orderFinanceCreateDto.PaymentIndex)
            {
                case 1:
                    order.FirstPayment.SetPaymentDate(orderFinanceCreateDto.PaymentDate);
                    order.SetFirstPaymentConfirmed(userId);
                    break;
                case 2:
                    order.SecondPayment.SetPaymentDate(orderFinanceCreateDto.PaymentDate);
                    order.SetSecondPaymentConfirmed(userId);
                    break;
                default:
                    throw new IFPSValidationAppException(new Dictionary<string, List<string>>()
                    { { "PaymentIndex", new List<string>() { "PaymentIndex must be 1 or 2" } } });
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedListDto<OrderFinanceListDto>> GetOrdersByCompanyAsync(int companyId, OrderFinanceFilterDto filterDto)
        {
            Expression<Func<Order, bool>> filter = CreateFilter(filterDto);

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<Order>(OrderFinanceFilterDto.GetColumnMappings(), nameof(Order.Id));
            var orders = await orderRepository.GetOrdersByCompany(companyId, filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return orders.ToPagedList(OrderFinanceListDto.FromEntity);
        }

        public async Task<OrderDetailsDto> GetOrderDetailsAsync(Guid id)
        {
            var order = await orderRepository.GetOrderByIdAsync(id);
            return OrderDetailsDto.FromEntity(order);
        }

        public async Task ExportOrdersResultsAsync(OrderFinanceFilterDto filterDto)
        {
            Expression<Func<Order, bool>> filter = CreateFilter(filterDto);
            var orders = await orderRepository.GetAllListIncludingAsync(filter, ent => ent.CurrentTicket.OrderState.Translations, ent => ent.FirstPayment);
            await CreateCsvFileAsync(orders);
        }

        public async Task ReserveOrFreeUpMaterialsForOrderAsync(Guid orderId, bool isReserved, int userId)
        {
            var requiredMaterialDbos = await CalculateRequiredAmount(orderId);
            var order = await orderRepository.SingleAsync(ent => ent.Id == orderId);
            var stockedMaterials = stockedMaterialRepository.GetStockedMaterialsByIds(requiredMaterialDbos.Keys.ToList()).Result;

            foreach (var stockedMat in stockedMaterials)
            {
                var requiredDbo = requiredMaterialDbos[stockedMat.MaterialId];
                double reservableAmount = requiredDbo.RequiredAmount;

                if (isReserved)
                {
                    stockedMat.StockedAmount += reservableAmount;
                }
                else
                {
                    stockedMat.StockedAmount -= reservableAmount;
                }
            }

            if (isReserved)
            {
                order.SetUnderMaterialReservationState(userId);
            }
            else
            {
                order.SetWaitingForSchedulingState(userId);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<Dictionary<Guid, RequiredMaterialForOrderDbo>> CalculateRequiredAmount(Guid orderId)
        {
            //double AREA_MULTIPLIER = appSettings.AreaMultiplierForBoardNumEstimation; // configuration.GetValue<double>("ApplicationSettings:AreaMultiplierForBoardNumEstimation");

            // gather required amounts for each material
            var order = await orderRepository.GetOrderByIdForMaterialReservation(orderId);
            var requiredMaterialDbos = new Dictionary<Guid, RequiredMaterialForOrderDbo>();

            foreach (var cfu in order.ConcreteFurnitureUnits)
            {
                foreach (var cfc in cfu.ConcreteFurnitureComponents)
                {
                    var area = cfc.FurnitureComponent.Width * cfc.FurnitureComponent.Length;

                    if (!requiredMaterialDbos.ContainsKey(cfc.FurnitureComponent.BoardMaterial.Id))
                    {
                        var dbo = new RequiredMaterialForOrderDbo()
                        {
                            RequiredAmount = area,
                            MaterialType = "DecorBoard"
                        };
                        requiredMaterialDbos[cfc.FurnitureComponent.BoardMaterial.Id] = dbo;
                    }
                    else
                    {
                        requiredMaterialDbos[cfc.FurnitureComponent.BoardMaterial.Id].RequiredAmount += area;
                    }

                    if (cfc.FurnitureComponent.TopFoil != null)
                    {
                        if (!requiredMaterialDbos.ContainsKey(cfc.FurnitureComponent.TopFoil.Id))
                        {
                            var dbo = new RequiredMaterialForOrderDbo()
                            {
                                RequiredAmount = cfc.FurnitureComponent.Width,
                                MaterialType = "Foil"
                            };
                            requiredMaterialDbos[cfc.FurnitureComponent.TopFoil.Id] = dbo;
                        }
                        else
                        {
                            requiredMaterialDbos[cfc.FurnitureComponent.TopFoil.Id].RequiredAmount += cfc.FurnitureComponent.Width;
                        }
                    }

                    if (cfc.FurnitureComponent.BottomFoil != null)
                    {
                        if (!requiredMaterialDbos.ContainsKey(cfc.FurnitureComponent.BottomFoil.Id))
                        {
                            var dbo = new RequiredMaterialForOrderDbo()
                            {
                                RequiredAmount = cfc.FurnitureComponent.Width,
                                MaterialType = "Foil"
                            };
                            requiredMaterialDbos[cfc.FurnitureComponent.BottomFoil.Id] = dbo;
                        }
                        else
                        {
                            requiredMaterialDbos[cfc.FurnitureComponent.BottomFoil.Id].RequiredAmount += cfc.FurnitureComponent.Width;
                        }
                    }

                    if (cfc.FurnitureComponent.RightFoil != null)
                    {
                        if (!requiredMaterialDbos.ContainsKey(cfc.FurnitureComponent.RightFoil.Id))
                        {
                            var dbo = new RequiredMaterialForOrderDbo()
                            {
                                RequiredAmount = cfc.FurnitureComponent.Length,
                                MaterialType = "Foil"
                            };
                            requiredMaterialDbos[cfc.FurnitureComponent.RightFoil.Id] = dbo;
                        }
                        else
                        {
                            requiredMaterialDbos[cfc.FurnitureComponent.RightFoil.Id].RequiredAmount += cfc.FurnitureComponent.Length;
                        }
                    }

                    if (cfc.FurnitureComponent.LeftFoil != null)
                    {
                        if (!requiredMaterialDbos.ContainsKey(cfc.FurnitureComponent.LeftFoil.Id))
                        {
                            var dbo = new RequiredMaterialForOrderDbo()
                            {
                                RequiredAmount = cfc.FurnitureComponent.Length,
                                MaterialType = "Foil"
                            };
                            requiredMaterialDbos[cfc.FurnitureComponent.LeftFoil.Id] = dbo;
                        }
                        else
                        {
                            requiredMaterialDbos[cfc.FurnitureComponent.LeftFoil.Id].RequiredAmount += cfc.FurnitureComponent.Length;
                        }
                    }
                }

                foreach (var accessory in cfu.FurnitureUnit.Accessories)
                {
                    if (!requiredMaterialDbos.ContainsKey(accessory.Accessory.Id))
                    {
                        var dbo = new RequiredMaterialForOrderDbo()
                        {
                            RequiredAmount = accessory.AccessoryAmount,
                            MaterialType = "Accessory"
                        };
                        requiredMaterialDbos[accessory.Accessory.Id] = dbo;
                    }
                    else
                    {
                        requiredMaterialDbos[accessory.Accessory.Id].RequiredAmount += accessory.AccessoryAmount;
                    }
                }
            }

            foreach (var requiredMatKVP in requiredMaterialDbos)
            {
                if (requiredMatKVP.Value.MaterialType == "DecorBoard")
                {
                    /// convert flat mm2 to board number
                    var decorBoard = await decorBoardMaterialRepository.SingleAsync(ent => ent.Id == requiredMatKVP.Key);
                    double area = decorBoard.Dimension.Length * decorBoard.Dimension.Width;
                    int boardNum = (int)Math.Ceiling((requiredMatKVP.Value.RequiredAmount * appSettings.AreaMultiplierForBoardNumEstimation) / area);
                    requiredMaterialDbos[requiredMatKVP.Key].RequiredAmount = boardNum;
                }
                else if (requiredMatKVP.Value.MaterialType == "Foil")
                {
                    /// convert foil length to rounded meters
                    requiredMaterialDbos[requiredMatKVP.Key].RequiredAmount = Math.Ceiling(requiredMatKVP.Value.RequiredAmount / 1000.0);
                }
            }

            return requiredMaterialDbos;
        }

        private static Expression<Func<Order, bool>> CreateFilter(OrderFinanceFilterDto filterDto)
        {
            Ensure.NotNull(filterDto);
            Expression<Func<Order, bool>> filter = (Order o) => true;

            if (!string.IsNullOrWhiteSpace(filterDto.OrderName))
            {
                filter = filter.And(ent => ent.OrderName.ToLower().Contains(filterDto.OrderName.ToLower().Trim()));
            }
            if (!string.IsNullOrWhiteSpace(filterDto.WorkingNumber))
            {
                filter = filter.And(ent => ent.WorkingNumber.ToLower().Contains(filterDto.WorkingNumber.ToLower()));
            }
            if (filterDto.CurrentStatusId != null)
            {
                filter = filter.And(ent => ent.CurrentTicket.OrderStateId == filterDto.CurrentStatusId);
            }
            if (filterDto.StatusDeadlineFrom != null)
            {
                filter = filter.And(ent => ent.Deadline >= filterDto.StatusDeadlineFrom);
            }
            if (filterDto.StatusDeadlineTo != null)
            {
                filter = filter.And(ent => ent.Deadline >= filterDto.StatusDeadlineTo);
            }
            if (filterDto.DeadlineFrom != null)
            {
                filter = filter.And(ent => ent.Deadline >= filterDto.DeadlineFrom);
            }
            if (filterDto.DeadlineTo != null)
            {
                filter = filter.And(ent => ent.Deadline >= filterDto.DeadlineTo);
            }

            return filter;
        }

        private async Task CreateCsvFileAsync(List<Order> orders)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                using (var writer = new System.IO.StreamWriter(stream, System.Text.Encoding.UTF8))
                {
                    using (var csvWriter = new CsvWriter(writer))
                    {
                        csvWriter.Configuration.Delimiter = ";";

                        csvWriter.WriteField("Order Name");
                        csvWriter.WriteField("Working number");
                        csvWriter.WriteField("Current status");
                        csvWriter.WriteField("Deadline");
                        csvWriter.WriteField(nameof(OrderFinanceFilterDto.Price));

                        await csvWriter.NextRecordAsync();

                        foreach (var order in orders)
                        {
                            csvWriter.WriteField(order.OrderName);
                            csvWriter.WriteField(order.WorkingNumber);
                            csvWriter.WriteField(order.CurrentTicket.OrderState.Translations.GetCurrentTranslation().Name);
                            csvWriter.WriteField(order.Deadline.ToString("YYYY.MM.DD"));
                            csvWriter.WriteField(order.FirstPayment.Price.Value);

                            await csvWriter.NextRecordAsync();
                        }

                        await writer.FlushAsync();
                        stream.Position = 0;

                        await fileHandlerService.UploadFromStreamAsync(stream, FileContainerProvider.Containers.Temp, ".csv");
                    }
                }
            }
        }
    }
}