using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Paging;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
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
    public class LayoutTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public LayoutTests(IFPSFactoryWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        [Fact]
        public async Task Get_layouts_by_production_process_should_work()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<CuttingsListDto>();

            var firstCutting = new CuttingsListDto(BuildProductionProcesses().First());
            expectedResult.Add(firstCutting);

            var secondCutting = new CuttingsListDto(BuildProductionProcesses().Last());
            expectedResult.Add(secondCutting);

            IPagedList<CuttingsListDto> pagedList = new PagedList<CuttingsListDto>()
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
                var service = scope.ServiceProvider.GetRequiredService<ICuttingsAppService>();
                var cuttingsFilterDto = new CuttingsFilterDto() { OrderName = "ProductionProcess Test Order" };
                var cuttings = await service.CuttingsListAsync(cuttingsFilterDto);

                // Assert
                result.Should().BeEquivalentTo(cuttings);
            }
        }

        [Fact]
        public async Task Get_layouts_by_production_process_filter_material_code_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICuttingsAppService>();
                var cuttingsFilterDto = new CuttingsFilterDto() { MaterialCode = "Wrong Material Code" };
                var cuttings = await service.CuttingsListAsync(cuttingsFilterDto);

                // Assert
                cuttings.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_layouts_by_production_process_filter_machine_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICuttingsAppService>();
                var cuttingsFilterDto = new CuttingsFilterDto() { Machine = "Wroooong!" };
                var cuttings = await service.CuttingsListAsync(cuttingsFilterDto);

                // Assert
                cuttings.Data.Count().Should().Be(0);
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
                Id = 10002,
                FurnitureUnitId = BuildFurnitureUnit().Id,
                FurnitureUnit = BuildFurnitureUnit()
            };
        }

        private FurnitureComponent BuildFurnitureComponent()
        {
            return new FurnitureComponent("Reservation test 01", 1)
            {
                Id = new Guid("4aa9bcaf-63e5-4cd6-aa6c-bf571fefb597"),
                Type = FurnitureComponentTypeEnum.Front,
                BoardMaterialId = new Guid("fb39bf09-e5e3-49c4-8e11-50782d5a5cad"),
                Width = 1000,
                Length = 600
            };
        }

        private ConcreteFurnitureComponent BuildConcreteFurnitureComponent()
        {
            return new ConcreteFurnitureComponent(BuildConcreteFurnitureUnit().Id, BuildFurnitureComponent().Id)
            {
                Id = 10003,
                QRCodeId = new Guid("6908447f-6357-49b0-8de1-5be18073e346")
            };
        }

        private Machine BuildMachine()
        {
            return new Machine("The Test strongest machine", 10004) { Id = 10001 };
        }

        private WorkStation BuildWorkStation()
        {
            return new WorkStation("Layout WS PP", 1, true, 10001)
            {
                Id = 10011,
                MachineId = BuildMachine().Id,
                Machine = BuildMachine()
            };
        }

        private BoardMaterial BuildDecorBoardMaterial()
        {
            return new DecorBoardMaterial("BL00712098")
            {
                Id = new Guid("fb39bf09-e5e3-49c4-8e11-50782d5a5cad"),
                CategoryId = 10000,
                Description = "Premium fenyő bútorlap",
                HasFiberDirection = false,
                ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"),
                SiUnitId = 10001,
                CurrentPriceId = 10000
            };
        }

        private List<LayoutPlan> BuildPlans()
        {
            return new List<LayoutPlan>()
            {
                new LayoutPlan()
                {
                    Id = 10015,
                    OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                    ScheduledStartTime = new DateTime(2020, 1, 31, 12, 0, 0),
                    ScheduledEndTime = new DateTime(2020, 1, 31, 18, 0, 0),
                    ConcreteFurnitureComponentId = BuildConcreteFurnitureComponent().Id,
                    WorkStationId = BuildWorkStation().Id,
                    WorkStation = BuildWorkStation(),
                    BoardId = BuildDecorBoardMaterial().Id,
                    Board = BuildDecorBoardMaterial()
                },
                new LayoutPlan()
                {
                    Id = 10016,
                    OptimizationId = new Guid("3908bed6-4469-4bdf-96b9-b477e9a96479"),
                    ScheduledStartTime = new DateTime(2020, 3, 10, 8, 0, 0),
                    ScheduledEndTime = new DateTime(2020, 3, 10, 15, 0, 0),
                    ConcreteFurnitureComponentId = BuildConcreteFurnitureComponent().Id,
                    WorkStationId = BuildWorkStation().Id,
                    WorkStation = BuildWorkStation(),
                    BoardId = BuildDecorBoardMaterial().Id,
                    Board = BuildDecorBoardMaterial()
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
                    Id = 10104,
                    Order = order,
                    Plan = plans.First()
                },
                new ProductionProcess(order.Id, plans.Last().Id)
                {
                    Id = 10105,
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
