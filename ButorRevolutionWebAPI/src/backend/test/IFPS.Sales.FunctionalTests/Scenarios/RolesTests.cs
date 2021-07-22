using ENCO.DDD.Domain.Model.Enums;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
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

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class RolesTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;

        public RolesTests(IFPSSalesWebApplicationFactory factory)
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

            var expectedResult = new RoleDetailsDto(new Role("Test Expert admin", 1) { Id = 10000 });

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
            var role1 = new Role("Test Expert admin", 10001)
            {
                Id = 10000,
                Division = new Division(DivisionTypeEnum.Admin) { Id = 256 }
            };
            role1.Division.AddTranslation(new DivisionTranslation(10010, "Admin", "Admin szerepkörök", LanguageTypeEnum.HU));

            var role2 = new Role("Test Customer", 10005)
            {
                Id = 10001,
                Division = new Division(DivisionTypeEnum.Customer) { Id = 300 }
            };
            role2.Division.AddTranslation(new DivisionTranslation(10011, "Megrendelő", "Megrendelői szerepkörök", LanguageTypeEnum.HU));
            expectedResult.Add(new RoleListDto(role1));
            expectedResult.Add(new RoleListDto(role2));
                       
            // Act
            var resp = await client.GetAsync("api/roles");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRoleAppService>();
                var roles = await service.GetRolesAsync();

                // Assert
                expectedResult.Should().BeEquivalentTo(roles.Where(ent => ent.Name.Contains("Test")).ToList());
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_roles_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var role = new RoleCreateDto() { DivisionId = 1, Name = "Customer", ClaimIdList = new List<int>() };
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

            var role = new RoleUpdateDto() { ClaimIdList = new List<int>() };
            var content = new StringContent(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync("api/roles/10000", content);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRoleAppService>();
                var updatedRole = await service.GetRoleDetailsAsync(10000);
                // Assert
                role.ClaimIdList.Should().BeEquivalentTo(updatedRole.ClaimIds);
            }
            resp.EnsureSuccessStatusCode();
        }
    }
}