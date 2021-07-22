using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class InspectionsTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public InspectionsTests(IFPSFactoryWebApplicationFactory factory)
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
        public async Task Get_inspection_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var inspection = new Inspection(Clock.Now, 10000) { InspectedOn = new DateTime(2019, 10, 19), Report = new Report("Report1") { Id = 10000 }, InspectedStorageId = 10000, ReportId = 10000 };
            inspection.UpdateUserInspections(new List<int> { 10000 });
            var expectedResult = new InspectionDetailsDto(inspection);

            // Act
            var resp = await client.GetAsync("api/inspections/10000");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Update_inspection_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var inspection = new InspectionUpdateDto
            {
                InspectedOn = Clock.Now,
                ReportName = "New report name",
                StorageId = 10000,
                DelegationIds = new List<int> { 10000, 10001 }
            };
            var content = new StringContent(JsonConvert.SerializeObject(inspection), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync("api/inspections/10001", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IInspectionAppService>();
                var updatedInspection = await service.GetInspectionAsync(10001);
                // Assert
                inspection.InspectedOn.Should().Be(updatedInspection.InspectedOn);
                inspection.ReportName.Should().BeEquivalentTo(updatedInspection.ReportName);
                inspection.StorageId.Should().Be(updatedInspection.StorageId);
                inspection.DelegationIds.Should().BeEquivalentTo(updatedInspection.DelegationIds);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_inspection_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync("api/inspections/10002");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IInspectionAppService>();
                Func<Task> act = async () => { await service.GetInspectionAsync(10002); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_inspection_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var inspection = new InspectionCreateDto
            {
                InspectedOn = Clock.Now,
                ReportName = "New report name",
                StorageId = 10000,
                DelegationIds = new List<int> { 10000, 10001 }
            };
            var content = new StringContent(JsonConvert.SerializeObject(inspection), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/inspections", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());
            stringresp.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Get_inspections_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<InspectionListDto>();
            var inspection = new InspectionListDto()
            {
                Id = 10003,
                InspectedOn = new DateTime(2019, 10, 19),
                ReportName = "Report4",
                StorageId = 10003,
                StorageName = "Storage Nr4",
                DelegationNames = new List<string> { "Yevgeny Zamjatin" }
            };

            expectedResult.Add(inspection);
            IPagedList<InspectionListDto> pagedList = new PagedList<InspectionListDto>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IInspectionAppService>();
                InspectionFilterDto inspectionFilterDto = new InspectionFilterDto() { InspectedOn = new DateTime(2019, 10, 19), StorageId = 10003 };
                var inspections = await service.GetInspectionsAsync(inspectionFilterDto);

                // Assert
                result.Should().BeEquivalentTo(inspections);
            }
        }

        [Fact]
        public async Task Get_inspections_filter_inspected_on_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IInspectionAppService>();
                InspectionFilterDto inspectionFilterDto = new InspectionFilterDto() { InspectedOn = new DateTime(2000, 10, 19) };
                var inspections = await service.GetInspectionsAsync(inspectionFilterDto);

                // Assert
                inspections.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_inspections_filter_storage_id_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IInspectionAppService>();
                InspectionFilterDto inspectionFilterDto = new InspectionFilterDto() { StorageId = 12125 };
                var inspections = await service.GetInspectionsAsync(inspectionFilterDto);

                // Assert
                inspections.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_inspections_filter_report_name_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IInspectionAppService>();
                InspectionFilterDto inspectionFilterDto = new InspectionFilterDto() { ReportName = "Wrong report name" };
                var inspections = await service.GetInspectionsAsync(inspectionFilterDto);

                // Assert
                inspections.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_inspections_filter_delegation_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IInspectionAppService>();
                InspectionFilterDto inspectionFilterDto = new InspectionFilterDto() { Delegation = "Wrong name" };
                var inspections = await service.GetInspectionsAsync(inspectionFilterDto);

                // Assert
                inspections.Data.Count().Should().Be(0);
            }
        }
    }
}