using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Paging;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class FurnitureUnitsTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public FurnitureUnitsTests(IFPSFactoryWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
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
        public async Task Get_furnitureunits_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            IPagedList<FurnitureUnitListDto> pagedList = new PagedList<FurnitureUnitListDto>()
            {
                Items = BuildFurnitureUnitListDto(),
                PageIndex = 0,
                PageSize = 20,
                TotalCount = BuildFurnitureUnitListDto().Count
            };

            var result = pagedList.ToPagedList();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFurnitureUnitAppService>();
                var funitFilterDto = new FurnitureUnitFilterDto();
                var furnitureUnits = await service.GetFurnitureUnitsAsync(funitFilterDto);

                // Assert
                result.Should().BeEquivalentTo(furnitureUnits);
            }
        }

        #region BuildEntities
        private static List<FurnitureUnitListDto> BuildFurnitureUnitListDto()
        {
            List<FurnitureUnitListDto> funits = new List<FurnitureUnitListDto>();

            GroupingCategory category = new GroupingCategory(GroupingCategoryEnum.MaterialType) { Id = 10000 };
            category.AddTranslation(new GroupingCategoryTranslation(10000, "Alapanyag típusok", ENCO.DDD.Domain.Model.Enums.LanguageTypeEnum.HU) { Id = 10001 });

            Image image = new Image("test.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), ThumbnailName = "test_thumbnail.jpg" };

            funits.Add(new FurnitureUnitListDto(new FurnitureUnit("EXSA", 1, 1, 1)
            {
                Id = new Guid("1a44bdef-80bc-49c7-9b03-6d597bf36e47"),
                Category = category,
                Image = image,
                BaseFurnitureUnitId = null,
                Description = "Miracle Desc",
                CurrentPriceId = 10000
            }));
            funits.Add(new FurnitureUnitListDto(new FurnitureUnit("EXSE", 1, 2, 3)
            {
                Id = new Guid("5db3fb96-1619-4b05-afa2-9485c282db76"),
                Category = category,
                Image = image,               
                BaseFurnitureUnitId = new Guid("1a44bdef-80bc-49c7-9b03-6d597bf36e47"),
                Description = "Brilliant Desc",
                CurrentPriceId = 10001
            }));
            funits.Add(new FurnitureUnitListDto(new FurnitureUnit("Reservation 02", 600, 200, 300)
            {
                Id = new Guid("f4f37e65-379c-4569-a720-83e4f9ec0e90"),
                Category = category,
                Image = image,              
                BaseFurnitureUnitId = null,
                Description = "lorem ipsum 02",
                CurrentPriceId = 10001
            }));

            funits.Add(new FurnitureUnitListDto(new FurnitureUnit(code: "EXSA", width: 1, height: 1, depth: 1)
            {
                Id = new Guid("7ED9EDE3-C0C7-492C-B236-38E4E47BAA10"),
                Category = category,
                Image = image,
                BaseFurnitureUnitId = null,
                Description = "Miracle Desc",
                CurrentPriceId = 10000
            }));
            funits.Add(new FurnitureUnitListDto(new FurnitureUnit(code: "EXSE", width: 1, height: 2, depth: 3)
            {
                Id = new Guid("31E41E48-C8D2-4AEB-873D-76BC4A7C55E4"),
                Category = category,
                Image = image,
                BaseFurnitureUnitId = new Guid("1A44BDEF-80BC-49C7-9B03-6D597BF36E47"),
                Description = "Brilliant Desc",
                CurrentPriceId = 10001
            }));
            funits.Add(new FurnitureUnitListDto(new FurnitureUnit(code: "EXDC", width: 1, height: 3, depth: 4)
            {
                Id = new Guid("DDC51E74-B8A2-45F6-B0D9-5D0E5AFA1D88"),
                Category = category,
                Image = image,
                BaseFurnitureUnitId = new Guid("1A44BDEF-80BC-49C7-9B03-6D597BF36E47"),
                Description = "Awesome Desc",
                CurrentPriceId = 10001
            }));
            funits.Add(new FurnitureUnitListDto(new FurnitureUnit(code: "EXFA", width: 2, height: 3, depth: 4)
            {
                Id = new Guid("FFFC8FEF-6FF2-4D85-B63B-90B156B49055"),
                Category = category,
                Image = image,
                BaseFurnitureUnitId = new Guid("1A44BDEF-80BC-49C7-9B03-6D597BF36E47"),
                Description = "Cool Desc",
                CurrentPriceId = 10001
            }));

            return funits;
        }
        #endregion
    }
}
