using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
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
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class CompaniesTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public CompaniesTests(IFPSFactoryWebApplicationFactory factory)
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
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TokenDto>(stringresp);
            return model.AccessToken;
        }

        [Fact]
        public async Task Get_company_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var company1 = BuildCompany();
            company1.AddVersion(new CompanyData("1111", "1111", new Address(6344, "Hajós", "Petőfi utca 52.", 1), null, Clock.Now) { Id = 10000 });
            company1.CompanyType = new CompanyType(CompanyTypeEnum.MyCompany) { Id = 10000 };

            var employee = new User("enco@enco.hu") { Id = 10000, CompanyId = 10000 };
            employee.AddVersion(new UserData("Yevgeny Zamjatin", "+362497489", Clock.Now,
                new Address(1117, "Budapest", "Baranyai utca 7.", 1))
            { Id = 10000 });
            var employee2 = new User("zelda@göttingsberg.com") { Id = 10001, CompanyId = 10000 };
            employee2.AddVersion(new UserData("Yevgeny Zamjatin", "+362497489", Clock.Now,
                new Address(1117, "Budapest", "Baranyai utca 7.", 1))
            { Id = 10000 });

            var encoContact = new User("contact@enco.test.hu") { Id = 10004 };
            encoContact.AddVersion(new UserData("Mihail Bulgakov", "+875345434", Clock.Now,
                new Address(1117, "Budapest", "Baranyai utca 8.", 1))
            { Id = 10001 });

            var expectedResult = new CompanyDetailsDto(company1)
            {
                ContactPerson = new ContactPersonListDto(encoContact),
                Employees = new List<EmployeeListDto>() {
                    new EmployeeListDto(employee) { Role = new List<string>() { "Admin" } },
                    new EmployeeListDto(employee2) { Role = new List<string>() { "Admin" } }
                }
            };

            // Act
            var resp = await client.GetAsync("api/companies/10000");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Get_companies_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<Company>();
            var company1 = BuildCompany();
            var encoContact = new User("contact@enco.test.hu") { Id = 10004 };
            encoContact.AddVersion(BuildUserData1());
            company1.AddVersion(BuildCompanyData(encoContact));
            Company company2 = BuildCompany2();
            company2.AddVersion(BuildUserData2());

            expectedResult.Add(company1);
            expectedResult.Add(company2);

            PagedList<Company> pagedList = new PagedList<Company>
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = expectedResult.Count
            };
            var result = pagedList.ToPagedList(CompanyListDto.FromEntity);

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICompanyAppService>();
                CompanyFilterDto companyFilterDto = new CompanyFilterDto() { Name = "Test", CompanyType = CompanyTypeEnum.MyCompany };
                var companies = await service.GetCompaniesAsync(companyFilterDto);

                // Assert

                result.Should().BeEquivalentTo(companies);
            }
        }

        [Fact]
        public async Task Get_companies_filter_name_works()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICompanyAppService>();
                CompanyFilterDto companyFilterDto = new CompanyFilterDto() { Name = "Wrong name" };
                var companies = await service.GetCompaniesAsync(companyFilterDto);

                // Assert
                companies.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_companies_filter_companytyape_works()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICompanyAppService>();
                CompanyFilterDto companyFilterDto = new CompanyFilterDto() { CompanyType = CompanyTypeEnum.Other };
                var companies = await service.GetCompaniesAsync(companyFilterDto);

                // Assert
                companies.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_companies_filter_address_works()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICompanyAppService>();
                CompanyFilterDto companyFilterDto = new CompanyFilterDto() { Address = "Wrong address" };
                var companies = await service.GetCompaniesAsync(companyFilterDto);

                // Assert
                companies.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Post_companies_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var company = new CompanyCreateDto()
            {
                Name = "Enco",
                CompanyTypeId = 10000,
                TaxNumber = "12",
                RegisterNumber = "12",
                Address = new AddressCreateDto()
                {
                    PostCode = 1117,
                    Address = "Irinyi",
                    City = "Budapest",
                    CountryId = 1
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/companies", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());
            stringresp.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Put_companies_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var company = new CompanyUpdateDto()
            {
                CompanyTypeId = 10000,
                TaxNumber = "12",
                RegisterNumber = "12",
                Address = new AddressCreateDto()
                {
                    PostCode = 1117,
                    Address = "Irinyi",
                    City = "Budapest",
                    CountryId = 1
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync("api/companies/10002", content);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICompanyAppService>();
                var updatedCompany = await service.GetCompanyDetailsAsync(10002);
                // Assert
                company.CompanyTypeId.Should().Be(updatedCompany.CompanyType.Id);
                company.TaxNumber.Should().BeEquivalentTo(updatedCompany.TaxNumber);
                company.RegisterNumber.Should().BeEquivalentTo(updatedCompany.RegisterNumber);

            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_companies_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            int id = 10004;
            // Act
            var resp = await client.DeleteAsync($"api/companies/{id}");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICompanyAppService>();
                Func<Task> act = async () => { await service.GetCompanyDetailsAsync(id); };

                // Assert
                await act.Should().ThrowAsync<InvalidOperationException>();
                resp.EnsureSuccessStatusCode();
            }
        }

        #region BuildEntities

        private static CompanyData BuildCompanyData(User encoContact)
        {
            return new CompanyData("1111", "1111", new Address(6344, "Hajós", "Petőfi utca 52.", 1), null, Clock.Now)
            { Id = 10000, ContactPerson = encoContact };
        }

        private static CompanyData BuildUserData2()
        {
            return new CompanyData("1111", "1111", new Address(1117, "Budapest", "Bocskai út 77-79.", 1),
                            null, Clock.Now)
            { Id = 10000 };
        }

        private static Company BuildCompany2()
        {
            return new Company("Test Bosch", 10000) { Id = 10009, CompanyType = BuildCompanyType() };
        }

        private static UserData BuildUserData1()
        {
            return new UserData("Mihail Bulgakov", "+875345434", Clock.Now,
                            new Address(1117, "Budapest", "Baranyai utca 8.", 1))
            { Id = 10001 };
        }

        private static Company BuildCompany()
        {
            return new Company("Test EN-CO Software", 10000) { Id = 10000, CompanyType = BuildCompanyType() };
        }

        private static CompanyType BuildCompanyType()
        {
            return new CompanyType(CompanyTypeEnum.MyCompany) { Id = 10000 };
        }

        #endregion

    }
}