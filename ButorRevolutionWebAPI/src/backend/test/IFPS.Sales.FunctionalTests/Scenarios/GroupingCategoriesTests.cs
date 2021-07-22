using ENCO.DDD.Domain.Model.Enums;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class GroupingCategoriesTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public GroupingCategoriesTests(IFPSSalesWebApplicationFactory factory)
        {
            this.factory = factory;
            this.jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            this.jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            this.jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        private async Task<string> GetAccessToken()
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
        public async Task Get_root_categories_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            // Act
            var resp = await client.GetAsync("api/groupingcategories/flatList?LoadOnlyRootObjects=true");

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringResponse = await resp.Content.ReadAsStringAsync();

            var responseObjects = JsonConvert.DeserializeObject<List<GroupingCategoryListDto>>(stringResponse.Trim(), jsonSerializerSettings);

            responseObjects.Where(ent => ent.Id >= 10000).ToList().Count.Should().Be(3);
        }

        [Fact]
        public async Task Get_flat_categories_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            // Act
            var resp = await client.GetAsync("api/groupingcategories/flatList");

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringResponse = await resp.Content.ReadAsStringAsync();

            var responseObjects = JsonConvert.DeserializeObject<List<GroupingCategoryListDto>>(stringResponse.Trim(), jsonSerializerSettings);

            responseObjects.Count.Should().BeGreaterThan(20);
        }

        [Fact]
        public async Task Get_hierarchical_categories_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            // Act
            var resp = await client.GetAsync("api/groupingcategories/hierarchicalList?CategoryType=MaterialType");

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringResponse = await resp.Content.ReadAsStringAsync();

            var responseObjects = JsonConvert.DeserializeObject<List<GroupingCategoryListDto>>(stringResponse.Trim(), jsonSerializerSettings);

            responseObjects.Count.Should().BeGreaterOrEqualTo(2);
        }

        [Fact]
        public async Task Get_category_by_id_works()
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            // Act
            var resp = await client.GetAsync("api/groupingcategories/10002");

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringResponse = await resp.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<GroupingCategoryDetailsDto>(stringResponse.Trim(),
                this.jsonSerializerSettings);

            responseObject.Translations.Count.Should().Be(2);
            responseObject.ParentId.Should().Be(10000);
        }

        [Fact]
        public async Task Get_category_by_wrong_id_cannot_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            // Act
            var resp = await client.GetAsync("api/groupingcategories/10009");

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_categories_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            var groupingCategoryCreateDto = new GroupingCategoryCreateDto
            {
                ParentId = 10002,
                Translations = new GroupingCategoryTranslationCreateDto[]
                {
                    new GroupingCategoryTranslationCreateDto
                    {
                        Name = "Boards",
                        Language = LanguageTypeEnum.EN
                    },
                    new GroupingCategoryTranslationCreateDto
                    {
                        Name = "Bútorlapok",
                        Language = LanguageTypeEnum.HU
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(groupingCategoryCreateDto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/groupingcategories", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringResponse = await resp.Content.ReadAsStringAsync();

            var responseId = JsonConvert.DeserializeObject<int>(stringResponse.Trim(), jsonSerializerSettings);
            responseId.Should().BeGreaterThan(10000);
        }

        [Fact]
        public async Task Put_categories_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            int waschingMachineId = 10003;
            var updateDto = new GroupingCategoryUpdateDto
            {
                ParentGroupId = 10002,
                Translations = new GroupingCategoryTranslationUpdateDto[]
                {
                    new GroupingCategoryTranslationUpdateDto
                    {
                        Id = 10008,
                        Name = "Mosogatógépek 2",
                        Language = LanguageTypeEnum.HU
                    },
                    new GroupingCategoryTranslationUpdateDto
                    {
                        Id = 10009,
                        Name = "Washing machines 2",
                        Language = LanguageTypeEnum.EN
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(updateDto), Encoding.UTF8, "application/json");

            //Act
            var resp = await client.PutAsync($"api/groupingcategories/{waschingMachineId}", content);

            //Assert
            resp.EnsureSuccessStatusCode();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();

                var modifiedGroupingCategory = context.GroupingCategories
                    .AsNoTracking()
                    .Include(ent => ent.Translations)
                    .Include(ent => ent.Children)
                    .Single(ent => ent.Id == waschingMachineId);

                modifiedGroupingCategory.Translations.Single(ent => ent.Language == LanguageTypeEnum.EN)
                    .GroupingCategoryName.Should().Be("Washing machines 2");
                modifiedGroupingCategory.Translations.Single(ent => ent.Language == LanguageTypeEnum.HU)
                    .GroupingCategoryName.Should().Be("Mosogatógépek 2");
            }
        }

        [Fact]
        public async Task Delete_grouping_category_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            int id = 10004;

            //Act
            var resp = await client.DeleteAsync($"api/groupingcategories/{id}");

            //Assert
            resp.EnsureSuccessStatusCode();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();

                var deletedGroupingCategory = context.GroupingCategories.SingleOrDefault(ent => ent.Id == id);

                deletedGroupingCategory.Should().Be(null);
            }
        }

        [Fact]
        public async Task Delete_grouping_category_with_children_cannot_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            int id = 10002;

            //Act
            var resp = await client.DeleteAsync($"api/groupingcategories/{id}");

            //Assert
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_grouping_category_with_bad_id_cannot_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            int id = 10022;

            //Act
            var resp = await client.DeleteAsync($"api/groupingcategories/{id}");

            //Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}