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
    public class UserTeamTypeTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public UserTeamTypeTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_userteam_types_should_work()
        {
            // Arrange
            var client = factory.CreateClient();

            var expectedResult = BuildUserTeamTypeLists();

            // Act
            var resp = await client.GetAsync("api/userteamtypes");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserTeamTypeAppService>();
                var types = await service.GetUserTeamTypesAsync();
                // Assert
                expectedResult.Should().BeEquivalentTo(types.Where(ent => ent.Id >= 10000));
                resp.EnsureSuccessStatusCode();
            }
        }

        private List<UserTeamTypeListDto> BuildUserTeamTypeLists()
        {
            return new List<UserTeamTypeListDto>()
            {
                new UserTeamTypeListDto(BuildShippingUserTeam()),
                new UserTeamTypeListDto(BuildInstallationUserTeam()),
                new UserTeamTypeListDto(BuildSurveyUserTeam())
            };
        }

        private UserTeamType BuildShippingUserTeam()
        {
            var userTeamType = new UserTeamType(UserTeamTypeEnum.ShippingGroup) { Id = 10000 };
            userTeamType.AddTranslation(new UserTeamTypeTranslation("Shippers", userTeamType.Id, LanguageTypeEnum.EN) { Id = 10000 });
            userTeamType.AddTranslation(new UserTeamTypeTranslation("Kiszállítók", userTeamType.Id, LanguageTypeEnum.HU) { Id = 10001 });
            return userTeamType;
        }

        private UserTeamType BuildInstallationUserTeam()
        {
            var userTeamType = new UserTeamType(UserTeamTypeEnum.InstallationGroup) { Id = 10001 };
            userTeamType.AddTranslation(new UserTeamTypeTranslation("Installation group", userTeamType.Id, LanguageTypeEnum.EN) { Id = 10002 });
            userTeamType.AddTranslation(new UserTeamTypeTranslation("Beszerelőcsapat", userTeamType.Id, LanguageTypeEnum.HU) { Id = 10003 });
            return userTeamType;
        }

        private UserTeamType BuildSurveyUserTeam()
        {
            var userTeamType = new UserTeamType(UserTeamTypeEnum.SurveyGroup) { Id = 10002 };
            userTeamType.AddTranslation(new UserTeamTypeTranslation("Survey group", userTeamType.Id, LanguageTypeEnum.EN) { Id = 10004 });
            userTeamType.AddTranslation(new UserTeamTypeTranslation("Helyszíni felmérőcsapat", userTeamType.Id, LanguageTypeEnum.HU) { Id = 10005 });
            return userTeamType;
        }
    }
}
