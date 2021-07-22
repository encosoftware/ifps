using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IFPS.Factory.FunctionalTests
{
    public class HealthCheckTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly ITestOutputHelper output;


        public HealthCheckTests(IFPSFactoryWebApplicationFactory factory, ITestOutputHelper output)
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
            output.WriteLine("Test Current Culture: " + CultureInfo.CurrentCulture.Name);
            Assert.Equal("hu-HU", CultureInfo.CurrentCulture.Name);
        }
    }
}
