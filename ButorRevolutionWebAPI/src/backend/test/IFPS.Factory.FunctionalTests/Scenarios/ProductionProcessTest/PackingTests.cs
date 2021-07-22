using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Paging;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
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
    public class PackingTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public PackingTests(IFPSFactoryWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        [Fact]
        public async Task Get_packing_production_process_plans_should_work()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<PackingListDto>();

            var firstPacking = new PackingListDto(BuildProductionProcesses().First());
            expectedResult.Add(firstPacking);

            var secondPacking = new PackingListDto(BuildProductionProcesses().Last());
            expectedResult.Add(secondPacking);

            IPagedList<PackingListDto> pagedList = new PagedList<PackingListDto>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 2
            };
            var result = pagedList.ToPagedList();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPackingsAppService>();
                var packingFilterDto = new PackingFilterDto() { OrderName = "ProductionProcess Test Order" };
                var packings = await service.GetPackingsAsync(packingFilterDto);

                // Assert
                result.Should().BeEquivalentTo(packings);
            }
        }

        [Fact]
        public async Task Get_packing_production_process_plans_with_wrong_unit_name_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPackingsAppService>();
                var packingFilterDto = new PackingFilterDto() { UnitName = "Wrong Unit Name" };
                var packings = await service.GetPackingsAsync(packingFilterDto);

                // Assert
                packings.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_packing_production_process_plans_with_wrong_order_name_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPackingsAppService>();
                var packingFilterDto = new PackingFilterDto() { OrderName = "Wrooooong!" };
                var packings = await service.GetPackingsAsync(packingFilterDto);

                // Assert
                packings.Data.Count().Should().Be(0);
            }
        }

        #region Build methods

        private Order BuildOrder()
        {
            return new Order("ProductionProcess Test Order")
            {
                Id = new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"),
                WorkingNumber = "2020-MZ/X-77",
                CurrentTicketId = 10000,
                CompanyId = 10000
            };
        }

        private FurnitureUnit BuildFurnitureUnit()
        {
            return new FurnitureUnit("Reservation 02", 600, 200, 300)
            {
                Id = new Guid("f4f37e65-379c-4569-a720-83e4f9ec0e90"),
                CategoryId = 10000,
                ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"),
                BaseFurnitureUnitId = null,
                Description = "lorem ipsum 02",
                CurrentPriceId = 10001
            };
        }

        private ConcreteFurnitureUnit BuildConcreteFurnitureUnit()
        {
            return new ConcreteFurnitureUnit(BuildOrder().Id)
            {
                FurnitureUnitId = BuildFurnitureUnit().Id,
                FurnitureUnit = BuildFurnitureUnit()
            };
        }

        private List<Plan> BuildPlans()
        {
            return new List<Plan>()
            {
                new Plan()
                {
                    Id = 10011,
                    OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                    ScheduledStartTime = new DateTime(2020, 2, 4, 13, 0, 0),
                    ScheduledEndTime = new DateTime(2020, 2, 4, 17, 0, 0),
                    ConcreteFurnitureUnitId = BuildConcreteFurnitureUnit().Id,
                    ConcreteFurnitureUnit = BuildConcreteFurnitureUnit()
                },
                new Plan()
                {
                    Id = 10012,
                    OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                    ScheduledStartTime = new DateTime(2020, 3, 13, 13, 20, 0),
                    ScheduledEndTime = new DateTime(2020, 3, 13, 16, 45, 0),
                    ConcreteFurnitureUnitId = BuildConcreteFurnitureUnit().Id,
                    ConcreteFurnitureUnit = BuildConcreteFurnitureUnit()
                },
            };
        }

        private UserData BuildWorkerUserData()
        {
            return new UserData("Test Worker Name", "+367012345678", Clock.Now, null)
            {
                Id = 10012
            };
        }

        private User BuildWorkerUser()
        {
            return new User("processworker@enco.hu", LanguageTypeEnum.HU)
            {
                Id = 10014,
                CurrentVersionId = BuildWorkerUserData().Id,
                CurrentVersion = BuildWorkerUserData()
            };
        }

        private List<ProcessWorker> BuildProcessWorkers(int productionProcessId)
        {
            return new List<ProcessWorker>()
            {
                new ProcessWorker()
                {
                    Id = 10017,
                    ProcessId = productionProcessId,
                    WorkerId = BuildWorkerUser().Id,
                    Worker = BuildWorkerUser()
                },
                new ProcessWorker()
                {
                    Id = 10018,
                    ProcessId = productionProcessId,
                    WorkerId = BuildWorkerUser().Id,
                    Worker = BuildWorkerUser()
                }
            };
        }

        private List<ProductionProcess> BuildProductionProcesses()
        {
            var order = BuildOrder();
            var plans = BuildPlans();

            var productionProcesses = new List<ProductionProcess>()
            {
                new ProductionProcess(order.Id, plans.First().Id)
                {
                    Id = 10108,
                    Order = order,
                    Plan = plans.First()
                },
                new ProductionProcess(order.Id, plans.Last().Id)
                {
                    Id = 10109,
                    Order = order,
                    Plan = plans.Last()
                }
            };

            var workersForFirst = BuildProcessWorkers(productionProcesses.First().Id);
            workersForFirst.ForEach(ent => productionProcesses.First().AddWorkers(ent));

            var workersForLast = BuildProcessWorkers(productionProcesses.Last().Id);
            workersForLast.ForEach(ent => productionProcesses.Last().AddWorkers(ent));

            return productionProcesses;
        }

        #endregion
    }
}
