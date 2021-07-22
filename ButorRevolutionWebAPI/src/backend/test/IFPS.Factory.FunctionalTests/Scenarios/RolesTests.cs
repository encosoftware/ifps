using ENCO.DDD.Domain.Model.Enums;
using FluentAssertions;
using IFPS.Factory.FunctionalTests;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class RolesTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;

        public RolesTests(IFPSFactoryWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        private async Task<string> getAccessToken()
        {
            var loginDto = new LoginDto()
            {
                Email = "enco@enco.hu",
                Password = "password",
                RememberMe = true
            };
            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
            var client = factory.CreateClient();
            var resp = await client.PostAsync("api/account/login/", content);
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TokenDto>(stringresp);
            return model.AccessToken;
        }

        [Fact]
        public async Task Get_role_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());
            client.DefaultRequestHeaders.Add("Accept-Language", "en-GB");
            var expectedRole = new Role("Admin", 10000) { Id = 10000 };
            expectedRole.AddDefaultRoleClaims(new DefaultRoleClaim(10000,10000));
            var expectedResult = new RoleDetailsDto(expectedRole);

            // Act
            var resp = await client.GetAsync("api/roles/10000");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRoleAppService>();
                var role = await service.GetRoleDetailsAsync(10000);
                // Assert
                expectedResult.Should().BeEquivalentTo(role);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_roles_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var expectedResult = new List<RoleListDto>();
            var role1 = new Role("Production", 10001)
            {
                Id = 10002,
                Division = new Division(DivisionTypeEnum.Production) { Id = 10001 }
            };
            role1.AddDefaultRoleClaims(new DefaultRoleClaim(10002, 10004));
            role1.Division.AddTranslation(new DivisionTranslation(10001, "Gyártói modul", "Törzsadatok felvételéért felelős jogosultságok", LanguageTypeEnum.HU));

            var roleTranslation1 = new RoleTranslation(role1.Id, "Gyártás", LanguageTypeEnum.HU)
            {
                Id = 10003
            };
            role1.AddTranslation(roleTranslation1);

            var role2 = new Role("Financial", 10002)
            {
                Id = 10004,
                Division = new Division(DivisionTypeEnum.Financial) { Id = 10002 }
            };
            role2.AddDefaultRoleClaims(new DefaultRoleClaim(10004, 10008));
            role2.Division.AddTranslation(new DivisionTranslation(10002, "Pénzügyi modul", "Törzsadatok felvételéért felelős jogosultságok", LanguageTypeEnum.HU));

            var roleTranslation2 = new RoleTranslation(role2.Id, "Pénzügy", LanguageTypeEnum.HU)
            {
                Id = 10005
            };
            role2.AddTranslation(roleTranslation2);

            expectedResult.Add(new RoleListDto(role1));
            expectedResult.Add(new RoleListDto(role2));

            // Act
            var resp = await client.GetAsync("api/roles");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRoleAppService>();
                var roles = await service.GetRolesAsync();

                // Assert
                expectedResult.Should().BeEquivalentTo(roles.Where(ent=> ent.Id == role1.Id || ent.Id == role2.Id));
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_roles_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var role = new RoleCreateDto() { DivisionId = 10000, Name = "Test", ClaimIdList = new List<int>() };
            var content = new StringContent(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/roles", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());
            stringresp.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Put_roles_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var role = new RoleUpdateDto() { ClaimIdList = new List<int>() { } };
            var content = new StringContent(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");

            int id = 10005;
            // Act
            var resp = await client.PutAsync($"api/roles/{id}", content);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRoleAppService>();
                var updatedRole = await service.GetRoleDetailsAsync(id);
                // Assert
                role.ClaimIdList.Should().BeEquivalentTo(updatedRole.ClaimIds);
            }
            resp.EnsureSuccessStatusCode();
        }
    }
}