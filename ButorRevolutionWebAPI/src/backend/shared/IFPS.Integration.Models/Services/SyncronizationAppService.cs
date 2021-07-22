using ENCO.DDD.Service;
using IFPS.Factory.Domain.Repositories;
using IFPS.Integration.Application.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IFPS.Integration.Application.Services
{
    public class SynchronizationAppService : ApplicationService, ISynchronizationAppService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly LocalConnectionConfiguration config;

        private readonly Sales.Domain.Repositories.IAccessoryMaterialRepository salesAccessoryRepository;
        private readonly Factory.Domain.Repositories.IAccessoryMaterialRepository factoryAccessoryRepository;
        private readonly Sales.Domain.Repositories.IApplianceMaterialRepository salesApplianceRepository;
        private readonly Factory.Domain.Repositories.IApplianceMaterialRepository factoryApplianceRepository;
        private readonly Sales.Domain.Repositories.IFoilMaterialRepository salesFoilRepository;
        private readonly Factory.Domain.Repositories.IFoilMaterialRepository factoryFoilRepository;
        private readonly Sales.Domain.Repositories.IDecorBoardMaterialRepository salesDecorBoardRepository;
        private readonly Factory.Domain.Repositories.IDecorBoardMaterialRepository factoryDecorBoardRepository;
        private readonly Sales.Domain.Repositories.IWorktopBoardMaterialRepository salesWorktopBoardRepository;
        private readonly Factory.Domain.Repositories.IWorktopBoardMaterialRepository factoryWorktopBoardRepository;
        private readonly Factory.Domain.Repositories.IStockedMaterialRepository stockedMaterialRepository;
        private readonly Sales.Domain.Repositories.IFurnitureUnitRepository salesFurnitureUnitRepository;
        private readonly Factory.Domain.Repositories.IFurnitureUnitRepository factoryFurnitureUnitRepository;
        private readonly Sales.Domain.Repositories.IFurnitureComponentRepository salesFurnitureComponentRepository;
        private readonly Factory.Domain.Repositories.IFurnitureComponentRepository factoryFurnitureComponentRepository;
        private readonly Sales.Domain.Repositories.IOrderRepository salesOrderRepository;
        private readonly Factory.Domain.Repositories.IOrderRepository factoryOrderRepository;
        private readonly Sales.Domain.Repositories.IImageRepository salesImageRepository;
        private readonly Factory.Domain.Repositories.IImageRepository factoryImageRepository;
        private readonly Sales.Domain.Repositories.IDocumentRepository salesDocumentRepository;
        private readonly Factory.Domain.Repositories.IDocumentRepository factoryDocumentRepository;
        private readonly Sales.Domain.Repositories.IOrderStateRepository salesOrderStateRepository;
        private readonly Factory.Domain.Repositories.IOrderStateRepository factoryOrderStateRepository;
        private readonly Sales.Domain.Repositories.IGroupingCategoryRepository salesGroupingCategoryRepository;
        private readonly Factory.Domain.Repositories.IGroupingCategoryRepository factoryGroupingCategoryRepository;
        private readonly Sales.Domain.Repositories.IWebshopOrderRepository salesWebshopOrderRepository;
        private readonly Factory.Domain.Repositories.IAccessoryFurnitureUnitRepository factoryAccessoryFurnitureUnitRepository;
        private readonly Sales.Domain.Repositories.IAccessoryFurnitureUnitRepository salesAccessoryFurnitureUnitRepository;
        private readonly Factory.Domain.Repositories.ISiUnitRepository factorySiUnitRepository;
        private readonly IUserRepository factoryUserRepository;

        public SynchronizationAppService(
            IHttpClientFactory clientFactory,
            IOptions<LocalConnectionConfiguration> options,
            IApplicationServiceDependencyAggregate aggregate,
            Sales.Domain.Repositories.IAccessoryMaterialRepository salesAccessoryRepository,
            Factory.Domain.Repositories.IAccessoryMaterialRepository factoryAccessoryRepository,
            Sales.Domain.Repositories.IFoilMaterialRepository salesFoilRepository,
            Factory.Domain.Repositories.IFoilMaterialRepository factoryFoilRepository,
            Sales.Domain.Repositories.IApplianceMaterialRepository salesApplianceRepository,
            Factory.Domain.Repositories.IApplianceMaterialRepository factoryApplianceRepository,
            Sales.Domain.Repositories.IDecorBoardMaterialRepository salesDecorBoardRepository,
            Factory.Domain.Repositories.IDecorBoardMaterialRepository factoryDecorBoardRepository,
            Sales.Domain.Repositories.IWorktopBoardMaterialRepository salesWorktopBoardRepository,
            Factory.Domain.Repositories.IWorktopBoardMaterialRepository factoryWorktopBoardRepository,
            Factory.Domain.Repositories.IStockedMaterialRepository stockedMaterialRepository,
            Sales.Domain.Repositories.IFurnitureUnitRepository salesFurnitureUnitRepository,
            Factory.Domain.Repositories.IFurnitureUnitRepository factoryFurnitureUnitRepository,
            Sales.Domain.Repositories.IFurnitureComponentRepository salesFurnitureComponentRepository,
            Factory.Domain.Repositories.IFurnitureComponentRepository factoryFurnitureComponentRepository,
            Sales.Domain.Repositories.IOrderRepository salesOrderRepository,
            Factory.Domain.Repositories.IOrderRepository factoryOrderRepository,
            Sales.Domain.Repositories.IImageRepository salesImageRepository,
            Factory.Domain.Repositories.IImageRepository factoryImageRepository,
            Sales.Domain.Repositories.IDocumentRepository salesDocumentRepository,
            Factory.Domain.Repositories.IDocumentRepository factoryDocumentRepository,
            Sales.Domain.Repositories.IOrderStateRepository salesOrderStateRepository,
            Factory.Domain.Repositories.IOrderStateRepository factoryOrderStateRepository,
            Sales.Domain.Repositories.IGroupingCategoryRepository salesGroupingCategoryRepository,
            Factory.Domain.Repositories.IGroupingCategoryRepository factoryGroupingCategoryRepository,
            Sales.Domain.Repositories.IWebshopOrderRepository salesWebshopOrderRepository,
            Factory.Domain.Repositories.IAccessoryFurnitureUnitRepository factoryAccessoryFurnitureUnitRepository,
            Sales.Domain.Repositories.IAccessoryFurnitureUnitRepository salesAccessoryFurnitureUnitRepository,
            Factory.Domain.Repositories.ISiUnitRepository factorySiUnitRepository,
            Factory.Domain.Repositories.IUserRepository factoryUserRepository
            ) : base(aggregate)
        {
            this.clientFactory = clientFactory;
            this.config = options.Value;
            this.salesAccessoryRepository = salesAccessoryRepository;
            this.factoryAccessoryRepository = factoryAccessoryRepository;
            this.salesApplianceRepository = salesApplianceRepository;
            this.factoryApplianceRepository = factoryApplianceRepository;
            this.salesFoilRepository = salesFoilRepository;
            this.factoryFoilRepository = factoryFoilRepository;
            this.salesDecorBoardRepository = salesDecorBoardRepository;
            this.factoryDecorBoardRepository = factoryDecorBoardRepository;
            this.salesWorktopBoardRepository = salesWorktopBoardRepository;
            this.factoryWorktopBoardRepository = factoryWorktopBoardRepository;
            this.stockedMaterialRepository = stockedMaterialRepository;
            this.factoryFurnitureUnitRepository = factoryFurnitureUnitRepository;
            this.salesFurnitureUnitRepository = salesFurnitureUnitRepository;
            this.salesFurnitureComponentRepository = salesFurnitureComponentRepository;
            this.factoryFurnitureComponentRepository = factoryFurnitureComponentRepository;
            this.salesOrderRepository = salesOrderRepository;
            this.factoryOrderRepository = factoryOrderRepository;
            this.salesImageRepository = salesImageRepository;
            this.factoryImageRepository = factoryImageRepository;
            this.salesDocumentRepository = salesDocumentRepository;
            this.factoryDocumentRepository = factoryDocumentRepository;
            this.salesOrderStateRepository = salesOrderStateRepository;
            this.factoryOrderStateRepository = factoryOrderStateRepository;
            this.salesGroupingCategoryRepository = salesGroupingCategoryRepository;
            this.factoryGroupingCategoryRepository = factoryGroupingCategoryRepository;
            this.salesWebshopOrderRepository = salesWebshopOrderRepository;
            this.factoryAccessoryFurnitureUnitRepository = factoryAccessoryFurnitureUnitRepository;
            this.salesAccessoryFurnitureUnitRepository = salesAccessoryFurnitureUnitRepository;
            this.factorySiUnitRepository = factorySiUnitRepository;
            this.factoryUserRepository = factoryUserRepository;
        }

        public async Task SyncronizeAccessoryMaterialToFactoryAsync()
        {
            var factoryAccessories = await factoryAccessoryRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var salesAccessories = await salesAccessoryRepository.GetAllListIncludingAsync(ent => !factoryAccessories.Contains(ent.Id), ent => ent.Image, ent => ent.CurrentPrice.Price);
            var siUnit = await factorySiUnitRepository.SingleAsync(ent => ent.UnitType == Factory.Domain.Enums.SiUnitEnum.Pcs);

            foreach (var accessory in salesAccessories)
            {
                var newAccessory = new Factory.Domain.Model.AccessoryMaterial(accessory.IsOptional, accessory.IsRequiredForAssembly, accessory.Code, accessory.TransactionMultiplier)
                {
                    Id = accessory.Id,
                    Description = accessory.Description,
                    CategoryId = accessory.CategoryId
                };
                await factoryAccessoryRepository.InsertAsync(newAccessory);
                newAccessory.AddPrice(new Factory.Domain.Model.MaterialPrice()
                {
                    Price = new Factory.Domain.Model.Price(accessory.CurrentPrice.Price.Value, accessory.CurrentPrice.Price.CurrencyId)
                });
                await CreateStockedMaterialIfNotExist(accessory.Id);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeApplianceMaterialToFactoryAsync()
        {
            var factoryAppliances = await factoryApplianceRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var salesAppliances = await salesApplianceRepository.GetAllListIncludingAsync(ent => !factoryAppliances.Contains(ent.Id), ent => ent.Image, ent => ent.CurrentPrice.Price);
            var siUnit = await factorySiUnitRepository.SingleAsync(ent => ent.UnitType == Factory.Domain.Enums.SiUnitEnum.Pcs);

            foreach (var appliance in salesAppliances)
            {
                var newAppliance = new Factory.Domain.Model.ApplianceMaterial(appliance.Code)
                {
                    Id = appliance.Id,
                    Description = appliance.Description,
                    CategoryId = appliance.CategoryId,
                    HanaCode = appliance.HanaCode,
                    SellPrice = new Factory.Domain.Model.Price(appliance.CurrentPrice.Price.Value, appliance.CurrentPrice.Price.CurrencyId)
                };
                await factoryApplianceRepository.InsertAsync(newAppliance);
                newAppliance.AddPrice(new Factory.Domain.Model.MaterialPrice()
                {
                    Price = new Factory.Domain.Model.Price(appliance.CurrentPrice.Price.Value, appliance.CurrentPrice.Price.CurrencyId)
                });
                await CreateStockedMaterialIfNotExist(appliance.Id);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeDecorBoardMaterialToFactoryAsync()
        {
            var factoryDecorBoards = await factoryDecorBoardRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var salesDecorBoards = await salesDecorBoardRepository.GetAllListIncludingAsync(ent => !factoryDecorBoards.Contains(ent.Id), ent => ent.Image, ent => ent.CurrentPrice.Price);
            var siUnit = await factorySiUnitRepository.SingleAsync(ent => ent.UnitType == Factory.Domain.Enums.SiUnitEnum.Pcs);

            foreach (var decorBoard in salesDecorBoards)
            {
                var newDecorBoard = new Factory.Domain.Model.DecorBoardMaterial(decorBoard.Code, decorBoard.TransactionMultiplier)
                {
                    Id = decorBoard.Id,
                    Description = decorBoard.Description,
                    CategoryId = decorBoard.CategoryId,
                    Dimension = new Factory.Domain.Model.Dimension(decorBoard.Dimension.Width, decorBoard.Dimension.Length, decorBoard.Dimension.Thickness),
                    HasFiberDirection = decorBoard.HasFiberDirection
                };
                await factoryDecorBoardRepository.InsertAsync(newDecorBoard);
                newDecorBoard.AddPrice(new Factory.Domain.Model.MaterialPrice()
                {
                    Price = new Factory.Domain.Model.Price(decorBoard.CurrentPrice.Price.Value, decorBoard.CurrentPrice.Price.CurrencyId)
                });
                await CreateStockedMaterialIfNotExist(decorBoard.Id);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeFoilMaterialToFactoryAsync()
        {
            var factoryFoils = await factoryFoilRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var salesFoils = await salesFoilRepository.GetAllListIncludingAsync(ent => !factoryFoils.Contains(ent.Id), ent => ent.Image, ent => ent.CurrentPrice.Price);
            var siUnit = await factorySiUnitRepository.SingleAsync(ent => ent.UnitType == Factory.Domain.Enums.SiUnitEnum.Pcs);

            foreach (var foil in salesFoils)
            {
                var newFoil = new Factory.Domain.Model.FoilMaterial(foil.Code, foil.TransactionMultiplier)
                {
                    Id = foil.Id,
                    Description = foil.Description,
                    Thickness = foil.Thickness,
                    CategoryId = foil.CategoryId
                };
                await factoryFoilRepository.InsertAsync(newFoil);
                newFoil.AddPrice(new Factory.Domain.Model.MaterialPrice()
                {
                    Price = new Factory.Domain.Model.Price(foil.CurrentPrice.Price.Value, foil.CurrentPrice.Price.CurrencyId)
                });
                await CreateStockedMaterialIfNotExist(foil.Id);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeFurnitureUnitToFactoryAsync()
        {
            var factoryFurnitureUnits = await factoryFurnitureUnitRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var salesFurnitureUnits = await salesFurnitureUnitRepository.GetAllListIncludingAsync(ent => !factoryFurnitureUnits.Contains(ent.Id), ent => ent.Image, ent => ent.CurrentPrice.Price);

            foreach (var furnitureUnit in salesFurnitureUnits)
            {
                var newFurnitureUnit = new Factory.Domain.Model.FurnitureUnit(furnitureUnit.Code, furnitureUnit.Width, furnitureUnit.Height, furnitureUnit.Depth)
                {
                    Id = furnitureUnit.Id,
                    Description = furnitureUnit.Description,
                    CategoryId = furnitureUnit.CategoryId
                };
                await factoryFurnitureUnitRepository.InsertAsync(newFurnitureUnit);
                newFurnitureUnit.AddPrice(new Factory.Domain.Model.FurnitureUnitPrice()
                {
                    Price = new Factory.Domain.Model.Price(furnitureUnit.CurrentPrice.Price.Value, furnitureUnit.CurrentPrice.Price.CurrencyId),
                    MaterialCost = new Factory.Domain.Model.Price(furnitureUnit.CurrentPrice.MaterialCost.Value, furnitureUnit.CurrentPrice.MaterialCost.CurrencyId)
                });
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeFurnitureComponentToFactoryAsync()
        {
            var factoryFurnitureComponents = await factoryFurnitureComponentRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var salesFurnitureComponents = await salesFurnitureComponentRepository.GetAllListIncludingAsync(ent => !factoryFurnitureComponents.Contains(ent.Id), ent => ent.Image);
            foreach (var furnitureComponent in salesFurnitureComponents)
            {
                var newFurnitureComponent = new Factory.Domain.Model.FurnitureComponent(furnitureComponent.Name, furnitureComponent.Width, furnitureComponent.Length, furnitureComponent.Amount)
                {
                    Id = furnitureComponent.Id,
                    FurnitureUnitId = furnitureComponent.FurnitureUnitId,
                    Type = (Factory.Domain.Enums.FurnitureComponentTypeEnum)furnitureComponent.Type,
                    BoardMaterialId = furnitureComponent.BoardMaterialId,
                    TopFoilId = furnitureComponent.TopFoilId,
                    BottomFoilId = furnitureComponent.BottomFoilId,
                    LeftFoilId = furnitureComponent.LeftFoilId,
                    RightFoilId = furnitureComponent.RightFoilId,
                    CenterPoint = new Factory.Domain.Model.AbsolutePoint(0, 0, 0),
                };
                await factoryFurnitureComponentRepository.InsertAsync(newFurnitureComponent);
                //TODO AbsolutePoint
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeAccessoryFurnitureUnitToFactoryAsync()
        {
            var factoryAccessoryFurnitureUnits = await factoryAccessoryFurnitureUnitRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var salesAccessoryFurnitureUnits = await salesAccessoryFurnitureUnitRepository.GetAllListAsync(ent => !factoryAccessoryFurnitureUnits.Contains(ent.Id));
            foreach (var furnitureUnit in salesAccessoryFurnitureUnits)
            {
                var newFurnitureUnit = new Factory.Domain.Model.AccessoryMaterialFurnitureUnit(furnitureUnit.FurnitureUnitId, furnitureUnit.AccessoryId, furnitureUnit.Name, furnitureUnit.AccessoryAmount);
                await factoryAccessoryFurnitureUnitRepository.InsertAsync(newFurnitureUnit);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeWorktopBoardMaterialToFactoryAsync()
        {
            var factoryWorktopBoards = await factoryWorktopBoardRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var salesWorktopBoards = await salesWorktopBoardRepository.GetAllListIncludingAsync(ent => !factoryWorktopBoards.Contains(ent.Id), ent => ent.Image, ent => ent.CurrentPrice.Price);
            var siUnit = await factorySiUnitRepository.SingleAsync(ent => ent.UnitType == Factory.Domain.Enums.SiUnitEnum.Pcs);

            foreach (var worktopBoard in salesWorktopBoards)
            {
                var newWorktopBoard = new Factory.Domain.Model.WorktopBoardMaterial(worktopBoard.Code, worktopBoard.TransactionMultiplier)
                {
                    Id = worktopBoard.Id,
                    Description = worktopBoard.Description,
                    CategoryId = worktopBoard.CategoryId,
                    Dimension = new Factory.Domain.Model.Dimension(worktopBoard.Dimension.Width, worktopBoard.Dimension.Length, worktopBoard.Dimension.Thickness),
                    HasFiberDirection = worktopBoard.HasFiberDirection
                };
                await factoryWorktopBoardRepository.InsertAsync(newWorktopBoard);
                newWorktopBoard.AddPrice(new Factory.Domain.Model.MaterialPrice()
                {
                    Price = new Factory.Domain.Model.Price(worktopBoard.CurrentPrice.Price.Value, worktopBoard.CurrentPrice.Price.CurrencyId)
                });
                await CreateStockedMaterialIfNotExist(worktopBoard.Id);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeOrderToFactoryAsync()
        {
            var factoryOrders = await factoryOrderRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var factoryAdmins = await factoryUserRepository.GetAllListAsync(ent => ent.Roles.Any(r => r.Role.Division.DivisionType == Factory.Domain.Enums.DivisionTypeEnum.Admin));
            var salesOrders = await salesOrderRepository.GetOrderWithOrderedFurnitureUnits(ent => (!factoryOrders.Contains(ent.Id) && ent.CurrentTicket.OrderState.State == Sales.Domain.Enums.OrderStateEnum.UnderProduction));
            foreach (var order in salesOrders)
            {
                var newOrder = new Factory.Domain.Model.Order(order.OrderName, order.Deadline)
                {
                    Id = order.Id,
                    WorkingNumber = order.WorkingNumber,
                    ShippingAddress = new Factory.Domain.Model.Address(order.ShippingAddress.PostCode, order.ShippingAddress.City, order.ShippingAddress.AddressValue, order.ShippingAddress.CountryId.GetValueOrDefault()),
                    CompanyId = order.SalesPerson.User.CompanyId.GetValueOrDefault(),
                    FirstPayment = new Factory.Domain.Model.OrderPrice(new Factory.Domain.Model.Price(order.FirstPayment.Price.Value, order.FirstPayment.Price.CurrencyId), order.FirstPayment.Deadline),
                    SecondPayment = new Factory.Domain.Model.OrderPrice(new Factory.Domain.Model.Price(order.SecondPayment.Price.Value, order.SecondPayment.Price.CurrencyId), order.SecondPayment.Deadline),
                };
                await factoryOrderRepository.InsertAsync(newOrder);
                foreach (var orderedFurnitureUnit in order.OrderedFurnitureUnits)
                {
                    var newConcreteFurnitureUnit = new Factory.Domain.Model.ConcreteFurnitureUnit(newOrder) { FurnitureUnitId = orderedFurnitureUnit.FurnitureUnitId };
                    foreach (var furnitureComponent in orderedFurnitureUnit.FurnitureUnit.Components)
                    {
                        var newConcreteFurnitureComponent = new Factory.Domain.Model.ConcreteFurnitureComponent(newConcreteFurnitureUnit.Id, furnitureComponent.Id);
                        newConcreteFurnitureUnit.AddCFC(newConcreteFurnitureComponent);
                    }
                    newOrder.AddCFU(newConcreteFurnitureUnit);
                }
                var orderState = await factoryOrderStateRepository.SingleAsync(ent => ent.State == Factory.Domain.Enums.OrderStateEnum.WaitingForFirstPayment);
                newOrder.AddTicket(new Factory.Domain.Model.Ticket(orderState.Id, factoryAdmins.First().Id, Clock.Now.AddDays(5)));
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeOrderToSalesAsync()
        {
            var salesOrders = await salesOrderRepository.GetAllListIncludingAsync(ent => true, ent => ent.CurrentTicket.OrderState, ent => ent.Tickets);
            var factoryOrders = await factoryOrderRepository.GetAllListIncludingAsync(ent => true, ent => ent.CurrentTicket.OrderState);
            foreach (var order in factoryOrders)
            {
                var salesOrder = salesOrders.SingleOrDefault(ent => ent.Id == order.Id);
                if (salesOrder == null)
                {
                    continue;
                }
                if (order.CurrentTicket.OrderState.State == Factory.Domain.Enums.OrderStateEnum.SecondPaymentConfirmed)
                {
                    var orderState = await salesOrderStateRepository.SingleAsync(ent => ent.State == Sales.Domain.Enums.OrderStateEnum.WaitingForShippingAppointmentReservation);
                    salesOrder.AddTicket(new Sales.Domain.Model.Ticket(orderState.Id, salesOrder.SalesPersonId, 5));
                }
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeImageToFactoryAsync()
        {
            var client = clientFactory.CreateClient();
            var factoryImages = await factoryImageRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var salesImages = await salesImageRepository.GetAllListIncludingAsync(ent => !factoryImages.Contains(ent.Id));
            foreach (var image in salesImages)
            {
                var request = $"{config.FactoryURL}/api/images/synchronize?containerName={image.ContainerName}&fileName={Uri.EscapeUriString(image.FileName)}";
                var response = await client.GetAsync(request);
                var (containerNameToReturn, fileNameToReturn) = JsonConvert.DeserializeObject<(string, string)>(await response.Content.ReadAsStringAsync());
                var newImage = new Factory.Domain.Model.Image(fileNameToReturn, image.Extension, containerNameToReturn) { Id = image.Id, ThumbnailName = image.ThumbnailName };
                await factoryImageRepository.InsertAsync(newImage);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeImageToSalesAsync()
        {
            var client = clientFactory.CreateClient();
            var salesImages = await salesImageRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var factoryImages = await salesImageRepository.GetAllListIncludingAsync(ent => !salesImages.Contains(ent.Id));
            foreach (var image in factoryImages)
            {
                var request = $"{config.SalesURL}/api/images/synchronize?containerName={image.ContainerName}&fileName={image.FileName}";
                var response = await client.GetAsync(request);
                var (containerNameToReturn, fileNameToReturn) = JsonConvert.DeserializeObject<(string, string)>(await response.Content.ReadAsStringAsync());
                var newImage = new Sales.Domain.Model.Image(fileNameToReturn, image.Extension, containerNameToReturn) { Id = image.Id, ThumbnailName = image.ThumbnailName };
                await salesImageRepository.InsertAsync(newImage);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeDocumentToFactoryAsync()
        {
            var client = clientFactory.CreateClient();
            var factoryDocuments = await factoryDocumentRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var salesDocuments = await salesDocumentRepository.GetAllListIncludingAsync(ent => (!factoryDocuments.Contains(ent.Id) && !(ent is Sales.Domain.Model.Image)));
            foreach (var document in salesDocuments)
            {
                var request = $"{config.FactoryURL}/api/documents/synchronize?id={document.Id}&containerName={document.ContainerName}&fileName={document.FileName}";
                var response = await client.GetAsync(request);
                var (containerNameToReturn, fileNameToReturn) = JsonConvert.DeserializeObject<(string, string)>(await response.Content.ReadAsStringAsync());
                var newDocument = new Factory.Domain.Model.Document(fileNameToReturn, document.Extension, containerNameToReturn) { Id = document.Id };
                await factoryDocumentRepository.InsertAsync(newDocument);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeDocumentToSalesAsync()
        {
            var client = clientFactory.CreateClient();
            var salesDocuments = await salesDocumentRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var factoryDocuments = await factoryDocumentRepository.GetAllListIncludingAsync(ent => (!salesDocuments.Contains(ent.Id) && !(ent is Factory.Domain.Model.Image)));
            foreach (var document in factoryDocuments)
            {
                var request = $"{config.SalesURL}/api/documents/synchronize?id={document.Id}&containerName={document.ContainerName}&fileName={document.FileName}";
                var response = await client.GetAsync(request);
                var (containerNameToReturn, fileNameToReturn) = JsonConvert.DeserializeObject<(string, string)>(await response.Content.ReadAsStringAsync());
                var newDocument = new Sales.Domain.Model.Document(fileNameToReturn, document.Extension, containerNameToReturn) { Id = document.Id };
                await salesDocumentRepository.InsertAsync(newDocument);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeGroupingCategoryToFactoryAsync()
        {
            var factoryCategories = await factoryGroupingCategoryRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var salesCategories = await salesGroupingCategoryRepository.GetAllListIncludingAsync(ent => !factoryCategories.Contains(ent.Id), ent => ent.Translations);
            foreach (var category in salesCategories)
            {
                var newCategory = new Factory.Domain.Model.GroupingCategory((Factory.Domain.Enums.GroupingCategoryEnum)category.CategoryType)
                {
                    Id = category.Id,
                    ParentGroupId = category.ParentGroupId
                };
                foreach (var translation in category.Translations)
                {
                    var factoryTranslation = new Factory.Domain.Model.GroupingCategoryTranslation(translation.CoreId, translation.GroupingCategoryName, translation.Language);
                    newCategory.AddTranslation(factoryTranslation);
                }

                await factoryGroupingCategoryRepository.InsertAsync(newCategory);
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SyncronizeWebshopOrderToFactoryAsync()
        {
            var factoryOrders = await factoryOrderRepository.GetAllListAsync(ent => true, ent => ent.Id);
            var salesOrders = await salesWebshopOrderRepository.GetOrderWithOrderedFurnitureUnits(ent => !factoryOrders.Contains(ent.Id));
            foreach (var order in salesOrders)
            {
                var newOrder = new Factory.Domain.Model.Order(order.OrderName, Clock.Now.AddDays(20))
                {
                    Id = order.Id,
                    WorkingNumber = order.WorkingNumber,
                    ShippingAddress = new Factory.Domain.Model.Address(order.ShippingAddress.PostCode, order.ShippingAddress.City, order.ShippingAddress.AddressValue, order.ShippingAddress.CountryId.GetValueOrDefault()),
                    CompanyId = 1, //TODO find own company
                    FirstPayment = new Factory.Domain.Model.OrderPrice(new Factory.Domain.Model.Price(order.Basket.SubTotal.Value / 2, order.Basket.SubTotal.CurrencyId), Clock.Now),
                    SecondPayment = new Factory.Domain.Model.OrderPrice(new Factory.Domain.Model.Price(order.Basket.SubTotal.Value / 2, order.Basket.SubTotal.CurrencyId), Clock.Now),
                };
                await factoryOrderRepository.InsertAsync(newOrder);
                foreach (var orderedFurnitureUnit in order.OrderedFurnitureUnits)
                {
                    var newConcreteFurnitureUnit = new Factory.Domain.Model.ConcreteFurnitureUnit(newOrder) { FurnitureUnitId = orderedFurnitureUnit.FurnitureUnitId };
                    foreach (var furnitureComponent in orderedFurnitureUnit.FurnitureUnit.Components)
                    {
                        var newConcreteFurnitureComponent = new Factory.Domain.Model.ConcreteFurnitureComponent(newConcreteFurnitureUnit.Id, furnitureComponent.Id);
                        newConcreteFurnitureUnit.AddCFC(newConcreteFurnitureComponent);
                    }
                    newOrder.AddCFU(newConcreteFurnitureUnit);
                }
                var orderState = await factoryOrderStateRepository.SingleAsync(ent => ent.State == Factory.Domain.Enums.OrderStateEnum.UnderMaterialReservation);
                newOrder.AddTicket(new Factory.Domain.Model.Ticket(orderState.Id, 1, Clock.Now.AddDays(5)));
            }
            await unitOfWork.SaveChangesAsync();
        }

        private async Task CreateStockedMaterialIfNotExist(Guid materialId)
        {
            var stockedMaterial = await stockedMaterialRepository.SingleOrDefaultAsync(ent => ent.MaterialId == materialId);
            if (stockedMaterial == null)
            {
                var newStockedMaterial = new Factory.Domain.Model.StockedMaterial(materialId, 0, 20, 50); // TODO min és req értékek nem jönnek sehonnan
                await stockedMaterialRepository.InsertAsync(newStockedMaterial);
            }
        }
    }
}
