using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Dbos;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.EF.Repositories
{
    public class OrderRepository : EFCoreRepositoryBase<IFPSSalesContext, Order, Guid>, IOrderRepository
    {
        public OrderRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<Order, object>>> DefaultIncludes => new List<Expression<Func<Order, object>>>
        {
        };
        public async Task<List<Order>> GetOrdersByCustomer(int customerId)
        {
            return await GetAll()
                .Include(ent => ent.Tickets)
                    .ThenInclude(ent => ent.OrderState)
                        .ThenInclude(ent => ent.Translations)
                .Include(ent => ent.Customer)
                    .ThenInclude(customer => customer.User)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Where(ent => ent.Customer.Id == customerId)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrders()
        {
            return await GetAll()
                .Include(ent => ent.Tickets)
                    .ThenInclude(ent => ent.OrderState)
                        .ThenInclude(ent => ent.Translations)
                .Include(ent => ent.Customer)
                    .ThenInclude(customer => customer.User)
                        .ThenInclude(ent => ent.CurrentVersion)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await GetAll()
                .Include(ent => ent.CurrentTicket)
                    .ThenInclude(ent => ent.AssignedTo)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.CurrentTicket)
                    .ThenInclude(ent => ent.OrderState)
                        .ThenInclude(ent => ent.Translations)
                .Include(ent => ent.SalesPerson)
                    .ThenInclude(salesPerson => salesPerson.User)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.Customer)
                    .ThenInclude(customer => customer.User)
                        .ThenInclude(ent => ent.CurrentVersion)
                .SingleOrDefaultAsync(ent => ent.Id == id)

                ?? throw new EntityNotFoundException(typeof(Order), id);
        }

        public async Task<Order> GetOrderForMessagesAsync(Guid id)
        {
            return await GetAll()
                .Include(ent => ent.SalesPerson)
                    .ThenInclude(ent => ent.User)
                        .ThenInclude(ent => ent.Image)
                .Include(ent => ent.SalesPerson)
                    .ThenInclude(ent => ent.User)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.Customer)
                    .ThenInclude(customer => customer.User)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.Customer)
                    .ThenInclude(ent => ent.User)
                        .ThenInclude(ent => ent.Image)
                .SingleOrDefaultAsync(ent => ent.Id == id)

                ?? throw new EntityNotFoundException(typeof(Order), id);
        }

        public async Task<Order> GetOrderWithTicketsAsync(Guid id)
        {
            return await GetAll()
                .Include(ent => ent.Tickets)
                    .ThenInclude(ent => ent.OrderState)
                        .ThenInclude(ent => ent.Translations)
                .Include(ent => ent.Tickets)
                    .ThenInclude(ent => ent.AssignedTo)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.CurrentTicket)
                        .ThenInclude(ent => ent.AssignedTo)
                            .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.CurrentTicket)
                        .ThenInclude(ent => ent.OrderState)
                            .ThenInclude(ent => ent.Translations)

                .SingleOrDefaultAsync(ent => ent.Id == id)

                ?? throw new EntityNotFoundException(typeof(Order), id);
        }

        public async Task<Order> GetCustomerOrderWithTicketsAsync(Guid id, int customerId)
        {
            return await GetAll()
                .Include(ent => ent.Tickets)
                    .ThenInclude(ent => ent.AssignedTo)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.Tickets)
                    .ThenInclude(ent => ent.OrderState)
                        .ThenInclude(ent => ent.Translations)
                .Include(ent => ent.CurrentTicket)
                        .ThenInclude(ent => ent.AssignedTo)
                            .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.CurrentTicket)
                        .ThenInclude(ent => ent.OrderState)
                            .ThenInclude(ent => ent.Translations)
                .SingleOrDefaultAsync(ent => ent.Id == id && ent.Customer.UserId == customerId)

                ?? throw new EntityNotFoundException(typeof(Order), id);
        }

        public async Task<int> GetNextWorkingNumber(int year)
        {
            var max = await context.Orders
                .IgnoreQueryFilters()
                .Where(ent => ent.WorkingNumberYear == year)
                .Select(ent => ent.WorkingNumberSerial)
                .DefaultIfEmpty(0)
                .MaxAsync();

            return max + 1;
        }

        public Task<IPagedList<Order>> GetOrdersByCompany(int companyId, Expression<Func<Order, bool>> predicate = null, Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null, int pageIndex = 0, int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.CurrentTicket)
                    .ThenInclude(ent => ent.OrderState)
                        .ThenInclude(ent => ent.Translations)
                .Include(ent => ent.Customer)
                    .ThenInclude(ent => ent.User)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.SalesPerson)
                    .ThenInclude(ent => ent.User)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Where(ent => ent.SalesPerson.User.CompanyId == companyId);

            return GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }

        public Task<IPagedList<Order>> GetOrders
           (Expression<Func<Order, bool>> predicate = null,
           Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
           int pageIndex = 0,
           int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.CurrentTicket)
                    .ThenInclude(ent => ent.AssignedTo)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.CurrentTicket.OrderState.Translations)
                .Include(ent => ent.FirstPayment)
                    .ThenInclude(ent => ent.Price)
                        .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.SecondPayment)
                    .ThenInclude(ent => ent.Price)
                        .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.SalesPerson)
                    .ThenInclude(ent => ent.User)
                        .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.Customer)
                    .ThenInclude(ent => ent.User)
                        .ThenInclude(ent => ent.CurrentVersion);

            return GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }

        // TODO include refactor
        public async Task<Order> GetOrderWithIncludesById(Guid orderId)
        {
            return await GetAll()
                .Include(ent => ent.Budget)
                    .ThenInclude(budget => budget.Currency)
                .Include(ent => ent.SalesPerson)
                    .ThenInclude(salesPerson => salesPerson.User)
                        .ThenInclude(user => user.Company)
                            .ThenInclude(company => company.CurrentVersion)
                .Include(ent => ent.SalesPerson)
                    .ThenInclude(salesPerson => salesPerson.User)
                        .ThenInclude(user => user.CurrentVersion)
                .Include(ent => ent.Customer)
                    .ThenInclude(customer => customer.User)
                        .ThenInclude(user => user.CurrentVersion)
                .Include(ent => ent.Customer)
                    .ThenInclude(customer => customer.User)
                        .ThenInclude(user => user.Company)
                            .ThenInclude(company => company.CurrentVersion)
                                .ThenInclude(current => current.ContactPerson)
                                    .ThenInclude(contact => contact.CurrentVersion)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ent => ent.FurnitureUnit)
                        .ThenInclude(ent => ent.Accessories)
                            .ThenInclude(ent => ent.Accessory)
                                .ThenInclude(accessory => accessory.CurrentPrice)
                                    .ThenInclude(price => price.Price)
                                        .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ent => ent.FurnitureUnit)
                        .ThenInclude(ent => ent.Accessories)
                            .ThenInclude(ent => ent.Accessory)
                                .ThenInclude(ent => ent.Image)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ent => ent.FurnitureUnit)
                        .ThenInclude(ent => ent.CurrentPrice)
                            .ThenInclude(current => current.Price)
                                .ThenInclude(price => price.Currency)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ent => ent.FurnitureUnit)
                        .ThenInclude(ent => ent.Image)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ent => ent.FurnitureUnit)
                        .ThenInclude(fu => fu.FurnitureUnitType)
                .Include(ent => ent.OrderedApplianceMaterials)
                    .ThenInclude(appliance => appliance.ApplianceMaterial)
                        .ThenInclude(material => material.SellPrice)
                            .ThenInclude(price => price.Currency)
                .Include(ent => ent.OrderedApplianceMaterials)
                    .ThenInclude(appliance => appliance.ApplianceMaterial)
                        .ThenInclude(material => material.Image)
                .Include(ent => ent.Services)
                    .ThenInclude(orderdService => orderdService.Service)
                        .ThenInclude(service => service.ServiceType)
                .Include(ent => ent.Services)
                    .ThenInclude(orderdService => orderdService.Service)
                        .ThenInclude(service => service.CurrentPrice)
                            .ThenInclude(current => current.Price)
                                .ThenInclude(price => price.Currency)
                .Include(ent => ent.OfferInformation)
                    .ThenInclude(ent => ent.ProductsPrice)
                        .ThenInclude(ent => ent.Currency)
                        .Include(ent => ent.OfferInformation)
                    .ThenInclude(ent => ent.ServicesPrice)
                        .ThenInclude(ent => ent.Currency)
                .SingleAsync(ent => ent.Id == orderId);
        }

        public async Task<Order> GetOrderForCreateFurnitureUnitById(Guid orderId)
        {
            return await GetAll()
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ofu => ofu.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Components)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ofu => ofu.FurnitureUnit)
                        .ThenInclude(fu => fu.FurnitureUnitType)
                .SingleAsync(ent => ent.Id == orderId);
        }

        public async Task<List<Order>> GetOrderWithOrderedFurnitureUnits(Expression<Func<Order, bool>> predicate = null)
        {
            return await GetAll()
                .Where(predicate)
                .Include(ent => ent.CurrentTicket)
                .Include(ent => ent.SalesPerson.User)
                .Include(ent => ent.FirstPayment.Price)
                .Include(ent => ent.SecondPayment.Price)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ofu => ofu.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Components)
                        .ToListAsync();
        }

        // TODO include refactor
        public async Task<Order> GetOrderWithIncludesForListById(Guid orderId)
        {
            return await GetAll()
                .Include(ent => ent.BaseCabinet)
                .Include(ent => ent.TopCabinet)
                .Include(ent => ent.TallCabinet)
                .Include(ent => ent.Budget)
                    .ThenInclude(budget => budget.Currency)
                .Include(ent => ent.Customer)
                    .ThenInclude(customer => customer.User)
                        .ThenInclude(user => user.Company)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.CurrentPrice)
                            .ThenInclude(current => current.Price)
                                .ThenInclude(price => price.Currency)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Accessories)
                            .ThenInclude(accessory => accessory.Accessory)
                                .ThenInclude(acc => acc.CurrentPrice)
                                    .ThenInclude(current => current.Price)
                                        .ThenInclude(price => price.Currency)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Accessories)
                            .ThenInclude(accessory => accessory.Accessory)
                                .ThenInclude(acc => acc.Image)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Image)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ent => ent.FurnitureUnit)
                        .ThenInclude(ent => ent.Components)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.FurnitureUnitType)
                .Include(ent => ent.OrderedApplianceMaterials)
                    .ThenInclude(appliance => appliance.ApplianceMaterial)
                        .ThenInclude(mat => mat.SellPrice)
                            .ThenInclude(price => price.Currency)
                .Include(ent => ent.OrderedApplianceMaterials)
                    .ThenInclude(appliance => appliance.ApplianceMaterial)
                        .ThenInclude(mat => mat.Image)
                .Include(ent => ent.Services)
                    .ThenInclude(orderdService => orderdService.Service)
                        .ThenInclude(service => service.ServiceType)
                .Include(ent => ent.Services)
                    .ThenInclude(orderdService => orderdService.Service)
                        .ThenInclude(service => service.CurrentPrice)
                            .ThenInclude(current => current.Price)
                                .ThenInclude(price => price.Currency)
                .Include(ent => ent.OfferInformation)
                .SingleAsync(ent => ent.Id == orderId);
        }

        // TODO include refactor
        public async Task<Order> GetOrderWithIncludesForOrderedFurnitureUnitDetailsById(Guid orderId)
        {
            return await GetAll()
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.CurrentPrice)
                            .ThenInclude(current => current.Price)
                                .ThenInclude(price => price.Currency)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Image)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Components)
                            .ThenInclude(component => component.Image)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Components)
                            .ThenInclude(component => component.TopFoil)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Components)
                            .ThenInclude(component => component.BottomFoil)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Components)
                            .ThenInclude(component => component.RightFoil)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Components)
                            .ThenInclude(component => component.LeftFoil)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Components)
                            .ThenInclude(component => component.BoardMaterial)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(orderedFurnitureUnit => orderedFurnitureUnit.FurnitureUnit)
                        .ThenInclude(furnitureUnit => furnitureUnit.Category)
                            .ThenInclude(category => category.Translations)
                .SingleOrDefaultAsync(ent => ent.Id == orderId)

                ?? throw new EntityNotFoundException(typeof(Order), orderId);
        }

        public async Task<List<TResult>> GetDocumentGroupsAsync<TResult>(Guid orderId, Expression<Func<DocumentGroup, TResult>> selector)
        {
            if (!await context.Orders.AnyAsync(x => x.Id == orderId))
                throw new EntityNotFoundException(typeof(Order), orderId);

            return await context.DocumentGroups
                .Where(x => x.OrderId == orderId)
                .Select(selector)
                .ToListAsync();
        }

        public async Task<DocumentGroup> GetDocumentGroupWithFolderAsync(Guid orderId, int documentGroupId)
        {
            return await context.DocumentGroups
                    .Include(ent => ent.DocumentFolder)
                        .ThenInclude(ent => ent.DocumentTypes)
                    .Include(ent => ent.Order)
                    .Include(ent => ent.Versions)
                .SingleOrDefaultAsync(ent => ent.Id == documentGroupId && ent.OrderId == orderId)
                ?? throw new EntityNotFoundException(typeof(DocumentGroup), documentGroupId);
        }
        public async Task<DocumentGroupVersion> GetDocumentGroupVersionWithIncludesAsync(Guid orderId, int documentGroupVersionId)
        {
            return await context.DocumentGroupVersions
                .Include(ent => ent.Core)
                    .ThenInclude(ent => ent.DocumentFolder)
                        .ThenInclude(ent => ent.DocumentTypes)
                .Include(ent => ent.Core)
                    .ThenInclude(ent => ent.Order)
                .Include(ent => ent.State)
                .SingleOrDefaultAsync(ent => ent.Id == documentGroupVersionId && ent.Core.OrderId == orderId)
                ?? throw new EntityNotFoundException(typeof(DocumentGroupVersion), documentGroupVersionId);
        }
        public async Task<Order> GetOrderForContractById(Guid orderId)
        {
            return await GetAll()
                .Include(ent => ent.SalesPerson)
                    .ThenInclude(sales => sales.User)
                        .ThenInclude(user => user.Company)
                            .ThenInclude(company => company.CurrentVersion)
                .Include(ent => ent.Customer)
                    .ThenInclude(customer => customer.User)
                        .ThenInclude(user => user.CurrentVersion)
                .Include(ent => ent.Customer)
                    .ThenInclude(customer => customer.User)
                        .ThenInclude(user => user.Company)
                            .ThenInclude(company => company.CurrentVersion)
                .Include(ent => ent.FirstPayment)
                    .ThenInclude(payment => payment.Price)
                        .ThenInclude(price => price.Currency)
                .Include(ent => ent.SecondPayment)
                    .ThenInclude(payment => payment.Price)
                        .ThenInclude(price => price.Currency)
                .Include(ent => ent.Budget)
                    .ThenInclude(budget => budget.Currency)
                .Include(ent => ent.OfferInformation)
                    .ThenInclude(ent => ent.ProductsPrice)
                        .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.OfferInformation)
                    .ThenInclude(ent => ent.ServicesPrice)
                .Include(ent => ent.OfferInformation)
                    .ThenInclude(ent => ent.ProductsPrice)
                        .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.Services)
                    .ThenInclude(ent => ent.Service)
                        .ThenInclude(service => service.ServiceType)
                .Include(ent => ent.Services)
                    .ThenInclude(orderdService => orderdService.Service)
                        .ThenInclude(service => service.CurrentPrice)
                            .ThenInclude(current => current.Price)
                                .ThenInclude(price => price.Currency)
                .Include(ent => ent.OrderedFurnitureUnits)
                .Include(ent => ent.OrderedApplianceMaterials)
                .SingleOrDefaultAsync(ent => ent.Id == orderId)

                ?? throw new EntityNotFoundException(typeof(Order), orderId);
        }

        public async Task<OrdersDbo> GetOrderByIdAsync(Expression<Func<Order, bool>> predicate, Expression<Func<Order, OrdersDbo>> selector)
        {
            return await GetAll().Where(predicate).Select(selector).SingleAsync();
        }

        public Task<Order> GetOrderWithOrderedAppliances(Guid id)
        {
            return GetAll()
                .Include(ent => ent.OrderedApplianceMaterials)
                    .ThenInclude(ent => ent.ApplianceMaterial)
                .Include(ent => ent.Services)
                    .ThenInclude(service => service.Service.ServiceType)
                .Include(ent => ent.Services)
                    .ThenInclude(service => service.Service.CurrentPrice.Price)
                .Include(ent => ent.OrderedFurnitureUnits)
                .Include(ent => ent.OfferInformation)
                .SingleOrDefaultAsync(ent => ent.Id == id) ??

                 throw new EntityNotFoundException(typeof(Order), id);
        }

        public async Task<Order> GetOrderByIdByAddService(Guid id)
        {
            return await GetAll()
                .Include(ent => ent.Services)
                    .ThenInclude(ent => ent.Service)
                        .ThenInclude(ent => ent.ServiceType)
                .Include(ent => ent.Services)
                    .ThenInclude(orderedService => orderedService.Service)
                        .ThenInclude(service => service.CurrentPrice.Price.Currency)
                .Include(ent => ent.OfferInformation)
                    .ThenInclude(info => info.ServicesPrice)
                .Include(ent => ent.OrderedApplianceMaterials)
                .Include(ent => ent.OrderedFurnitureUnits)
                    .ThenInclude(ent => ent.FurnitureUnit)
                        .ThenInclude(ent => ent.Components)
                .SingleAsync(ent => ent.Id == id);
        }

        public async Task<Order> GetOrderDeleteByIdAsync(Guid orderId)
        {
            return await GetAll()
                .Include(ent => ent.OrderedFurnitureUnits)
                .Include(ent => ent.OrderedApplianceMaterials)
                .Include(ent => ent.OfferInformation)
                .Include(ent => ent.Services)
                    .ThenInclude(orderedService => orderedService.Service)
                        .ThenInclude(service => service.ServiceType)
                .Include(ent => ent.Services)
                    .ThenInclude(orderedService => orderedService.Service)
                        .ThenInclude(service => service.CurrentPrice.Price)
                .SingleAsync(ent => ent.Id == orderId);
        }

        public async Task<bool> IsOrderWithSalesPersonExistingAsync(int salesPersonId)
        {
            return await GetAll()
                .AnyAsync(ent => ent.SalesPersonId == salesPersonId);
        }

        public async Task<bool> IsOrderWithCustomerExistingAsync(int customerId)
        {
            return await GetAll()
                .AnyAsync(ent => ent.CustomerId == customerId);
        }
    }
}
