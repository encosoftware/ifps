using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class TicketsTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public TicketsTests(IFPSSalesWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        //[Fact]
        //public async Task Get_tickets_works()
        //{
        //    // Arrange
        //    factory.CreateClient();

        //    //var user = new User("enco@enco.hu") { Id = 10101 };
        //    //user.AddVersion(new UserData("Tommy Customer (seed)", "00000", Clock.Now)
        //    //{
        //    //    Id = 10100,
        //    //    ContactAddress = new Address(1117, "Budapest", "Bocskai út 77-79.", 1)
        //    //});
        //    //var customer = new Customer(user.Id, new DateTime(2019, 12, 31)) { Id = 10240, User = user };
        //    //var order = new Order("SALES_Order 002 (seed)", 10101, 10000, Clock.Now, Price.GetDefaultPrice()) { Id = new Guid("2418B030-A64B-4724-9702-964CF5EB04C6"), Customer = customer };
        //    //var orderState = new OrderState(OrderStateEnum.WaitingForContract) { Id = 3 };
        //    //orderState.AddTranslation(new OrderStateTranslation(3, "WaitingForContract", LanguageTypeEnum.EN) { Id = 3 });
        //    //order.AddTicket(new Ticket(orderState, customer.User, new DateTime(2020, 1, 1), order) { Id = 10000 });
        //    //var expectedResult = new TicketListDto(order);

        //    // Act
        //    using (var scope = factory.Server.Host.Services.CreateScope())
        //    {
        //        var service = scope.ServiceProvider.GetRequiredService<ITicketAppService>();
        //        var tickets = await service.GetTicketList();

        //        // Assert
        //        //tickets.Should().ContainEquivalentOf(expectedResult);
        //        tickets.Should().HaveCount(38);
        //    }
        //}

        [Fact]
        public async Task Get_own_tickets_works()
        {
            // Arrange
            factory.CreateClient();
            var expectedResult = new List<TicketListDto>();
            var user = new User("enco@enco.hu") { Id = 10101 };
            user.AddVersion(new UserData("enco", "00000", Clock.Now)
            {
                Id = 10100,
                ContactAddress = new Address(1117, "Budapest", "Bocskai út 77-79.", 1)
            });
            var customer = new Customer(user.Id, new DateTime(2019, 12, 31)) { Id = 10240, User = user };
            var order = new Order("Teszt order", 10101, 10000, Clock.Now, Price.GetDefaultPrice()) { Id = new Guid("fc2ffeb7-58fb-4dbe-ac8c-85918e940b01"), Customer = customer };
            var orderState = new OrderState(OrderStateEnum.WaitingForOffer) { Id = 3 };
            orderState.AddTranslation(new OrderStateTranslation(3, "WaitingForOffer", LanguageTypeEnum.EN) { Id = 3 });

            order.AddTicket(new Ticket(orderState, customer.User, 2, order) { Id = 10000 });
            expectedResult.Add(new TicketListDto(order));

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ITicketAppService>();
                var tickets = await service.GetOwnTicketList(10101);

                // Assert
                expectedResult.Should().ContainEquivalentOf(tickets[0]);
            }
        }

        [Fact]
        public async Task Get_tickets_by_orderId_works()
        {
            // Arrange
            factory.CreateClient();

            var user = new User("enco7@enco.hu") { Id = 10101 };
            user.AddVersion(new UserData("enco", "00000", Clock.Now)
            {
                Id = 10100,
                ContactAddress = new Address(1117, "Budapest", "Bocskai út 77-79.", 1)
            });

            var customer = new Customer(user.Id, new DateTime(2019, 12, 31)) { Id = 10240, User = user };
            var orderState = new OrderState(OrderStateEnum.WaitingForOffer) { Id = 10000 };
            var ticket = new Ticket(orderState, user, 2) { Id = 10000 };

            var order = new Order("Teszt order", user.Id, 10000, Clock.Now, new Price(0.0, 1))
            {
                Id = new Guid("FC2FFEB7-58FB-4DBE-AC8C-85918E940B01"),
                Customer = customer,
                CustomerId = customer.Id,
                WorkingNumberSerial = 11111,
                WorkingNumberYear = 3000,
                ShippingAddress = new Address(6344, "Hajós", "Petőfi utca 52.", 1),
            };

            orderState.AddTranslation(new OrderStateTranslation(coreId: 10000, "WaitingForOffer", LanguageTypeEnum.EN) { Id = 10000 });
            orderState.AddTranslation(new OrderStateTranslation(coreId: 10000, "Várakozás árajánlatra", LanguageTypeEnum.HU) { Id = 10001 });
            order.AddTicket(ticket);
            var expectedResult = new TicketByOrderListDto(order.CurrentTicket);

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ITicketAppService>();
                var tickets = await service.GetTicketsByOrderAsync(order.Id);

                // Assert
                // The next two lines: we must compare just the dates beacuse due to domain events there are some seconds delay
                tickets.First().Deadline = tickets.First().Deadline.Value.Date;
                expectedResult.Deadline = expectedResult.Deadline.Value.Date;

                tickets.Should().BeEquivalentTo(expectedResult);
                tickets.Should().ContainEquivalentOf(expectedResult);
            }
        }

        [Fact]
        public async Task Get_tickets_by_wrong_orderId_should_not_work()
        {
            // Arrange
            factory.CreateClient();

            var user = new User("enco7@enco.hu") { Id = 10101 };
            user.AddVersion(new UserData("enco", "00000", Clock.Now)
            {
                Id = 10100,
                ContactAddress = new Address(1117, "Budapest", "Bocskai út 77-79.", 1)
            });

            var customer = new Customer(user.Id, new DateTime(2019, 12, 31)) { Id = 10240, User = user };
            var orderState = new OrderState(OrderStateEnum.WaitingForOffer);
            var ticket = new Ticket(orderState, user, 2) { Id = 10000 };

            var order = new Order("Teszt order", user.Id, 10000, Clock.Now, new Price(0.0, 1))
            {
                Id = new Guid("FC2FFEB7-58FB-AAAA-EEEE-85918E940B01"),
                Customer = customer,
                CustomerId = customer.Id,
                WorkingNumberSerial = 11111,
                WorkingNumberYear = 3000,
                ShippingAddress = new Address(6344, "Hajós", "Petőfi utca 52.", 1),
            };

            orderState.AddTranslation(new OrderStateTranslation(coreId: 10000, "WaitingForOffer", LanguageTypeEnum.EN) { Id = 10000 });
            order.AddTicket(ticket);
            var expectedResult = new TicketByOrderListDto(order.CurrentTicket);

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ITicketAppService>();

                // Assert
                await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetTicketsByOrderAsync(order.Id));
            }
        }

        [Fact]
        public async Task Get_customerticket_by_orderId_and_customerId_works()
        {
            // Arrange
            factory.CreateClient();

            var user = new User("enco7@enco.hu") { Id = 10101 };
            user.AddVersion(new UserData("enco", "00000", Clock.Now)
            {
                Id = 10100,
                ContactAddress = new Address(1117, "Budapest", "Bocskai út 77-79.", 1)
            });

            var customer = new Customer(user.Id, new DateTime(2019, 12, 31)) { Id = 10240, User = user };
            var orderState = new OrderState(OrderStateEnum.WaitingForOffer) { Id = 10000 };
            var ticket = new Ticket(orderState, user, 2) { Id = 10000 };

            var order = new Order("Teszt order", user.Id, 10000, Clock.Now, new Price(0.0, 1))
            {
                Id = new Guid("FC2FFEB7-58FB-4DBE-AC8C-85918E940B01"),
                Customer = customer,
                CustomerId = customer.Id,
                WorkingNumberSerial = 11111,
                WorkingNumberYear = 3000,
                ShippingAddress = new Address(6344, "Hajós", "Petőfi utca 52.", 1),
            };

            orderState.AddTranslation(new OrderStateTranslation(coreId: 10000, "Waiting For Offer", LanguageTypeEnum.EN) { Id = 10000 });
            orderState.AddTranslation(new OrderStateTranslation(coreId: 10000, "Várakozás árajánlatra", LanguageTypeEnum.HU) { Id = 10001 });
            order.AddTicket(ticket);
            var expectedResult = new TicketByOrderListDto(order.CurrentTicket);

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ITicketAppService>();
                var tickets = await service.GetCustomerTicketsByOrderAsync(order.Id, customer.UserId);

                // Assert
                // The next two lines: we must compare just the dates beacuse due to domain events there are some seconds delay
                tickets.First().Deadline = tickets.First().Deadline.Value.Date;
                expectedResult.Deadline = expectedResult.Deadline.Value.Date;

                tickets.Should().BeEquivalentTo(expectedResult);
            }
        }

        [Fact]
        public async Task Get_customerticket_by_wrong_orderId_and_wrong_customerId_should_not_work()
        {
            // Arrange
            factory.CreateClient();

            var user = new User("enco7@enco.hu") { Id = 10999 };
            user.AddVersion(new UserData("enco", "00000", Clock.Now)
            {
                Id = 10100,
                ContactAddress = new Address(1117, "Budapest", "Bocskai út 77-79.", 1)
            });

            var customer = new Customer(user.Id, new DateTime(2019, 12, 31)) { Id = 10240, User = user };
            var orderState = new OrderState(OrderStateEnum.WaitingForOffer);
            var ticket = new Ticket(orderState, user, 2) { Id = 10000 };

            var order = new Order("Teszt order", user.Id, 10000, Clock.Now, new Price(0.0, 1))
            {
                Id = new Guid("FC2FFEB7-58FB-FFFF-EEEE-85918E940B01"),
                Customer = customer,
                CustomerId = customer.Id,
                WorkingNumberSerial = 11111,
                WorkingNumberYear = 3000,
                ShippingAddress = new Address(6344, "Hajós", "Petőfi utca 52.", 1),
            };

            orderState.AddTranslation(new OrderStateTranslation(coreId: 10000, "WaitingForOffer", LanguageTypeEnum.EN) { Id = 10000 });
            order.AddTicket(ticket);
            var expectedResult = new TicketByOrderListDto(order.CurrentTicket);

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ITicketAppService>();

                // Assert
                await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetCustomerTicketsByOrderAsync(order.Id, customer.UserId));
            }
        }
    }
}