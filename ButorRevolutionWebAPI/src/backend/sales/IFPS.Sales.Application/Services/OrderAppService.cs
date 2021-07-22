using ENCO.DDD;
using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Repositories;
using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Dto.Users;
using IFPS.Sales.Application.Exceptions;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Helpers;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using LinqKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private const double DIVIDE_BY_TWO = 0.5;

        private readonly IOrderRepository orderRepository;
        private readonly IUserRepository userRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly ISalesPersonRepository salesPersonRepository;
        private readonly ICabinetMaterialRepository cabinetMaterialRepository;
        private readonly IFurnitureUnitRepository furnitureUnitRepository;
        private readonly IFurnitureComponentRepository furnitureComponentRepository;
        private readonly IApplianceMaterialRepository applianceMaterialRepository;
        private readonly IServiceRepository serviceRepository;
        private readonly IDocumentFolderRepository documentFolderRepository;
        private readonly IDocumentStateRepository documentStateRepository;
        private readonly IOrderStateRepository orderStateRepository;
        private readonly IDocumentTypeRepository documentTypeRepository;
        private readonly IDocumentService documentService;
        private readonly IDocumentRepository documentRepository;
        private readonly IAppointmentService appointmentService;
        private readonly IDocumentAppService documentAppService;
        private readonly ApplicationSettings applicationSettings;
        private readonly OrderStateDeadlineConfiguration orderStateConfiguration;
        private readonly string contentRootPath = "";

        public OrderAppService(IApplicationServiceDependencyAggregate aggregate,
            IDocumentFolderRepository documentFolderRepository,
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            ICabinetMaterialRepository cabinetMaterialRepository,
            IFurnitureUnitRepository furnitureUnitRepository,
            IFurnitureComponentRepository furnitureComponentRepository,
            IApplianceMaterialRepository applianceMaterialRepository,
            IServiceRepository serviceRepository,
            ISalesPersonRepository salesPersonRepository,
            IOrderStateRepository orderStateRepository,
            IUserRepository userRepository,
            IDocumentStateRepository documentStateRepository,
            IDocumentTypeRepository documentTypeRepository,
            IAppointmentService appointmentService,
            IDocumentAppService documentAppService,
            IDocumentService documentService,
            IDocumentRepository documentRepository,
            IOptions<ApplicationSettings> options,
            IOptions<OrderStateDeadlineConfiguration> orderStateConfiguration,
            IHostingEnvironment hostingEnvironment) : base(aggregate)

        {
            this.orderRepository = orderRepository;
            this.salesPersonRepository = salesPersonRepository;
            this.customerRepository = customerRepository;
            this.cabinetMaterialRepository = cabinetMaterialRepository;
            this.furnitureUnitRepository = furnitureUnitRepository;
            this.applianceMaterialRepository = applianceMaterialRepository;
            this.furnitureComponentRepository = furnitureComponentRepository;
            this.serviceRepository = serviceRepository;
            this.documentFolderRepository = documentFolderRepository;
            this.orderStateRepository = orderStateRepository;
            this.userRepository = userRepository;
            this.documentStateRepository = documentStateRepository;
            this.documentTypeRepository = documentTypeRepository;
            this.documentService = documentService;
            this.documentRepository = documentRepository;
            this.appointmentService = appointmentService;
            this.documentAppService = documentAppService;
            this.applicationSettings = options.Value;
            this.orderStateConfiguration = orderStateConfiguration.Value;
            this.contentRootPath = System.IO.Path.Combine(hostingEnvironment.ContentRootPath, "AppData");
        }

        public async Task<Guid> CreateOrderAsync(OrderCreateDto orderDto)
        {
            var customer = await customerRepository.GetByUserIdAsync(orderDto.CustomerUserId);
            var salesPerson = await salesPersonRepository.GetByUserIdAsync(orderDto.SalesPersonUserId);
            var user = await userRepository.GetUserByCreateOrderByUserIdAsync(orderDto.CustomerUserId);

            var address = orderDto.ShippingAddress.CreateModelObject();
            if (!user.CurrentVersion.ContactAddress.Equals(address))
            {
                user.CurrentVersion.ContactAddress = address;
            }

            var order = new Order(orderDto.OrderName, customer.Id, salesPerson.Id, orderDto.Deadline, Price.GetDefaultPrice(), address);
            if (user.CompanyId == null)
            {
                order.IsPrivatePerson = true;
            }

            order.OfferInformation = new OfferInformation(Price.GetDefaultPrice(), Price.GetDefaultPrice());

            await orderRepository.InsertAsync(order);
            order.SetWaitingForOfferState();
            await InitDocumentGroupsForNewOrderAsync(order);
            await unitOfWork.SaveChangesAsync();

            return order.Id;
        }

        private async Task InitDocumentGroupsForNewOrderAsync(Order order)
        {
            var folders = await documentFolderRepository.GetAllListAsync();
            var state = await documentStateRepository.SingleOrDefaultAsync(ent => ent.State == DocumentStateEnum.Empty)
                ?? throw new EntityNotFoundException(typeof(DocumentState), "ent => ent.State == DocumentStateEnum.Absent");
            var groups = new List<DocumentGroup>();

            foreach (var folder in folders)
            {
                var group = new DocumentGroup(order, folder);
                if (!folder.IsHistorized) //we should add a default version to those folders, which will have only one version
                {
                    group.AddNewVersion(new DocumentGroupVersion(group, state));
                }

                groups.Add(group);
            }

            order.AddDocumentGroups(groups);
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            await orderRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<OrderSalesHeaderDto> GetOrderDetailsAsync(Guid id, int userId)
        {
            var user = await userRepository.GetUserDivisionAsync(userId);
            var divisionTypes = user.Roles.Select(ent => ent.Role.Division.DivisionType).ToList();
            var order = await orderRepository.GetOrderByIdAsync(id);
            return OrderSalesHeaderDto.FromModel(order, divisionTypes);
        }

        public async Task<PagedListDto<OrderListDto>> GetOrdersAsync(OrderFilterDto filterDto, int? userId, DivisionTypeEnum? division)
        {
            CreateFilter(out Expression<Func<Order, bool>> filter, filterDto);
            var orderingQuery = filterDto.Orderings.ToOrderingExpression<Order>(OrderFilterDto.GetColumnMappings(), nameof(Order.Id));

            switch (division)
            {
                case DivisionTypeEnum.Sales:
                    filter = filter.And(ent => ent.SalesPerson.UserId == userId);
                    break;
                case DivisionTypeEnum.Partner:
                    List<Guid> orderIds = await appointmentService.GetOrderIdsByPartnerIdAsync();
                    filter = filter.And(ent => orderIds.Contains(ent.Id));
                    break;
                case DivisionTypeEnum.Customer:
                    filter = filter.And(ent => ent.Customer.UserId == userId);
                    break;
                case DivisionTypeEnum.Admin:
                    break;
                default:
                    throw new IFPSValidationAppException(new Dictionary<string, List<string>>() { { "Division", new List<string>() { "Division should be Sales, Partner or Customer" } } });
            }

            var orders = await orderRepository.GetOrders(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return orders.ToPagedList(OrderListDto.FromEntity);
        }


        public async Task UpdateOrderState(Guid id, OrderEditDto updateDto)
        {
            var customer = await customerRepository.GetByUserIdAsync(updateDto.CustomerUserId);
            var salesPerson = await salesPersonRepository.GetByUserIdAsync(updateDto.SalesPersonUserId);
            var state = await orderStateRepository.GetAsync(updateDto.CurrentStatusId);
            var user = updateDto.AssignedToUserId.HasValue ? await userRepository.GetAsync(updateDto.AssignedToUserId.Value) : null;

            var order = await orderRepository.GetOrderByIdAsync(id);
            var deadlineOffset = orderStateConfiguration.GetType().GetProperty(state.State.ToString()).GetValue(orderStateConfiguration);
            updateDto.UpdateModelObject(order, customer.Id, salesPerson.Id, state.Id, user.Id, (int)deadlineOffset);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UploadDocuments(Guid orderId, DocumentUploadDto dto)
        {
            var group = await orderRepository.GetDocumentGroupWithFolderAsync(orderId, dto.DocumentGroupId);
            if (!group.IsHistorized && dto.DocumentGroupVersionId.HasValue)
            {
                throw new IFPSAppException("Can not add version to a not historized document group!");
            }

            var uploader = await userRepository.GetAsync(dto.UploaderUserId);
            var type = await documentTypeRepository.GetAsync(dto.DocumentTypeId);

            var documents = (await Task.WhenAll(dto.Documents.Select(x => documentService.CreateDocumentAsync(x.ContainerName, x.FileName, type, uploader)))).ToList();

            if (dto.DocumentGroupVersionId.HasValue)
            {
                await documentService.AddDocumentsToExistingVersionAsync(group, dto.DocumentGroupVersionId.Value, documents, type);
            }
            else if (group.IsHistorized)
            {
                await documentService.AddDocumentsToNewVersionAsync(group, documents, type);
            }
            else
            {
                await documentService.AddDocumentsToExistingVersionAsync(group, group.Versions.First().Id, documents, type);
            }

            var documentType = await documentTypeRepository.SingleOrDefaultAsync(ent => ent.Id == dto.DocumentTypeId);
            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId, ent => ent.CurrentTicket, ent => ent.SalesPerson, ent => ent.Customer.User.CurrentVersion);

            if (documentType.Type == DocumentTypeEnum.Offer)
            {
                order.SetWaitingForOfferFeedbackState();
            }

            if (documentType.Type == DocumentTypeEnum.Contract)
            {
                order.SetWaitingForContractFeedbackState();
            }

            await unitOfWork.SaveChangesAsync();
        }


        public async Task<List<DocumentGroupDto>> GetDocumentsOfOrder(Guid orderId)
        {
            return await orderRepository.GetDocumentGroupsAsync(orderId, DocumentGroupDto.Projection);
        }

        public async Task CreateOfferAsync(OfferCreateDto dto, Guid orderId)
        {
            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId);
            order.OfferInformation = new OfferInformation(Price.GetDefaultPrice(), Price.GetDefaultPrice());
            dto.Requires.UpdateModelObject(order);

            var topCabinet = dto.TopCabinet.CreateModelObject(order.Id);
            topCabinet.CabinetType = CabinetTypeEnum.TopCabinet;
            await cabinetMaterialRepository.InsertAsync(topCabinet);
            order.TopCabinetId = topCabinet.Id;

            var baseCabinet = dto.BaseCabinet.CreateModelObject(order.Id);
            baseCabinet.CabinetType = CabinetTypeEnum.BaseCabinet;
            await cabinetMaterialRepository.InsertAsync(baseCabinet);
            order.BaseCabinetId = baseCabinet.Id;

            var tallCabinet = dto.TallCabinet.CreateModelObject(order.Id);
            tallCabinet.CabinetType = CabinetTypeEnum.TallCabinet;
            await cabinetMaterialRepository.InsertAsync(tallCabinet);
            order.TallCabinetid = tallCabinet.Id;

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<FurnitureUnitDetailsByOfferDto> GetOrderedFurnitureUnitAsync(Guid orderId, int orderedFurnitureUnitId)
        {
            var order = await orderRepository.GetOrderWithIncludesForOrderedFurnitureUnitDetailsById(orderId);
            var orderedFurnitureUnit = order.OrderedFurnitureUnits.Single(ent => ent.Id == orderedFurnitureUnitId);
            return new FurnitureUnitDetailsByOfferDto(orderedFurnitureUnit);
        }

        public async Task UpdateOrderedFurnitureUnitQuantityAsync(Guid orderId, int orderedFurnitureId, UpdateOrderedFurnitureUnitQuantityByOfferDto dto)
        {
            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId, x => x.OrderedFurnitureUnits);
            var orderedFurnitureUnit = order.OrderedFurnitureUnits.Single(ent => ent.Id == orderedFurnitureId);
            dto.UpdateModelObject(orderedFurnitureUnit);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<Guid> CreateFurnitureUnitByOfferAsync(Guid orderId, Guid baseFurnitureUnitId, FurnitureUnitCreateWithQuantityByOfferDto dto)
        {
            var order = await orderRepository.GetOrderForCreateFurnitureUnitById(orderId);
            var ofu = order.OrderedFurnitureUnits.Single(ent => ent.FurnitureUnitId == baseFurnitureUnitId);
            var furnitureUnit = dto.CreateModelObject(baseFurnitureUnitId, ofu.FurnitureUnit.ImageId, ofu.FurnitureUnit.CurrentPriceId, ofu.FurnitureUnit.FurnitureUnitTypeId);
            await furnitureUnitRepository.InsertAsync(furnitureUnit);

            foreach (var frontComponent in dto.Fronts)
            {
                var parentFrontComponent = ofu.FurnitureUnit.Components.Single(ent => ent.Id == frontComponent.Id);

                var front = CompareToTheComponents(parentFrontComponent, frontComponent);
                var newComponentFromDto = new FurnitureComponentsCreateWithoutIdByOfferDto(front);
                var newComponent = newComponentFromDto.CreateModelObject(front.FurnitureUnitId, front.ImageId);
                await furnitureComponentRepository.InsertAsync(newComponent);

                furnitureUnit.AddFurnitureComponent(newComponent);
            }

            foreach (var corpusComponent in dto.Corpuses)
            {
                var parentCorpusComponent = ofu.FurnitureUnit.Components.Single(ent => ent.Id == corpusComponent.Id);

                var corpus = CompareToTheComponents(parentCorpusComponent, corpusComponent);
                var newComponentFromDto = new FurnitureComponentsCreateWithoutIdByOfferDto(corpus);
                var newComponent = newComponentFromDto.CreateModelObject(corpus.FurnitureUnitId, corpus.ImageId);
                await furnitureComponentRepository.InsertAsync(newComponent);

                furnitureUnit.AddFurnitureComponent(newComponent);
            }

            order.AddOrderedFurnitureUnit(new OrderedFurnitureUnit(order.Id, furnitureUnit.Id, dto.Quantity));
            order.RemoveOrderedFurnitureUnit(ofu);

            await unitOfWork.SaveChangesAsync();
            return furnitureUnit.Id;
        }


        public async Task AddOrderedFurnitureUnitAsync(Guid orderId, FurnitureUnitCreateByOfferDto dto)
        {
            var order = await orderRepository.GetOrderByIdByAddService(orderId);

            var furnitureUnit = await furnitureUnitRepository.GetFurnitureUnitByIdByOfferAsync(dto.FurnitureUnitId);
            var orderedFurnitureUnit = dto.CreateModelObject(order.Id);
            orderedFurnitureUnit.UnitPrice = furnitureUnit.CurrentPrice.Price;

            order.AddOrderedFurnitureUnit(orderedFurnitureUnit);
            order.OfferInformation.ProductsPrice.Add(orderedFurnitureUnit.UnitPrice.Value * orderedFurnitureUnit.Quantity);

            foreach (var accessoryMaterialFurnitureUnit in furnitureUnit.Accessories)
            {
                var accessoryAmount = orderedFurnitureUnit.Quantity > 1 ? orderedFurnitureUnit.Quantity *
                    accessoryMaterialFurnitureUnit.AccessoryAmount : accessoryMaterialFurnitureUnit.AccessoryAmount;

                order.OfferInformation.ProductsPrice.Add(accessoryMaterialFurnitureUnit.Accessory.CurrentPrice.Price.Value * accessoryAmount);
            }

            foreach (var service in order.Services)
            {
                AddExistingServicePricesToOrder(order, service, dto.Quantity);
            }

            await unitOfWork.SaveChangesAsync();
        }

        private void AddExistingServicePricesToOrder(Order order, OrderedService service, int sumQuantity)
        {
            double priceToAdd = 0;
            if (service.Service.ServiceType.Type == ServiceTypeEnum.Assembly)
            {
                priceToAdd += sumQuantity * double.Parse(applicationSettings.AssemblyPrice);
            }
            else if (service.Service.ServiceType.Type == ServiceTypeEnum.Installation)
            {
                priceToAdd += sumQuantity * double.Parse(applicationSettings.InstallationFurnitureUnit);
            }
            order.OfferInformation.ServicesPrice.Add(priceToAdd);
        }

        public async Task DeleteOrderedFurnitureUnitAsync(Guid orderId, Guid furnitureUnitId)
        {
            var order = await orderRepository.GetOrderDeleteByIdAsync(orderId);
            var furnitureUnit = await furnitureUnitRepository.GetFurnitureUnitByIdByOfferAsync(furnitureUnitId);

            var orderedFurnitureUnit = order.OrderedFurnitureUnits.Single(ent => ent.FurnitureUnitId == furnitureUnitId);
            order.RemoveOrderedFurnitureUnit(orderedFurnitureUnit);

            order.OfferInformation.ProductsPrice.Div(orderedFurnitureUnit.UnitPrice.Value * orderedFurnitureUnit.Quantity);
            SetServicePriceAfterOrderedFurnitureUnitDeleted(order, orderedFurnitureUnit);

            foreach (var accessoryMaterialFurnitureUnit in furnitureUnit.Accessories)
            {
                var accessoryAmount = orderedFurnitureUnit.Quantity > 1 ? orderedFurnitureUnit.Quantity *
                    accessoryMaterialFurnitureUnit.AccessoryAmount : accessoryMaterialFurnitureUnit.AccessoryAmount;

                order.OfferInformation.ProductsPrice.Div(accessoryMaterialFurnitureUnit.Accessory.CurrentPrice.Price.Value * accessoryAmount);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<OfferDetailsDto> GetFurnitureUnitsListByOfferAsync(Guid orderId)
        {
            var order = await orderRepository.GetOrderWithIncludesForListById(orderId);

            var offerDetails = new OfferDetailsDto(order, double.Parse(applicationSettings.ShippingBasicFee, CultureInfo.InvariantCulture), double.Parse(applicationSettings.InstallationBasicFee, CultureInfo.InvariantCulture));
            var accessories = new List<AccessoryMaterialFurnitureUnit>();
            var currency = order.Budget.Currency;

            foreach (var unit in order.OrderedFurnitureUnits)
            {
                var oneUnit = new FurnitureUnitListByOfferDto(unit);

                if (unit.FurnitureUnit.FurnitureUnitType.Type == FurnitureUnitTypeEnum.Top)
                {
                    offerDetails.Products.TopCabinets.Add(oneUnit);
                }

                if (unit.FurnitureUnit.FurnitureUnitType.Type == FurnitureUnitTypeEnum.Base)
                {
                    offerDetails.Products.BaseCabinets.Add(oneUnit);
                }

                if (unit.FurnitureUnit.FurnitureUnitType.Type == FurnitureUnitTypeEnum.Tall)
                {
                    offerDetails.Products.TallCabinets.Add(oneUnit);
                }

                //var accessoryAmount = unit.Quantity > 1 ? unit.Quantity : 0;

                unit.FurnitureUnit.Accessories.ToList().ForEach(ent =>
                {
                    accessories.Add(ent);
                });
            }

            foreach (var appliance in order.OrderedApplianceMaterials)
            {
                var oneAppliance = new ApplianceListByOfferDto(appliance);
                offerDetails.Products.Appliances.Add(oneAppliance);
            }

            accessories.ToList().ForEach(ent =>
            {
                var ofu = order.OrderedFurnitureUnits.Single(entity => entity.FurnitureUnitId == ent.FurnitureUnitId);
                var accessoryAmount = ofu.Quantity > 1 ? ofu.Quantity : 0;

                offerDetails.Products.Accessories.Add(new AccessoryMaterialsListByOfferDto(ent, accessoryAmount));

            });

            if (order.OfferInformation != null)
            {
                offerDetails.Products.Prices.SetPricesValue(order,
                    order.OfferInformation.ProductsPrice,
                    order.OfferInformation.ServicesPrice,
                    double.Parse(applicationSettings.VAT, CultureInfo.InvariantCulture),
                    double.Parse(applicationSettings.AssemblyPrice, CultureInfo.InvariantCulture),
                    double.Parse(applicationSettings.InstallationFurnitureUnit, CultureInfo.InvariantCulture),
                    double.Parse(applicationSettings.InstallationBasicFee, CultureInfo.InvariantCulture),
                    double.Parse(applicationSettings.ShippingBasicFee, CultureInfo.InvariantCulture));
            }
            return offerDetails;
        }

        public async Task AddApplianceAsync(Guid orderId, ApplianceCreateByOfferDto dto)
        {
            var order = await orderRepository.GetOrderByIdByAddService(orderId);

            var appliance = await applianceMaterialRepository.SingleIncludingAsync(ent => ent.Id == dto.ApplianceMaterialId);
            var orderedApplianceMaterial = dto.CreateModelObject(order.Id);
            order.AddAppliance(orderedApplianceMaterial);
            order.OfferInformation.ProductsPrice.Add(orderedApplianceMaterial.Quantity * appliance.SellPrice.Value);

            foreach (var service in order.Services)
            {
                AddExistingServicePricesToOrder(order, service, dto.Quantity);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<ApplianceDetailsByOfferDto> GetApplianceAsync(Guid orderId, int orderedApplianceMaterialId)
        {
            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId, x => x.OrderedApplianceMaterials);
            var orderedApplianceMaterial = order.OrderedApplianceMaterials.Single(ent => ent.Id == orderedApplianceMaterialId);
            return new ApplianceDetailsByOfferDto(orderedApplianceMaterial);
        }

        public async Task UpdateApplianceAsync(Guid orderId, int orderedApplianceMaterialId, ApplianceUpdateByOfferDto dto)
        {
            var order = await orderRepository.GetOrderWithOrderedAppliances(orderId);
            var orderedAppliance = order.OrderedApplianceMaterials.Single(ent => ent.Id == orderedApplianceMaterialId);
            order.OfferInformation.ProductsPrice.Div(orderedAppliance.Quantity * orderedAppliance.ApplianceMaterial.SellPrice.Value);
            dto.UpdateModelObject(orderedAppliance);

            order.OfferInformation.ProductsPrice.Add(orderedAppliance.Quantity * orderedAppliance.ApplianceMaterial.SellPrice.Value);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteApplianceFromAplliancesListAsync(Guid orderId, int orderedApplianceMaterialId)
        {
            var order = await orderRepository.GetOrderWithOrderedAppliances(orderId);

            var orderedApplianceMaterial = order.OrderedApplianceMaterials.Single(ent => ent.Id == orderedApplianceMaterialId);
            order.RemoveAppliance(orderedApplianceMaterial);
            order.OfferInformation.ProductsPrice.Div(orderedApplianceMaterial.Quantity * orderedApplianceMaterial.ApplianceMaterial.SellPrice.Value);

            SetServicePriceAfterOrderedApplianceDeleted(order, orderedApplianceMaterial);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<OfferPreviewDto> OfferPreview(Guid orderId)
        {
            var order = await orderRepository.GetOrderWithIncludesById(orderId);
            var renderers = await documentRepository
                .GetAllListAsync(ent => ent.DocumentType.Type == DocumentTypeEnum.Render
                && ent.DocumentGroupVersion.Core.OrderId == orderId
                && ent.DocumentGroupVersion.State.State == DocumentStateEnum.Approved);

            var offerPreview = new OfferPreviewDto(order) { Renderers = renderers.Select(ent => DocumentDetailsDto.FromModel(ent)).ToList() };

            offerPreview.Prices.SetPricesValue(order,
                    order.OfferInformation.ProductsPrice,
                    order.OfferInformation.ServicesPrice,
                    double.Parse(applicationSettings.VAT, CultureInfo.InvariantCulture),
                    double.Parse(applicationSettings.AssemblyPrice, CultureInfo.InvariantCulture),
                    double.Parse(applicationSettings.InstallationFurnitureUnit, CultureInfo.InvariantCulture),
                    double.Parse(applicationSettings.InstallationBasicFee, CultureInfo.InvariantCulture),
                    double.Parse(applicationSettings.ShippingBasicFee, CultureInfo.InvariantCulture));

            SetServicePricesByOfferPreview(order, offerPreview);

            return offerPreview;
        }

        public async Task AddServiceAsync(Guid orderId, ServiceCreateByOfferDto dto)
        {
            var order = await orderRepository.GetOrderByIdByAddService(orderId);

            if (dto.ServiceType == ServiceTypeEnum.Shipping)
            {
                var shippingService = await serviceRepository.SingleIncludingAsync(ent => ent.Id == dto.ShippingServicePriceId,
                    ent => ent.CurrentPrice.Price);

                if (dto.IsAdded)
                {
                    var choosedShipping = order.Services.SingleOrDefault(ent => ent.OrderId == order.Id && ent.Service.ServiceType.Type == ServiceTypeEnum.Shipping
                                                && ent.ServiceId != dto.ShippingServicePriceId);
                    if (choosedShipping != null)
                    {
                        choosedShipping.ServiceId = dto.ShippingServicePriceId.Value;
                        SetServicePriceAfterDropdownChanged(order, choosedShipping.Service.CurrentPrice.Price.Value,
                            shippingService.CurrentPrice.Price.Value);
                    }
                    else
                    {
                        var orderedService = dto.CreateModelObject(order.Id, shippingService.Id);
                        AddOrderedService(order, orderedService, shippingService.CurrentPrice.Price.Value +
                            double.Parse(applicationSettings.ShippingBasicFee, CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    // TODO div basicfee?
                    RemoveOrderedService(order, shippingService.CurrentPrice.Price.Value +
                        double.Parse(applicationSettings.ShippingBasicFee, CultureInfo.InvariantCulture),
                        shippingService.Id);
                }
            }

            else if (dto.ServiceType == ServiceTypeEnum.Assembly)
            {
                var assemblyService = await serviceRepository.SingleIncludingAsync(ent => ent.ServiceType.Type == ServiceTypeEnum.Assembly,
                    ent => ent.CurrentPrice.Price, ent => ent.CurrentPrice.Price.Currency);

                var sumQuantity = order.OrderedFurnitureUnits.Select(ent => ent.Quantity).Sum();
                var servicePrice = sumQuantity * double.Parse(applicationSettings.AssemblyPrice);

                if (dto.IsAdded)
                {
                    var orderedService = dto.CreateModelObject(order.Id, assemblyService.Id);
                    AddOrderedService(order, orderedService, servicePrice);
                }
                else
                {
                    RemoveOrderedService(order, servicePrice, assemblyService.Id);
                }
            }

            else if (dto.ServiceType == ServiceTypeEnum.Installation)
            {
                var installationService = await serviceRepository.SingleIncludingAsync(ent => ent.ServiceType.Type == ServiceTypeEnum.Installation,
                    ent => ent.CurrentPrice.Price, ent => ent.CurrentPrice.Price.Currency);

                var servicePrice = CalculateInstallationPrice(order);

                if (dto.IsAdded)
                {
                    var orderedService = dto.CreateModelObject(order.Id, installationService.Id);
                    AddOrderedService(order, orderedService, servicePrice);
                }
                else
                {
                    RemoveOrderedService(order, servicePrice, installationService.Id);
                }
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task SetVatAsync(Guid orderId, bool isVat)
        {
            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId, ent => ent.OfferInformation);
            order.OfferInformation.IsVatRequired = isVat;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedListDto<OrderFinanceListDto>> GetOrdersByCompanyAsync(int companyId, OrderFinanceFilterDto filterDto)
        {
            Expression<Func<Order, bool>> filter = (Order o) => true;
            Ensure.NotNull(filterDto);

            filter = CreateFilter(filterDto, filter);
            var orderingQuery = filterDto.Orderings.ToOrderingExpression<Order>(
                OrderFinanceFilterDto.GetColumnMappings(), nameof(Order.Id));

            var orders = await orderRepository.GetOrdersByCompany(companyId, filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return orders.ToPagedList(OrderFinanceListDto.FromEntity);
        }


        public async Task AddOrderPaymentAsync(Guid orderId, OrderFinanceCreateDto orderFinanceCreateDto)
        {
            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId, ent => ent.FirstPayment, ent => ent.SecondPayment);
            switch (orderFinanceCreateDto.PaymentIndex)
            {
                case 1:
                    order.FirstPayment.SetPaymentDate(orderFinanceCreateDto.PaymentDate);
                    break;
                case 2:
                    order.SecondPayment.SetPaymentDate(orderFinanceCreateDto.PaymentDate);
                    break;
                default:
                    throw new IFPSValidationAppException(new Dictionary<string, List<string>>() { { "PaymentIndex", new List<string>() { "PaymentIndex must be 1 or 2! " } } });
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<ContractDetailsDto> GetContractAsync(Guid orderId)
        {
            var order = await orderRepository.GetOrderForContractById(orderId);
            var contractDto = new ContractDetailsDto(order);

            contractDto.Producer.BankAccount = applicationSettings.BankAccount;

            contractDto.Financial.PaymentDetails.SetPricesValue(order,
                    order.OfferInformation.ProductsPrice,
                    order.OfferInformation.ServicesPrice,
                    double.Parse(applicationSettings.VAT, CultureInfo.InvariantCulture),
                    double.Parse(applicationSettings.AssemblyPrice, CultureInfo.InvariantCulture),
                    double.Parse(applicationSettings.InstallationFurnitureUnit, CultureInfo.InvariantCulture),
                    double.Parse(applicationSettings.InstallationBasicFee, CultureInfo.InvariantCulture),
                    double.Parse(applicationSettings.ShippingBasicFee, CultureInfo.InvariantCulture));

            if (order.FirstPayment == null && order.SecondPayment == null)
            {
                var total = contractDto.Financial.PaymentDetails.Total;
                contractDto.Financial.FirstPayment = new PriceListDto(total.Value.Value, total.CurrencyId.Value, total.Currency);
                contractDto.Financial.FirstPayment.Value *= DIVIDE_BY_TWO;
                contractDto.Financial.FirstPaymentDate = Clock.Now.AddDays(7);
                contractDto.Financial.SecondPayment = new PriceListDto(contractDto.Financial.FirstPayment.Value.Value, total.CurrencyId.Value, total.Currency);
                contractDto.Financial.SecondPaymentDate = Clock.Now.AddMonths(2);
            }
            else
            {
                contractDto.Financial.FirstPayment = new PriceListDto(order.FirstPayment.Price);
                contractDto.Financial.FirstPaymentDate = order.FirstPayment.Deadline;
                contractDto.Financial.SecondPayment = new PriceListDto(order.SecondPayment.Price);
                contractDto.Financial.SecondPaymentDate = order.SecondPayment.Deadline;
            }
            return contractDto;
        }

        public async Task CreateContractAsync(Guid orderId, ContractCreateDto contractCreateDto)
        {
            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId,
                ent => ent.FirstPayment, ent => ent.SecondPayment);

            order = contractCreateDto.UpdateModelObject(order);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ApproveDocuments(Guid orderId, int documentGroupVersionId, DocumentStateEnum result, int callerId)
        {
            if (result != DocumentStateEnum.Approved && result != DocumentStateEnum.Declined)
            {
                throw new IFPSAppException("Result can be Approved or Declined only!");
            }
            var customer = await customerRepository.GetByUserIdAsync(callerId);
            var version = await orderRepository.GetDocumentGroupVersionWithIncludesAsync(orderId, documentGroupVersionId);
            if (version.Core.Order.CustomerId != customer.Id)
            {
                throw new IFPSAppException("Can not approve the documents of an order that is assigned to someone else!");
            }
            if (!version.Core.IsHistorized || version.State.State != DocumentStateEnum.WaitingForApproval)
            {
                throw new IFPSAppException("Can not approve this version!");
            }

            var state = await documentStateRepository.SingleAsync(x => x.State == result);
            version.State = state;

            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId, ent => ent.CurrentTicket, ent => ent.SalesPerson, ent => ent.Customer.User.CurrentVersion);

            if (version.Core.DocumentFolder.DocumentTypes.Any(ent => ent.Type == DocumentTypeEnum.Offer))
            {
                if (result == DocumentStateEnum.Declined)
                {
                    order.SetWaitingForOfferStateAfterDeclined();
                }

                if (result == DocumentStateEnum.Approved)
                {
                    order.SetWaitingForOnSiteSurveyAppointmentReservationState();
                }
            }

            if (version.Core.DocumentFolder.DocumentTypes.Any(ent => ent.Type == DocumentTypeEnum.Contract))
            {
                if (result == DocumentStateEnum.Declined)
                {
                    order.SetWaitingForContractStateAfterDeclined();
                }

                if (result == DocumentStateEnum.Approved)
                {
                    order.SetUnderProductionState();
                }
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<UserDropdownAvatarDto> GetCustomerByOrderAsync(Guid orderId)
        {
            var customerAvatar = (await orderRepository
                .GetAllListAsync<UserDropdownAvatarDto>(x => x.Id == orderId, UserDropdownAvatarDto.CustomerProjection))
                .SingleOrDefault();

            return customerAvatar ?? throw new EntityNotFoundException($"The order with id {orderId} has no Customer");
        }

        public async Task SetShippingStateAsync(Guid orderId)
        {
            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId, ent => ent.Tickets, ent => ent.CurrentTicket, ent => ent.SalesPerson);
            var state = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.Delivered);
            order.AddTicket(new Ticket(state.Id, order.SalesPerson.UserId, orderStateConfiguration.Delivered));
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SetInstallationStateAsync(Guid orderId)
        {
            var order = await orderRepository.SingleIncludingAsync(ent => ent.Id == orderId, ent => ent.Tickets, ent => ent.CurrentTicket, ent => ent.SalesPerson);
            var state = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.Installed);
            order.AddTicket(new Ticket(state.Id, order.SalesPerson.UserId, orderStateConfiguration.Installed));
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SetWaitingForContractStateAsync(Guid orderId, int partnerId)
        {
            var order = await orderRepository.SingleAsync(ent => ent.Id == orderId);
            order.SetWaitingForContractState(partnerId);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DownloadDocuments(Guid orderId, System.IO.MemoryStream memoryStream)
        {
            List<DocumentDetailsDto> documents = await documentAppService.GetAllDocumentsAsync(orderId);
            //documentgroup --> documentGroupVersion
            using (System.IO.Compression.ZipArchive zip = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                foreach (var document in documents)
                {
                    System.IO.Compression.ZipArchiveEntry zipItem = zip.CreateEntry(document.FileName);

                    byte[] file = System.IO.File.ReadAllBytes(contentRootPath + @"\OrderDocuments\" + document.FileName);
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(file))
                    {
                        using (System.IO.Stream entry = zipItem.Open())
                        {
                            await ms.CopyToAsync(entry);
                        }
                    }
                }
            }
        }



        private static Expression<Func<Order, bool>> CreateFilter(OrderFinanceFilterDto filterDto, Expression<Func<Order, bool>> filter)
        {
            if (!string.IsNullOrWhiteSpace(filterDto.OrderId))
            {
                filter = filter.And(ent => ent.OrderName.ToLower().Contains(filterDto.OrderId.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(filterDto.WorkingNumber))
            {
                filter = filter.And(ent => ent.WorkingNumber.ToLower().Contains(filterDto.WorkingNumber.ToLower()));
            }
            if (filterDto.CurrentStatusId != null)
            {
                filter = filter.And(ent => ent.CurrentTicket.OrderStateId == filterDto.CurrentStatusId);
            }
            if (filterDto.StatusDeadline != null)
            {
                filter = filter.And(ent => ent.CurrentTicket.Deadline == filterDto.StatusDeadline);
            }
            if (!string.IsNullOrWhiteSpace(filterDto.Customer))
            {
                filter = filter.And(ent => ent.Customer.User.CurrentVersion.Name.ToLower().Contains(filterDto.Customer.ToLower()));
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
        private void CreateFilter(out Expression<Func<Order, bool>> filter, OrderFilterDto filterDto)
        {
            filter = (Order o) => o.CurrentTicket.OrderState.State != OrderStateEnum.Completed;

            Ensure.NotNull(filterDto);

            if (!string.IsNullOrWhiteSpace(filterDto.OrderId))
            {
                filter = filter.And(ent => ent.OrderName.ToLower().Contains(filterDto.OrderId.ToLower()));
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
                filter = filter.And(ent => ent.CurrentTicket.Deadline >= filterDto.StatusDeadlineFrom);
            }
            if (filterDto.StatusDeadlineTo != null)
            {
                filter = filter.And(ent => ent.CurrentTicket.Deadline <= filterDto.StatusDeadlineTo);
            }
            if (!string.IsNullOrWhiteSpace(filterDto.Responsible))
            {
                filter = filter.And(ent => ent.CurrentTicket.AssignedTo.CurrentVersion.Name.ToLower().Contains(filterDto.Responsible.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(filterDto.Customer))
            {
                filter = filter.And(ent => ent.Customer.User.CurrentVersion.Name.ToLower().Contains(filterDto.Customer.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(filterDto.Sales))
            {
                filter = filter.And(ent => ent.SalesPerson.User.CurrentVersion.Name.ToLower().Contains(filterDto.Sales.ToLower()));
            }
            if (filterDto.CreatedOnFrom != null)
            {
                filter = filter.And(ent => ent.CreationTime >= filterDto.CreatedOnFrom);
            }
            if (filterDto.CreatedOnTo != null)
            {
                filter = filter.And(ent => ent.CreationTime <= filterDto.CreatedOnTo);
            }
            if (filterDto.DeadlineFrom != null)
            {
                filter = filter.And(ent => ent.Deadline >= filterDto.DeadlineFrom);
            }
            if (filterDto.DeadlineTo != null)
            {
                filter = filter.And(ent => ent.Deadline <= filterDto.DeadlineTo);
            }
        }

        private FurnitureComponent CompareToTheComponents(FurnitureComponent parent, FurnitureComponentsCreateByOfferDto dto)
        {
            if (!parent.Name.Equals(dto.Name))
            {
                parent.Name = dto.Name;
            }
            if (parent.Width != dto.Width)
            {
                parent.Width = dto.Width;
            }
            if (parent.Length != dto.Height)
            {
                parent.Length = dto.Height;
            }
            if (parent.Amount != dto.Amount)
            {
                parent.Amount = dto.Amount;
            }
            if (parent.BoardMaterialId != dto.BoardMaterialId)
            {
                parent.BoardMaterialId = dto.BoardMaterialId;
            }
            if (parent.BottomFoilId != dto.BottomFoilId)
            {
                parent.BottomFoilId = dto.BottomFoilId;
            }
            if (parent.TopFoilId != dto.TopFoilId)
            {
                parent.TopFoilId = dto.TopFoilId;
            }
            if (parent.RightFoilId != dto.RightFoilId)
            {
                parent.RightFoilId = dto.RightFoilId;
            }
            if (parent.LeftFoilId != dto.LeftFoilId)
            {
                parent.LeftFoilId = dto.LeftFoilId;
            }
            return parent;
        }

        private double CalculateInstallationPrice(Order order)
        {
            double installationPrice = 0;
            if (order.OrderedFurnitureUnits.Count() > 0 || order.OrderedApplianceMaterials.Count() > 0)
            {
                installationPrice = order.OrderedFurnitureUnits.Select(ent => ent.Quantity).Sum() *
                                        double.Parse(applicationSettings.InstallationFurnitureUnit) +
                                    order.OrderedApplianceMaterials.Select(ent => ent.Quantity).Sum() *
                                        double.Parse(applicationSettings.InstallationFurnitureUnit) +
                                        double.Parse(applicationSettings.InstallationBasicFee);
            }

            return installationPrice;
        }

        private void AddOrderedService(Order order, OrderedService orderedService, double servicePrice)
        {
            order.AddService(orderedService);
            order.OfferInformation.ServicesPrice.Add(servicePrice);
        }

        private void RemoveOrderedService(Order order, double servicePrice, int id)
        {
            var orderedService = order.Services.SingleOrDefault(ent => ent.OrderId == order.Id && ent.ServiceId == id);
            order.RemoveService(orderedService);
            order.OfferInformation.ServicesPrice.Div(servicePrice);
        }

        private void SetServicePriceAfterDropdownChanged(Order order, double lastPrice, double nextPrice)
        {
            order.OfferInformation.ServicesPrice.Div(lastPrice);
            order.OfferInformation.ServicesPrice.Add(nextPrice);
        }

        private void SetServicePriceAfterOrderedFurnitureUnitDeleted(Order order, OrderedFurnitureUnit orderedFurnitureUnit)
        {
            if (order.Services.Count() > 0)
            {
                if (order.Services.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Assembly))
                {
                    order.OfferInformation.ServicesPrice.Div(orderedFurnitureUnit.Quantity * double.Parse(applicationSettings.AssemblyPrice, CultureInfo.InvariantCulture));
                }

                if (order.Services.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Installation))
                {
                    if (order.OrderedFurnitureUnits.Count() == 0 && order.OrderedApplianceMaterials.Count() == 0)
                    {
                        order.OfferInformation.ServicesPrice.Div(double.Parse(applicationSettings.InstallationBasicFee) +
                        double.Parse(applicationSettings.InstallationFurnitureUnit, CultureInfo.InvariantCulture) * orderedFurnitureUnit.Quantity);
                    }
                    else
                    {
                        order.OfferInformation.ServicesPrice.Div(//double.Parse(applicationSettings.InstallationBasicFee) +
                        double.Parse(applicationSettings.InstallationFurnitureUnit, CultureInfo.InvariantCulture) * orderedFurnitureUnit.Quantity);
                    }
                }
            }

            ClearServices(order);
        }

        private void SetServicePriceAfterOrderedApplianceDeleted(Order order, OrderedApplianceMaterial orderedApplianceMaterial)
        {
            if (order.Services.Count() > 0)
            {
                if (order.Services.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Installation))
                {
                    if (order.OrderedFurnitureUnits.Count() == 0 && order.OrderedApplianceMaterials.Count() == 0)
                    {
                        order.OfferInformation.ServicesPrice.Div(double.Parse(applicationSettings.InstallationBasicFee) +
                        double.Parse(applicationSettings.InstallationFurnitureUnit, CultureInfo.InvariantCulture) * orderedApplianceMaterial.Quantity);
                    }
                    else
                    {
                        order.OfferInformation.ServicesPrice.Div(//double.Parse(applicationSettings.InstallationBasicFee) +
                        double.Parse(applicationSettings.InstallationFurnitureUnit, CultureInfo.InvariantCulture) * orderedApplianceMaterial.Quantity);
                    }
                }
            }

            ClearServices(order);
        }

        private void ClearServices(Order order)
        {
            if (order.Services.Count() > 0)
            {
                if (order.Services.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Shipping))
                {
                    var choosedService = order.Services.Single(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Shipping);

                    order.OfferInformation.ServicesPrice.Div(double.Parse(applicationSettings.ShippingBasicFee, CultureInfo.InvariantCulture) +
                        choosedService.Service.CurrentPrice.Price.Value);
                }
            }

            if (order.OrderedFurnitureUnits.Count() == 0 && order.OrderedApplianceMaterials.Count() == 0 && order.Services.Count() != 0)
            {
                order.ClearOrderedServicesList();
            }
        }

        private void SetServicePricesByOfferPreview(Order order, OfferPreviewDto offerPreview)
        {
            var orderedServices = order.Services.Where(ent => ent.OrderId == order.Id).ToList();

            if (order.OfferInformation.IsVatRequired == false)
            {
                offerPreview.Prices.Vat.Value = 0;
            }

            if (orderedServices.Count() != 0)
            {
                if (!orderedServices.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Assembly))
                {
                    offerPreview.Prices.Assembly.Value = 0;
                }

                if (!orderedServices.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Installation))
                {
                    offerPreview.Prices.Installation.Value = 0;
                }

                if (!orderedServices.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Shipping))
                {
                    offerPreview.Prices.Shipping.Value = 0;
                }
            }
            else
            {
                offerPreview.Prices.Assembly.Value = 0;
                offerPreview.Prices.Installation.Value = 0;
                offerPreview.Prices.Shipping.Value = 0;
            }
        }
    }
}
