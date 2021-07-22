using FluentAssertions;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class CameraTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;

        public CameraTests(IFPSFactoryWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Theory]
        [InlineData("192.168.0.1", 10000)]
        [InlineData("192.150.0.10", 10001)]
        public async Task Set_ProductionProcess_StartTime_Works(string ipAddress, int cfcId)
        {
            //Arrange
            var client = factory.CreateClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSFactoryContext>();

                var planAppService = scope.ServiceProvider.GetRequiredService<IPlanAppService>();
                
                //Act
                await planAppService.SetPlanProductionProcessTimeAsync(ipAddress, cfcId);


                var modifiedProductionProcessTime = context.Plans
                    .AsNoTracking()
                    .Include(ent => ent.WorkStation.WorkStationType)
                    .Include(ent => ent.ProductionProcess)
                    .Single(ent => ent.ConcreteFurnitureComponentId == cfcId &&
                    ent.WorkStation.WorkStationType.StationType == Domain.Enums.WorkStationTypeEnum.Cnc);

                //Assert
                modifiedProductionProcessTime.ProductionProcess.StartTime.Should().NotBeNull();
            }
        }

        [Theory]
        [InlineData("192.168.10.1", 10000)]
        [InlineData("192.155.0.1", 10001)]
        public async Task Set_ProductionProcess_EndTime_Works(string ipAddress, int cfcId)
        {
            //Arrange
            var client = factory.CreateClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSFactoryContext>();

                var planProcess = scope.ServiceProvider.GetRequiredService<IPlanAppService>();

                //Act
                await planProcess.SetPlanProductionProcessTimeAsync(ipAddress, cfcId);

                var modifiedPlanProcessTime = context.Plans
                    .Include(ent => ent.WorkStation.WorkStationType)
                    .Include(ent => ent.ProductionProcess)
                    .Single(ent => ent.ConcreteFurnitureComponentId == cfcId
                    && ent.WorkStation.WorkStationType.StationType == Domain.Enums.WorkStationTypeEnum.Cnc);

                //Assert
                modifiedPlanProcessTime.ProductionProcess.EndTime.Should().NotBeNull();
            }
        }
    }
}
