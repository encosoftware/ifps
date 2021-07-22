using ENCO.DDD.Domain.Model.Enums;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class FurnitureUnitTypeTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public FurnitureUnitTypeTests(IFPSSalesWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            CultureInfo.CurrentCulture = new CultureInfo("hu-HU");
        }

        [Fact]
        public async Task Get_furniture_unit_types_should_work()
        {
            // Arrange
            var client = factory.CreateClient();

            var expectedResult = BuildFurnitureUnitTypeLists();

            // Act
            var resp = await client.GetAsync("api/furnitureunittypes");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFurnitureUnitTypeAppService>();
                var types = await service.GetFurnitureUnitTypesAsync();
                // Assert
                expectedResult.Should().BeEquivalentTo(types.Where(ent => ent.Id >= 10700 && ent.Id <= 10720));
                resp.EnsureSuccessStatusCode();
            }
        }

        private List<FurnitureUnitTypeListDto> BuildFurnitureUnitTypeLists()
        {
            return new List<FurnitureUnitTypeListDto>()
            {
                new FurnitureUnitTypeListDto(BuildBaseType()),
                new FurnitureUnitTypeListDto(BuildTopType()),
                new FurnitureUnitTypeListDto(BuildTallType())
            };
        }

        private FurnitureUnitType BuildBaseType()
        {
            var baseType = new FurnitureUnitType(FurnitureUnitTypeEnum.Base)
            {
                Id = 10720
            };
            baseType.AddTranslation(new FurnitureUnitTypeTranslation(baseType.Id, "Base", LanguageTypeEnum.EN));
            baseType.AddTranslation(new FurnitureUnitTypeTranslation(baseType.Id, "Alsó", LanguageTypeEnum.HU));
            return baseType;
        }

        private FurnitureUnitType BuildTallType()
        {
            var tallType = new FurnitureUnitType(FurnitureUnitTypeEnum.Tall)
            {
                Id = 10710
            };
            tallType.AddTranslation(new FurnitureUnitTypeTranslation(tallType.Id, "Tall", LanguageTypeEnum.EN));
            tallType.AddTranslation(new FurnitureUnitTypeTranslation(tallType.Id, "Magas", LanguageTypeEnum.HU));
            return tallType;
        }

        private FurnitureUnitType BuildTopType()
        {
            var topType = new FurnitureUnitType(FurnitureUnitTypeEnum.Top)
            {
                Id = 10700
            };
            topType.AddTranslation(new FurnitureUnitTypeTranslation(topType.Id, "Top", LanguageTypeEnum.EN));
            topType.AddTranslation(new FurnitureUnitTypeTranslation(topType.Id, "Felső", LanguageTypeEnum.HU));
            return topType;
        }
    }
}
