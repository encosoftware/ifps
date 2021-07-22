using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Paging;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class OrderSchedulingTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public OrderSchedulingTests(IFPSFactoryWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        [Fact]
        public async Task Get_order_scheduling_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<OrderSchedulingListDto>();

            var firstOrder = new OrderSchedulingListDto()
            {
                OrderId = new Guid("9aa53060-4b7a-4f82-8784-2bcc6313fbd3"),
                Completion = 0,
                CurrentStatus = new OrderStateListDto() { Id = 10007, State = OrderStateEnum.UnderProduction, Translation = "Várakozás gyártásra" },
                Deadline = new DateTime(2019, 8, 25),
                EstimatedProcessTime = 60000,
                OrderName = "Test Bestellung-1203",
                WorkingNumber = "MZ/X-503",
                IsEnough = true
            };

            expectedResult.Add(firstOrder);

            var secondOrder = new OrderSchedulingListDto()
            {
                OrderId = new Guid("5bab1970-41de-428f-876f-ac220bd0e7b1"),
                Completion = 42,
                CurrentStatus = new OrderStateListDto() { Id = 10007, State = OrderStateEnum.UnderProduction, Translation = "Várakozás gyártásra" },
                Deadline = new DateTime(2019, 8, 25),
                EstimatedProcessTime = 42000,
                OrderName = "Test Bestellung-9357",
                WorkingNumber = "MZ/X-880",
                IsEnough = true
            };

            expectedResult.Add(secondOrder);

            var thirdOrder = new OrderSchedulingListDto()
            {
                OrderId = new Guid("b8dd50e9-155d-449a-a45f-d027a2d43eba"),
                Completion = 100,
                CurrentStatus = new OrderStateListDto() { Id = 10007, State = OrderStateEnum.UnderProduction, Translation = "Várakozás gyártásra" },
                Deadline = new DateTime(2019, 8, 25),
                EstimatedProcessTime = 120000,
                OrderName = "Test Bestellung-3990",
                WorkingNumber = "MZ/X-007",
                IsEnough = true
            };

            expectedResult.Add(thirdOrder);

            IPagedList<OrderSchedulingListDto> pagedList = new PagedList<OrderSchedulingListDto>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 3
            };
            var result = pagedList.ToPagedList();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrderSchedulingAppService>();
                var orderSchedulingFilterDto = new OrderSchedulingFilterDto() { OrderName = "Bestellung" };
                var orders = await service.OrderSchedulingListAsync(orderSchedulingFilterDto);

                // Assert
                orders.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_order_scheduling_filter_order_name_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrderSchedulingAppService>();
                var orderSchedulingFilterDto = new OrderSchedulingFilterDto() { OrderName = "Wrong Order Name" };
                var orders = await service.OrderSchedulingListAsync(orderSchedulingFilterDto);

                // Assert
                orders.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_order_scheduling_filter_working_number_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrderSchedulingAppService>();
                var orderSchedulingFilterDto = new OrderSchedulingFilterDto() { WorkingNumber = "Wrong Working Number" };
                var orders = await service.OrderSchedulingListAsync(orderSchedulingFilterDto);

                // Assert
                orders.Data.Count().Should().Be(0);
            }
        }
    }
}
