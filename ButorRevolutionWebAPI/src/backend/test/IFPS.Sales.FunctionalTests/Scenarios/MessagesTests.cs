using FluentAssertions;
using IFPS.Sales.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class MessagesTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public MessagesTests(IFPSSalesWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        [Fact]
        public async Task Get_unanswered_messages_works()
        {
            // Arrange
            factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IMessageAppService>();
                var tickets = await service.GetUnansweredMessageListByUserAsync(10000);

                // Assert
                tickets[0].MessageCount.Should().Be(5);
                tickets[1].MessageCount.Should().Be(1);
            }
        }
    }
}