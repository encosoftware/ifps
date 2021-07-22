using FluentAssertions;
using IFPS.Factory.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class OrderStateTypeTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public OrderStateTypeTests(IFPSFactoryWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        [Fact]
        public async Task Get_order_status_types_for_dropdown_works()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrderStateTypeAppService>();
                var orderStatusTypes = await service.GetOrderStateTypeDropdownListAsync();

                // Assert
                orderStatusTypes.Should().HaveCountGreaterOrEqualTo(11);
            }
        }
    }
}
