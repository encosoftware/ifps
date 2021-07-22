using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
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

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class CompaniesTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public CompaniesTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_company_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var company1 = new Company("Test EN-CO Software", 1) { Id = 10000 };

            company1.AddVersion(new CompanyData("1111", "1111", new Address(6344, "Hajós", "Petőfi utca 52.", 1), null, Clock.Now) { Id = 10000 });

            company1.CompanyType = new CompanyType(CompanyTypeEnum.MyCompany) { Id = 6 };

            var employee = new User("le@enco.hu") { Id = 10030, CompanyId = 10000 };
            employee.AddVersion(new UserData("Lapos Elemér", "06301112222", Clock.Now,
                new Address(5000, "Szolnok", "Kossuth tér 1.", 1))
            { Id = 10030 });
            
            //var employee2 = new User("enco@enco.hu") { Id = 1, CompanyId = 10000 };
            //employee2.AddVersion(new UserData("SUPER ADMIN", "06205555555", Clock.Now,
            //    new Address(5000, "Szolnok", "Kossuth tér 1.", 1))
            //{ Id = 1 });
            //employee2.AddRoles(new List<Role> { new Role("Admin Expert", 1) });


            var encoContact = new User("enco2@enco.hu") { Id = 10000 };
            encoContact.AddVersion(new UserData("Kelemen Elemér", "06301112222", Clock.Now,
                new Address(5000, "Szolnok", "Kossuth tér 3.", 1))
            { Id = 10000 });

            var expectedResult = new CompanyDetailsDto(company1)
            {
                ContactPerson = new ContactPersonListDto(encoContact),
                Employees = new List<EmployeeListDto>() {
                    //new EmployeeListDto(employee2),
                    new EmployeeListDto(employee)
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
            var company1 = new Company("Test EN-CO Software", 1) { Id = 10000 };
            company1.CompanyType = new CompanyType(CompanyTypeEnum.MyCompany) { Id = 6 };
            var company2 = new Company("Test Bosch", 1) { Id = 10001 };
            company2.CompanyType = new CompanyType(CompanyTypeEnum.MyCompany) { Id = 7 };

            company1.AddVersion(new CompanyData("1111", "1111", new Address(6344, "Hajós", "Petőfi utca 52.", 1), null, Clock.Now) { Id = 10000 });
            company2.AddVersion(new CompanyData("1111", "1111", new Address(1117, "Budapest", "Bocskai út 77-79.", 1), null, Clock.Now) { Id = 10001 });

            expectedResult.Add(company2);
            PagedList<Company> pagedList = new PagedList<Company>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList(CompanyListDto.FromEntity);

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICompanyAppService>();
                CompanyFilterDto companyFilterDto = new CompanyFilterDto() { Name = "Test", CompanyType = CompanyTypeEnum.MyCompany, Address = "Budapest" };
                var companies = await service.GetCompaniesAsync(companyFilterDto);

                // Assert

                result.Should().BeEquivalentTo(companies);
            }
        }

        [Fact]
        public async Task Get_CompanyName_By_Id_Works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var company = new Company("Test EN-CO Software", 1) { Id = 10000 };

            //Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICompanyAppService>();
                var name = await service.GetCompanyNameByIdAsync(company.Id);

                // Assert
                company.Name.Should().BeEquivalentTo(name);
            }
        }

        [Fact]
        public async Task Get_Non_Existing_Company_By_Id_Not_Works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var company = new Company("Test EN-CO Software", 1) { Id = 10099 };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICompanyAppService>();

                //Act & Assert
                await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetCompanyNameByIdAsync(company.Id));
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
        public async Task Get_companies_filter_companytype_works()
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
                CompanyTypeId = 1,
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
                CompanyTypeId = 1,
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
            var resp = await client.PutAsync("api/companies/10000", content);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICompanyAppService>();
                var updatedCompany = await service.GetCompanyDetailsAsync(10000);

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

            // Act
            var resp = await client.DeleteAsync("api/companies/1");

            // Assert
            resp.EnsureSuccessStatusCode();
        }
    }
}