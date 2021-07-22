using FluentAssertions;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IFPS.Sales.FunctionalTests
{
    public class HealthCheckTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly ITestOutputHelper output;


        public HealthCheckTests(IFPSSalesWebApplicationFactory factory, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
        }

        [Fact]
        public async Task Healthcheck_works_and_service_looks_healthy()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            var resp = await client.GetAsync("health");

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            stringresp.Should().Be("Healthy");
        }

        [Fact]
        public async Task Culture_check()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            var resp = await client.GetAsync("api/testdebug/culture");

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            output.WriteLine("Current Culture: " + stringresp);
            output.WriteLine("Test Current Culture: " + CultureInfo.CurrentCulture.Name);

            Assert.Equal("hu-HU", stringresp);
            Assert.Equal("hu-HU", CultureInfo.CurrentCulture.Name);
        }
    }
}