using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
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
    public class UsersTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public UsersTests(IFPSSalesWebApplicationFactory factory)
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

        #region Get Users test

        [Fact]
        public async Task Get_users_filter_email_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<User>();

            var user1 = new User("we@enco.test.hu") { Id = 10001, CreationTime = new DateTime(2019, 03, 01) };
            user1.AddVersion(new UserData("Wincs Eszter", "06301234567", Clock.Now,
                new Address(1117, "Budapest", "Seholse utca 1.", 1))
            { Id = 10001, CoreId = 10001 });

            var user2 = new User("ee@enco.test.hu") { Id = 10002, CreationTime = new DateTime(2019, 05, 10) };
            user2.AddVersion(new UserData("Ebéd Elek", "06301112222", Clock.Now,
                new Address(5000, "Szolnok", "Kossuth tér 3.", 1))
            { Id = 10002, CoreId = 10002 });
            var user3 = new User("el@enco.test.hu") { Id = 10003, CreationTime = new DateTime(2019, 06, 05) };
            user3.AddVersion(new UserData("Eszte Lenke", "06309876543", Clock.Now,
                new Address(1117, "Budapest", "Irinyi József utca 42.", 1))
            { Id = 10003, CoreId = 10003 });

            expectedResult.AddRange(new User[] { user1, user2, user3 });

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 3
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);

            result.Data[0].AddRole("Teszt Very Expert admin");
            result.Data[0].Company = "Teszt Nokia";
            result.Data[1].AddRole("Teszt Expert Customer");
            result.Data[1].Company = "Teszt Bakosfa";

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { Email = "@enco.test.hu" };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_users_filter_wrong_email_works()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { Email = "Wrong email" };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_users_filter_name_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<User>();

            var user2 = new User("ee@enco.test.hu") { Id = 10002, CreationTime = new DateTime(2019, 05, 10) };

            user2.AddVersion(new UserData("Ebéd Elek", "06301112222", Clock.Now,
                new Address(5000, "Szolnok", "Kossuth tér 3.", 1))
            { Id = 10002, CoreId = 10002 });

            expectedResult.AddRange(new User[] { user2 });

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);

            result.Data[0].AddRole("Teszt Expert Customer");
            result.Data[0].Company = "Teszt Bakosfa";

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { Name = "Elek" };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_users_filter_wrong_name_works()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { Name = "Wrong name" };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_users_filter_phone_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<User>();

            var user3 = new User("el@enco.test.hu") { Id = 10003, CreationTime = new DateTime(2019, 06, 05) };
            user3.AddVersion(new UserData("Eszte Lenke", "06309876543", Clock.Now,
                new Address(1117, "Budapest", "Irinyi József utca 42.", 1))
            { Id = 10003, CoreId = 10003 });

            expectedResult.AddRange(new User[] { user3 });

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { PhoneNumber = "06309876543" };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_users_filter_wrong_phone_works()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { PhoneNumber = "999999999" };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_users_filter_company_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<User>();

            var user1 = new User("we@enco.test.hu") { Id = 10001, CreationTime = new DateTime(2019, 03, 01) };
            user1.AddVersion(new UserData("Wincs Eszter", "06301234567", Clock.Now,
                new Address(1117, "Budapest", "Seholse utca 1.", 1))
            { Id = 10001, CoreId = 10001 });

            expectedResult.AddRange(new User[] { user1 });

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);

            result.Data[0].AddRole("Teszt Very Expert admin");
            result.Data[0].Company = "Teszt Nokia";

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { CompanyName = "Nokia", Email = "@enco.test.hu" };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_users_filter_wrong_company_works()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { CompanyName = "Disney" };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_users_filter_role_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<User>();

            var user1 = new User("we@enco.test.hu") { Id = 10001, CreationTime = new DateTime(2019, 03, 01) };
            user1.AddVersion(new UserData("Wincs Eszter", "06301234567", Clock.Now,
                new Address(1117, "Budapest", "Seholse utca 1.", 1))
            { Id = 10001, CoreId = 10001 });

            expectedResult.AddRange(new User[] { user1 });

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);

            result.Data[0].AddRole("Teszt Very Expert admin");
            result.Data[0].Company = "Teszt Nokia";

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { RoleId = 10002 };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_users_filter_wrong_role_works()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { RoleId = 99 };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_users_filter_isActive_works()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { IsActive = true, Email = "@enco.test.hu" };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Data.Count().Should().BeGreaterOrEqualTo(3);
            }
        }

        [Fact]
        public async Task Get_users_filter_isActive_works_2()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { IsActive = false, Email = "@enco.test.hu" };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_users_filter_AddedOnFrom_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<User>();

            var user2 = new User("ee@enco.test.hu") { Id = 10002, CreationTime = new DateTime(2019, 05, 10) };
            user2.AddVersion(new UserData("Ebéd Elek", "06301112222", Clock.Now,
                new Address(5000, "Szolnok", "Kossuth tér 3.", 1))
            { Id = 10002, CoreId = 10002 });
            var user3 = new User("el@enco.test.hu") { Id = 10003, CreationTime = new DateTime(2019, 06, 05) };
            user3.AddVersion(new UserData("Eszte Lenke", "06309876543", Clock.Now,
                new Address(1117, "Budapest", "Irinyi József utca 42.", 1))
            { Id = 10003, CoreId = 10003 });

            expectedResult.AddRange(new User[] { user2, user3 });

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 2
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);

            result.Data[0].AddRole("Teszt Expert Customer");
            result.Data[0].Company = "Teszt Bakosfa";

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { Email = "@enco.test.hu", AddedOnFrom = new DateTime(2019, 04, 01) };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                //foreach (var user in users.Data)
                //{
                //    user.AddedOn.Should().BeOnOrAfter(userFilterDto.AddedOnFrom.Value);
                //    result.Data.Single(u => u.Id == user.Id).AddedOn = user.AddedOn;
                //}
                users.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_users_filter_AddedOnTo_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<User>();

            var user1 = new User("we@enco.test.hu") { Id = 10001, CreationTime = new DateTime(2019, 03, 01) };
            user1.AddVersion(new UserData("Wincs Eszter", "06301234567", Clock.Now,
                new Address(1117, "Budapest", "Seholse utca 1.", 1))
            { Id = 10001, CoreId = 10001 });

            var user2 = new User("ee@enco.test.hu") { Id = 10002, CreationTime = new DateTime(2019, 05, 10) };
            user2.AddVersion(new UserData("Ebéd Elek", "06301112222", Clock.Now,
                new Address(5000, "Szolnok", "Kossuth tér 3.", 1))
            { Id = 10002, CoreId = 10002 });

            expectedResult.AddRange(new User[] { user1, user2 });

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 2
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);

            result.Data[0].AddRole("Teszt Very Expert admin");
            result.Data[0].Company = "Teszt Nokia";
            result.Data[1].AddRole("Teszt Expert Customer");
            result.Data[1].Company = "Teszt Bakosfa";

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { Email = "@enco.test.hu", AddedOnTo = new DateTime(2019, 05, 30) };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_users_filter_AddedOn_Range_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<User>();

            var user2 = new User("ee@enco.test.hu") { Id = 10002, CreationTime = new DateTime(2019, 05, 10) };
            user2.AddVersion(new UserData("Ebéd Elek", "06301112222", Clock.Now,
                new Address(5000, "Szolnok", "Kossuth tér 3.", 1))
            { Id = 10002, CoreId = 10002 });

            expectedResult.AddRange(new User[] { user2 });

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);

            result.Data[0].AddRole("Teszt Expert Customer");
            result.Data[0].Company = "Teszt Bakosfa";

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto()
                {
                    Email = "@enco.test.hu",
                    AddedOnFrom = new DateTime(2019, 04, 01),
                    AddedOnTo = new DateTime(2019, 05, 30)
                };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Should().BeEquivalentTo(result);
            }
        }

        #endregion Get Users test

        #region Post user

        [Fact]
        public async Task Post_user_base_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var createDto = new UserCreateDto
            {
                Name = "Metall_Ica",
                Email = "mi@enco.eu",
                PhoneNumber = "06302468135",
                RoleId = 10006,
                Password = "Password1"
            };
            var content = new StringContent(JsonConvert.SerializeObject(createDto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/users", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());
            stringresp.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Post_user_with_customer_role_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var createDto = new UserCreateDto
            {
                Name = "Csik_Lorant",
                Email = "csl@enco.eu",
                PhoneNumber = "",
                RoleId = 10003,
                Password = "Password1"
            };
            var content = new StringContent(JsonConvert.SerializeObject(createDto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/users", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var respContent = int.Parse(await resp.Content.ReadAsStringAsync());
            respContent.Should().BeOfType(typeof(int));
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
                var customerEntityExist = await customerRepository.IsCustomerExistsAsync(respContent);

                customerEntityExist.Should().BeTrue();

                var salesPersonRepository = scope.ServiceProvider.GetRequiredService<ISalesPersonRepository>();
                var salesPersonEntityExist = await salesPersonRepository.IsSalesPersonExistsAsync(respContent);

                salesPersonEntityExist.Should().BeFalse();

                var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
                var employeeEntityExist = await employeeRepository.IsEmployeeExistsAsync(respContent);

                employeeEntityExist.Should().BeFalse();
            }
        }

        [Fact]
        public async Task Post_user_with_salseperson_role_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var createDto = new UserCreateDto
            {
                Name = "Vi_Zora",
                Email = "vz@enco.eu",
                PhoneNumber = "",
                RoleId = 10004,
                Password = "Password1"
            };
            var content = new StringContent(JsonConvert.SerializeObject(createDto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/users", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var respContent = int.Parse(await resp.Content.ReadAsStringAsync());
            respContent.Should().BeOfType(typeof(int));
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
                var customerEntityExist = await customerRepository.IsCustomerExistsAsync(respContent);

                customerEntityExist.Should().BeFalse();

                var salesPersonRepository = scope.ServiceProvider.GetRequiredService<ISalesPersonRepository>();
                var salesPersonEntityExist = await salesPersonRepository.IsSalesPersonExistsAsync(respContent);

                salesPersonEntityExist.Should().BeTrue();

                var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
                var employeeEntityExist = await employeeRepository.IsEmployeeExistsAsync(respContent);

                employeeEntityExist.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Post_user_with_employee_creation_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var createDto = new UserCreateDto
            {
                Name = "Ga_Zora",
                Email = "gz@enco.eu",
                PhoneNumber = "",
                RoleId = 10005,
                Password = "Password1"
            };
            var content = new StringContent(JsonConvert.SerializeObject(createDto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/users", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var respContent = int.Parse(await resp.Content.ReadAsStringAsync());
            respContent.Should().BeOfType(typeof(int));
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
                var customerEntityExist = await customerRepository.IsCustomerExistsAsync(respContent);

                customerEntityExist.Should().BeFalse();

                var salesPersonRepository = scope.ServiceProvider.GetRequiredService<ISalesPersonRepository>();
                var salesPersonEntityExist = await salesPersonRepository.IsSalesPersonExistsAsync(respContent);

                salesPersonEntityExist.Should().BeFalse();

                var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
                var employeeEntityExist = await employeeRepository.IsEmployeeExistsAsync(respContent);

                employeeEntityExist.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Post_user_dto_validation_works_name_empty()
        {
            // Arrange
            var client = factory.CreateClient();
            var createDto = new UserCreateDto
            {
                Name = " ",
                Email = "gz@enco.eu",
                PhoneNumber = "",
                RoleId = 10005
            };
            var content = new StringContent(JsonConvert.SerializeObject(createDto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/users", content);

            // Assert
            resp.IsSuccessStatusCode.Should().BeFalse();
            resp.StatusCode.Should().Equals(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_user_dto_validation_works_email_empty()
        {
            // Arrange
            var client = factory.CreateClient();
            var createDto = new UserCreateDto
            {
                Name = "Ga_Zora",
                Email = " ",
                PhoneNumber = "",
                RoleId = 10005
            };
            var content = new StringContent(JsonConvert.SerializeObject(createDto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/users", content);

            // Assert
            resp.IsSuccessStatusCode.Should().BeFalse();
            resp.StatusCode.Should().Equals(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_user_dto_validation_works_phonenumber_null()
        {
            // Arrange
            var client = factory.CreateClient();
            var createDto = new UserCreateDto
            {
                Name = "Ga_Zora",
                Email = "gz@enco.eu",
                PhoneNumber = null,
                RoleId = 10005
            };
            var content = new StringContent(JsonConvert.SerializeObject(createDto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/users", content);

            // Assert
            resp.IsSuccessStatusCode.Should().BeFalse();
            resp.StatusCode.Should().Equals(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_user_role_validation_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var createDto = new UserCreateDto
            {
                Name = "Ga_Zora",
                Email = "gz@enco.eu",
                PhoneNumber = "",
                RoleId = 9999999
            };
            var content = new StringContent(JsonConvert.SerializeObject(createDto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/users", content);

            // Assert
            resp.IsSuccessStatusCode.Should().BeFalse();
            resp.StatusCode.Should().Equals(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_user_duplicated_email_not_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var createDto = new UserCreateDto
            {
                Name = "Wincs_Eszter",
                Email = "we@enco.test.hu",
                PhoneNumber = "06301234567",
                RoleId = 10006
            };
            var content = new StringContent(JsonConvert.SerializeObject(createDto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/users", content);

            // Assert
            resp.StatusCode.Should().Equals(400);
        }

        #endregion Post user

        #region Activate Deactivate User

        [Fact]
        public async Task Deactivate_users_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var userId = 10004;

            // Act
            var resp = await client.PutAsync($"api/users/{userId}/deactivate", new StringContent(""));

            // Assert
            resp.EnsureSuccessStatusCode();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var user = await userRepository.GetAsync(userId);

                user.IsActive.Should().BeFalse();
            }
        }

        [Fact]
        public async Task Activate_users_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var userId = 10005;

            // Act
            var resp = await client.PutAsync($"api/users/{userId}/activate", new StringContent(""));

            // Assert
            resp.EnsureSuccessStatusCode();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var user = await userRepository.GetAsync(userId);

                user.IsActive.Should().BeTrue();
            }
        }

        #endregion Activate Deactivate User

        #region Delete User

        [Fact]
        public async Task Delete_user_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var content = new StringContent(JsonConvert.SerializeObject(CreateUser()), Encoding.UTF8, "application/json");
            var createResp = await client.PostAsync("api/users", content);
            createResp.EnsureSuccessStatusCode();
            var userId = int.Parse(await createResp.Content.ReadAsStringAsync());

            // Act
            var resp = await client.DeleteAsync($"api/users/{userId}");

            // Assert
            resp.EnsureSuccessStatusCode();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
                var customerEntityExist = await customerRepository.IsCustomerExistsAsync(userId);

                customerEntityExist.Should().BeFalse();

                var salesPersonRepository = scope.ServiceProvider.GetRequiredService<ISalesPersonRepository>();
                var salesPersonEntityExist = await salesPersonRepository.IsSalesPersonExistsAsync(userId);

                salesPersonEntityExist.Should().BeFalse();

                var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
                var employeeEntityExist = await employeeRepository.IsEmployeeExistsAsync(userId);

                employeeEntityExist.Should().BeFalse();

                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                await Assert.ThrowsAsync<EntityNotFoundException>(() => userRepository.GetAsync(userId));
            }
        }

        #endregion Delete User

        #region Get User Deatails

        [Fact]
        public async Task Get_user_base_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var expectedResult = new UserDetailsDto
            {
                Id = 10001,
                Name = "Wincs Eszter",
                Email = "we@enco.test.hu",
                PhoneNumber = "06301234567",
                Address = new AddressDetailsDto(new Address(1117, "Budapest", "Seholse utca 1.", 1)),
                Company = new CompanyDto { Id = 10002, Name = "Teszt Nokia" },
                IsActive = true,
                IsEmployee = false,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>(),
                OwnedRolesIds = new List<int>() { 10002 },
            };

            // Act
            var resp = await client.GetAsync("api/users/10001");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Get_user_all_details_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var userId = 10006;
            var expectedResult = new UserDetailsDto
            {
                Id = userId,
                Name = "Nemer Eszti",
                Email = "ne@enco.hu",
                PhoneNumber = "06701234567",
                Address = new AddressDetailsDto(new Address(5000, "Szolnok", "Seholse utca 1.", 1)),
                Company = new CompanyDto { Id = 10003, Name = "Teszt Bakosfa" },
                IsActive = true,
                IsEmployee = true,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { 10000, 10001, 10002, 10003, 10004, 10005 },
                OwnedRolesIds = new List<int>() { 10003,10004 },

                WorkingInfo = new WorkingInfoDto
                {
                    MaxDiscountPercent = 10,
                    MinDiscountPercent = 1,
                    Offices = new List<VenueDto>(){
                        new VenueDto
                              {
                                 Id = 1,
                                 Name ="Iroda (seed)"
                              }},
                    Supervisees = new List<UserDto>(){
                        new UserDto
                              {
                                 Email ="dd@enco.hu",
                                 Id = 10008,
                                 Name ="Dia Dóra",
                                 PhoneNumber ="",
                              },
                        new UserDto
                              {
                                 Email = "hz@enco.hu",
                                 Id = 10009,
                                 Name ="Hú Zóra",
                                 PhoneNumber ="",
                              }},
                    Supervisor = new UserDto
                    {
                        Email = "to@enco.hu",
                        Id = 10007,
                        Name = "Tök Ödön",
                        PhoneNumber = "",
                    },
                    Teams = new List<string>() {
                        "Bakosfa User Team"},
                    WorkingHours = new List<WorkingHourDto>(){
                        new WorkingHourDto()
                              {
                                 DayTypeId = 1,
                                 From = new DateTime(0001,01,01,09,00,00),
                                 Id = 10001,
                                 To = new DateTime(0001,01,01,12,00,00),
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 1,
                                 From = new DateTime(0001,01,01,13,00,00),
                                 Id = 10002,
                                 To = new DateTime(0001,01,01,16,00,00)
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 2,
                                 From = new DateTime(0001,01,01,09,00,00),
                                 Id = 10003,
                                 To = new DateTime(0001,01,01,14,00,00)
                              },
                        }
                },
                Notifications = new NotificationsDto
                {
                    EventTypeIds = new List<int>() { 1, 4, 2, 3 },
                    NotificationTypeFlags = new List<string>() { "SMS", "Email" },
                },
                IsCompanyNeeded = true
            };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                var result = await service.GetUserDetailsAsync(userId);

                // Assert
                result.Should().BeEquivalentTo(expectedResult);
            }
        }

        #endregion Get User Deatails

        #region Update User Test

        [Fact]
        public async Task Put_user_base_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var userId = 10012;

            var preActResult = new UserDetailsDto
            {
                Id = userId,
                Name = "Para Zita",
                Email = "pa@enco.hu",
                PhoneNumber = "06301234567",
                Address = new AddressDetailsDto(new Address(1117, "Budapest", "Seholse utca 1.", 1)),
                Company = new CompanyDto { Id = 10002, Name = "Teszt Nokia" },
                IsActive = true,
                IsEmployee = false,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>(),
                OwnedRolesIds = new List<int>() { 10006 },
                Notifications = null,
                WorkingInfo = null,
                IsCompanyNeeded = true
            };

            var updateDto = new UserUpdateDto
            {
                Name = "Pár Zoltán",
                Email = "pz@enco.hu",
                PhoneNumber = "06709876542",
                Address = new AddressCreateDto()
                {
                    CountryId = 2,
                    PostCode = 5000,
                    City = "Szolnok",
                    Address = "Kossuth tér 3.",
                },
                CompanyId = 10001,
                Language = LanguageTypeEnum.EN,
                OwnedClaimsIds = new List<int>() { 10000 },
                OwnedRolesIds = new List<int>() { 10006 },
            };

            var postActResult = new UserDetailsDto
            {
                Id = userId,
                Name = "Pár Zoltán",
                Email = "pz@enco.hu",
                PhoneNumber = "06709876542",
                Address = new AddressDetailsDto(new Address(5000, "Szolnok", "Kossuth tér 3.", 2)),
                Company = new CompanyDto { Id = 10001, Name = "Test Bosch" },
                IsActive = true,
                IsEmployee = true,
                Language = LanguageTypeEnum.EN,
                OwnedClaimsIds = new List<int>() { 10000 },
                OwnedRolesIds = new List<int>() { 10006 },
                Notifications = null,
                WorkingInfo = null,
                IsCompanyNeeded = true
            };

            var preActState = await client.GetAsync($"api/users/{userId}");
            preActState.EnsureSuccessStatusCode();
            var stringresp = await preActState.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(preActResult, jsonSerializerSettings));

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(updateDto), Encoding.UTF8, "application/json");
            var resp = await client.PutAsync($"api/users/{userId}", content);

            // Assert
            resp.EnsureSuccessStatusCode();

            var client2 = factory.CreateClient();

            var postActState = await client.GetAsync($"api/users/{userId}");
            postActState.EnsureSuccessStatusCode();
            stringresp = await postActState.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(postActResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Put_user_remove_claims_roles_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10010;

            var updateDto = new UserUpdateDto
            {
                Name = "Nemer Eszti2",
                Email = "ne2@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 2,
                    PostCode = 5000,
                    City = "Szolnok",
                    Address = "Seholse utca 1.",
                },
                CompanyId = 10003,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { },
                OwnedRolesIds = new List<int>() { },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                var preActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .Include(ent => ent.Roles)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);
                preActUser.Should().NotBeNull();
                preActUser.Claims.Should().NotBeEmpty();
                preActUser.Roles.Should().NotBeEmpty();
                preActUser.Claims.Should().HaveCount(6);
                preActUser.Roles.Should().HaveCount(2);

                // Act
                await service.UpdateUserAsync(userId, updateDto);
            }

            var client2 = factory.CreateClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                // Assert
                var postActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .Include(ent => ent.Roles)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);

                postActUser.Should().NotBeNull();
                postActUser.Claims.Should().BeEmpty();
                postActUser.Roles.Should().BeEmpty();

                var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
                var customerEntityExist = await customerRepository.IsCustomerExistsAsync(userId);
                customerEntityExist.Should().BeFalse();

                var salesPersonRepository = scope.ServiceProvider.GetRequiredService<ISalesPersonRepository>();
                var salesPersonEntityExist = await salesPersonRepository.IsSalesPersonExistsAsync(userId);
                salesPersonEntityExist.Should().BeFalse();

                var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
                var employeeEntityExist = await employeeRepository.IsEmployeeExistsAsync(userId);
                employeeEntityExist.Should().BeFalse();
            }
        }

        [Fact]
        public async Task Put_user_add_claims_roles_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10016;

            var updateDto = new UserUpdateDto
            {
                Name = "Har Mónika3",
                Email = "hm3@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { 10000, 10004, 10002 },
                OwnedRolesIds = new List<int>() { 10003, 10004 },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                var preActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .Include(ent => ent.Roles)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);
                preActUser.Should().NotBeNull();
                preActUser.Claims.Should().BeEmpty();
                preActUser.Roles.Should().BeEmpty();

                // Act
                await service.UpdateUserAsync(userId, updateDto);
            }

            var client2 = factory.CreateClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();

                // Assert
                var postActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .Include(ent => ent.Roles)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);

                postActUser.Should().NotBeNull();
                postActUser.Claims.Should().NotBeEmpty();
                postActUser.Roles.Should().NotBeEmpty();
                postActUser.Claims.Should().HaveCount(3);
                postActUser.Roles.Should().HaveCount(2);

                var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
                var customerEntityExist = await customerRepository.IsCustomerExistsAsync(userId);
                customerEntityExist.Should().BeTrue();

                var salesPersonRepository = scope.ServiceProvider.GetRequiredService<ISalesPersonRepository>();
                var salesPersonEntityExist = await salesPersonRepository.IsSalesPersonExistsAsync(userId);
                salesPersonEntityExist.Should().BeTrue();

                var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
                var employeeEntityExist = await employeeRepository.IsEmployeeExistsAsync(userId);
                employeeEntityExist.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Put_user_change_claims_without_closing_entities_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10017;

            var updateDto = new UserUpdateDto
            {
                Name = "Nemer Eszti3",
                Email = "ne3@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { 10001, 10003, 10005 },
                OwnedRolesIds = new List<int>() { 10007, 10008, 10009 },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                var preActUser = await context.Users
                                    .Include(ent => ent.Roles)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);
                preActUser.Should().NotBeNull();
                preActUser.Roles.Should().NotBeEmpty();
                preActUser.Roles.Should().HaveCount(3);
                preActUser.Roles.Should().Contain(x => x.RoleId == 10002);
                preActUser.Roles.Should().Contain(x => x.RoleId == 10003);
                preActUser.Roles.Should().Contain(x => x.RoleId == 10004);

                var salesPerson = await context.SalesPersons.SingleOrDefaultAsync(x => x.Id == 10006);
                salesPerson.Should().NotBeNull();
                salesPerson.ValidTo.Should().BeNull();

                var customers = await context.Customers.SingleOrDefaultAsync(x => x.Id == 10006);
                customers.Should().NotBeNull();
                customers.ValidTo.Should().BeNull();

                var employees = await context.Employees.SingleOrDefaultAsync(x => x.Id == 10006);
                employees.Should().NotBeNull();
                employees.ValidTo.Should().BeNull();

                // Act
                await service.UpdateUserAsync(userId, updateDto);
            }

            var client2 = factory.CreateClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();

                // Assert
                var postActUser = await context.Users
                                    .Include(ent => ent.Roles)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);

                postActUser.Should().NotBeNull();
                postActUser.Roles.Should().NotBeEmpty();
                postActUser.Roles.Should().HaveCount(3);
                postActUser.Roles.Should().Contain(x => x.RoleId == 10007);
                postActUser.Roles.Should().Contain(x => x.RoleId == 10008);
                postActUser.Roles.Should().Contain(x => x.RoleId == 10009);

                var salesPerson = await context.SalesPersons.SingleOrDefaultAsync(x => x.Id == 10006);
                salesPerson.Should().NotBeNull();
                salesPerson.ValidTo.Should().BeNull();

                var customers = await context.Customers.SingleOrDefaultAsync(x => x.Id == 10006);
                customers.Should().NotBeNull();
                customers.ValidTo.Should().BeNull();

                var employees = await context.Employees.SingleOrDefaultAsync(x => x.Id == 10006);
                employees.Should().NotBeNull();
                employees.ValidTo.Should().BeNull();
            }
        }

        [Fact]
        public async Task Put_user_add_wrong_claims_not_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10016;

            var updateDto = new UserUpdateDto
            {
                Name = "Har Mónika3",
                Email = "hm3@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { 90000, 90004, 90002 },
                OwnedRolesIds = new List<int>() { },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                // Act & Assert
                await Assert.ThrowsAsync<DbUpdateException>(() => service.UpdateUserAsync(userId, updateDto));
            }
        }

        [Fact]
        public async Task Put_user_add_wrong_roles_not_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10016;

            var updateDto = new UserUpdateDto
            {
                Name = "Har Mónika3",
                Email = "hm3@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { },
                OwnedRolesIds = new List<int>() { 90000, 90004, 90002 },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                // Act & Assert
                await service.UpdateUserAsync(userId, updateDto);
                var user = await context.Users.SingleAsync(ent => ent.Id == userId);
                user.Roles.Should().HaveCount(0);
            }
        }

        [Fact]
        public async Task Put_user_add_company_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10013;

            var updateDto = new UserUpdateDto
            {
                Name = "Har Mónika",
                Email = "hm@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = 10003,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { },
                OwnedRolesIds = new List<int>() { },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                /* TODO var preActUserCompany = await context.CompanyUsers
                                    .Where(cu => cu.ValidFrom <= Clock.Now
                                        && (!cu.ValidTo.HasValue || cu.ValidTo.Value >= Clock.Now))
                                    .Where(cu => userId == cu.UserId)
                                    .SingleOrDefaultAsync();

                preActUserCompany.Should().BeNull();*/

                // Act
                await service.UpdateUserAsync(userId, updateDto);

                // Assert
                var postActUserCompany = context.Users.Single(u => u.Id == userId).CompanyId;

                postActUserCompany.Should().NotBeNull();
                postActUserCompany.Should().Equals(10003);
            }
        }

        [Fact]
        public async Task Put_user_change_company_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10014;

            var updateDto = new UserUpdateDto
            {
                Name = "Har Mónika1",
                Email = "hm1@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = 10003,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { },
                OwnedRolesIds = new List<int>() { },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                /* TODO var preActUserCompany = await context.CompanyUsers
                                    .Where(cu => cu.ValidFrom <= Clock.Now
                                        && (!cu.ValidTo.HasValue || cu.ValidTo.Value >= Clock.Now))
                                    .Where(cu => userId == cu.UserId)
                                    .SingleOrDefaultAsync();

                preActUserCompany.Should().NotBeNull();
                preActUserCompany.UserId.Should().Equals(userId);
                preActUserCompany.CompanyId.Should().Equals(10000);*/

                // Act
                await service.UpdateUserAsync(userId, updateDto);

                // Assert
                var postActUserCompany = context.Users.Single(ent => ent.Id == userId).CompanyId;

                postActUserCompany.Should().NotBeNull();
                postActUserCompany.Should().Equals(10003);
            }
        }

        [Fact]
        public async Task Put_user_remove_company_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10015;

            var updateDto = new UserUpdateDto
            {
                Name = "Har Mónika2",
                Email = "hm2@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { },
                OwnedRolesIds = new List<int>() { },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                var preActUserCompany = context.Users.Single(ent => ent.Id == userId).CompanyId;

                preActUserCompany.Should().NotBeNull();
                preActUserCompany.Should().Equals(10000);

                // Act
                await service.UpdateUserAsync(userId, updateDto);

                // Assert
                var postActUserCompany = context.Users.Single(ent => ent.Id == userId).CompanyId;

                postActUserCompany.Should().BeNull();
            }
        }

        [Fact]
        public async Task Put_user_SalesPerson_clear_datas_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10018;
            var spId = 10007;

            var updateDto = new UserUpdateDto
            {
                Name = "Nemer Eszti4",
                Email = "ne4@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { 10002 },
                OwnedRolesIds = new List<int>() { 10004 },
                WorkingInfo = new UpdateWorkingInfoDto
                {
                    MaxDiscountPercent = 0,
                    MinDiscountPercent = 0,
                    OfficeIds = new List<int>(),
                    SupervisorUserId = null,
                    WorkingHours = new List<WorkingHourDto>()
                    {
                    },
                },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                var preActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);
                preActUser.Should().NotBeNull();
                preActUser.Claims.Should().NotBeEmpty();
                preActUser.Claims.Should().HaveCount(1);
                preActUser.Claims.Should().Contain(x => x.ClaimId == 10002);

                var salesPerson = await context.SalesPersons
                    .Include(x => x.Offices)
                    .Include(x => x.DefaultTimeTable)
                    .SingleOrDefaultAsync(x => x.Id == spId);
                salesPerson.Should().NotBeNull();
                salesPerson.ValidTo.Should().BeNull();
                salesPerson.DefaultTimeTable.Should().HaveCount(6);
                salesPerson.Offices.Should().HaveCount(1);
                salesPerson.Offices.Should().Contain(x => x.OfficeId == 1);
                salesPerson.SupervisorId.Should().Equals(10006);
                salesPerson.UserId.Should().Equals(userId);

                // Act
                await service.UpdateUserAsync(userId, updateDto);
            }

            var client2 = factory.CreateClient();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();

                // Assert
                var postActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);

                postActUser.Should().NotBeNull();
                postActUser.Claims.Should().NotBeEmpty();
                postActUser.Claims.Should().HaveCount(1);
                postActUser.Claims.Should().Contain(x => x.ClaimId == 10002);

                var salesPerson = await context.SalesPersons
                    .Include(x => x.Offices)
                    .Include(x => x.DefaultTimeTable)
                    .SingleOrDefaultAsync(x => x.Id == spId);
                salesPerson.Should().NotBeNull();
                salesPerson.ValidTo.Should().BeNull();
                salesPerson.DefaultTimeTable.Should().BeEmpty();
                salesPerson.Offices.Should().BeEmpty();
                salesPerson.SupervisorId.Should().BeNull();
                salesPerson.UserId.Should().Equals(userId);
            }
        }

        [Fact]
        public async Task Put_user_SalesPerson_modify_datas_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10019;
            var spId = 10008;

            var updateDto = new UserUpdateDto
            {
                Name = "Nemer Eszti5",
                Email = "ne5@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { 10002 },
                OwnedRolesIds = new List<int>() { 10004 },
                WorkingInfo = new UpdateWorkingInfoDto
                {
                    MaxDiscountPercent = 5,
                    MinDiscountPercent = 25,
                    OfficeIds = new List<int>() { 2 },
                    SupervisorUserId = 10007,
                    WorkingHours = new List<WorkingHourDto>(){
                        new WorkingHourDto()
                              {
                                 DayTypeId = 1,
                                 From = new DateTime(0001,01,01,08,00,00),
                                 Id = 10010,
                                 To = new DateTime(0001,01,01,16,30,00),
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 2,
                                 From = new DateTime(0001,01,01,9,00,00),
                                 Id = 10012,
                                 To = new DateTime(0001,01,01,18,00,00)
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 3,
                                 From = new DateTime(0001,01,01,6,00,00),
                                 To = new DateTime(0001,01,01,12,00,00)
                              },
                        }
                },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                var preActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);
                preActUser.Should().NotBeNull();
                preActUser.Claims.Should().NotBeEmpty();
                preActUser.Claims.Should().HaveCount(1);
                preActUser.Claims.Should().Contain(x => x.ClaimId == 10002);

                var salesPerson = await context.SalesPersons
                    .Include(x => x.Offices)
                    .Include(x => x.DefaultTimeTable)
                    .SingleOrDefaultAsync(x => x.Id == spId);
                salesPerson.Should().NotBeNull();
                salesPerson.ValidTo.Should().BeNull();
                salesPerson.DefaultTimeTable.Should().HaveCount(6);
                salesPerson.Offices.Should().HaveCount(1);
                salesPerson.Offices.Should().Contain(x => x.OfficeId == 1);
                salesPerson.SupervisorId.Should().Equals(10006);
                salesPerson.MinDiscount.Should().Equals(1);
                salesPerson.MaxDiscount.Should().Equals(15);
                salesPerson.UserId.Should().Equals(userId);

                // Act
                await service.UpdateUserAsync(userId, updateDto);
            }

            var client2 = factory.CreateClient();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();

                // Assert
                var postActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);

                postActUser.Should().NotBeNull();
                postActUser.Claims.Should().NotBeEmpty();
                postActUser.Claims.Should().HaveCount(1);
                postActUser.Claims.Should().Contain(x => x.ClaimId == 10002);

                var salesPerson = await context.SalesPersons
                    .Include(x => x.Offices)
                    .Include(x => x.DefaultTimeTable)
                    .SingleOrDefaultAsync(x => x.Id == spId);
                salesPerson.Should().NotBeNull();
                salesPerson.ValidTo.Should().BeNull();
                salesPerson.DefaultTimeTable.Should().HaveCount(3);
                salesPerson.DefaultTimeTable.Should().Contain(x => x.Id == 10010 && x.DayTypeId == 1
                    && x.Interval.From.TimeOfDay == new TimeSpan(8, 0, 0) && x.Interval.To.TimeOfDay == new TimeSpan(16, 30, 0));
                salesPerson.DefaultTimeTable.Should().Contain(x => x.Id == 10012 && x.DayTypeId == 2
                    && x.Interval.From.TimeOfDay == new TimeSpan(9, 0, 0) && x.Interval.To.TimeOfDay == new TimeSpan(18, 00, 0));
                salesPerson.DefaultTimeTable.Should().Contain(x => x.DayTypeId == 3
                    && x.Interval.From.TimeOfDay == new TimeSpan(6, 0, 0) && x.Interval.To.TimeOfDay == new TimeSpan(12, 0, 0));
                salesPerson.Offices.Should().HaveCount(1);
                salesPerson.Offices.Should().Contain(x => x.OfficeId == 2);
                salesPerson.SupervisorId.Should().Equals(10007);
                salesPerson.MinDiscount.Should().Equals(5);
                salesPerson.MaxDiscount.Should().Equals(25);
                salesPerson.UserId.Should().Equals(userId);
            }
        }

        [Fact]
        public async Task Put_user_SalesPerson_add_claim_and_datas_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10020;

            var updateDto = new UserUpdateDto
            {
                Name = "Har Mónika4",
                Email = "hm4@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { 10002 },
                OwnedRolesIds = new List<int>() { 10004 },
                WorkingInfo = new UpdateWorkingInfoDto
                {
                    MaxDiscountPercent = 5,
                    MinDiscountPercent = 25,
                    OfficeIds = new List<int>() { 2 },
                    SupervisorUserId = 10007,
                    WorkingHours = new List<WorkingHourDto>(){
                        new WorkingHourDto()
                              {
                                 DayTypeId = 1,
                                 From = new DateTime(0001,01,01,08,00,00),
                                 To = new DateTime(0001,01,01,16,30,00),
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 2,
                                 From = new DateTime(0001,01,01,9,00,00),
                                 To = new DateTime(0001,01,01,18,00,00)
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 3,
                                 From = new DateTime(0001,01,01,6,00,00),
                                 To = new DateTime(0001,01,01,12,00,00)
                              },
                        }
                },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                var salesPersonRepository = scope.ServiceProvider.GetRequiredService<ISalesPersonRepository>();

                var preActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);
                preActUser.Should().NotBeNull();
                preActUser.Claims.Should().BeEmpty();

                var isSalesPersonEntityExists = await salesPersonRepository.IsSalesPersonExistsAsync(userId);
                isSalesPersonEntityExists.Should().BeFalse();
                await Assert.ThrowsAsync<EntityNotFoundException>(() => salesPersonRepository.GetByUserIdWithOfficesSupervisorsSuperviseesAsync(userId));

                // Act
                await service.UpdateUserAsync(userId, updateDto);
            }

            var client2 = factory.CreateClient();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var salesPersonRepository = scope.ServiceProvider.GetRequiredService<ISalesPersonRepository>();

                // Assert
                var postActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);

                postActUser.Should().NotBeNull();
                postActUser.Claims.Should().NotBeEmpty();
                postActUser.Claims.Should().HaveCount(1);
                postActUser.Claims.Should().Contain(x => x.ClaimId == 10002);

                var isSalesPersonEntityExists = await salesPersonRepository.IsSalesPersonExistsAsync(userId);
                isSalesPersonEntityExists.Should().BeTrue();

                var salesPerson = await salesPersonRepository.GetByUserIdWithOfficesSupervisorsSuperviseesAsync(userId);
                salesPerson.Should().NotBeNull();
                salesPerson.ValidTo.Should().BeNull();
                salesPerson.DefaultTimeTable.Should().HaveCount(3);
                salesPerson.DefaultTimeTable.Should().Contain(x => x.DayTypeId == 1
                    && x.Interval.From.TimeOfDay == new TimeSpan(8, 0, 0) && x.Interval.To.TimeOfDay == new TimeSpan(16, 30, 0));
                salesPerson.DefaultTimeTable.Should().Contain(x => x.DayTypeId == 2
                    && x.Interval.From.TimeOfDay == new TimeSpan(9, 0, 0) && x.Interval.To.TimeOfDay == new TimeSpan(18, 00, 0));
                salesPerson.DefaultTimeTable.Should().Contain(x => x.DayTypeId == 3
                    && x.Interval.From.TimeOfDay == new TimeSpan(6, 0, 0) && x.Interval.To.TimeOfDay == new TimeSpan(12, 0, 0));
                salesPerson.Offices.Should().HaveCount(1);
                salesPerson.Offices.Should().Contain(x => x.OfficeId == 2);
                salesPerson.SupervisorId.Should().Equals(10007);
                salesPerson.MinDiscount.Should().Equals(5);
                salesPerson.MaxDiscount.Should().Equals(25);
                salesPerson.UserId.Should().Equals(userId);
            }
        }

        [Fact]
        public async Task Put_user_SalesPerson_add_wrong_office_not_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10020;

            var updateDto = new UserUpdateDto
            {
                Name = "Har Mónika4",
                Email = "hm4@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { 10002 },
                OwnedRolesIds = new List<int>() { 10004},
                WorkingInfo = new UpdateWorkingInfoDto
                {
                    MaxDiscountPercent = 5,
                    MinDiscountPercent = 25,
                    OfficeIds = new List<int>() { 99999 },
                    SupervisorUserId = 10007,
                    WorkingHours = new List<WorkingHourDto>(){
                        new WorkingHourDto()
                              {
                                 DayTypeId = 1,
                                 From = new DateTime(0001,01,01,08,00,00),
                                 To = new DateTime(0001,01,01,16,30,00),
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 2,
                                 From = new DateTime(0001,01,01,9,00,00),
                                 To = new DateTime(0001,01,01,18,00,00)
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 3,
                                 From = new DateTime(0001,01,01,6,00,00),
                                 To = new DateTime(0001,01,01,12,00,00)
                              },
                        }
                },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                // Act && Assert
                await Assert.ThrowsAsync<EntityNotFoundException>(() => service.UpdateUserAsync(userId, updateDto));
            }
        }

        [Fact]
        public async Task Put_user_SalesPerson_add_wrong_supervisor_not_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10020;

            var updateDto = new UserUpdateDto
            {
                Name = "Har Mónika4",
                Email = "hm4@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { 10002 },
                OwnedRolesIds = new List<int>() { 10004 },
                WorkingInfo = new UpdateWorkingInfoDto
                {
                    MaxDiscountPercent = 5,
                    MinDiscountPercent = 25,
                    OfficeIds = new List<int>() { 1 },
                    SupervisorUserId = 999999,
                    WorkingHours = new List<WorkingHourDto>(){
                        new WorkingHourDto()
                              {
                                 DayTypeId = 1,
                                 From = new DateTime(0001,01,01,08,00,00),
                                 To = new DateTime(0001,01,01,16,30,00),
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 2,
                                 From = new DateTime(0001,01,01,9,00,00),
                                 To = new DateTime(0001,01,01,18,00,00)
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 3,
                                 From = new DateTime(0001,01,01,6,00,00),
                                 To = new DateTime(0001,01,01,12,00,00)
                              },
                        }
                },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                // Act && Assert
                await Assert.ThrowsAsync<EntityNotFoundException>(() => service.UpdateUserAsync(userId, updateDto));
            }
        }

        [Fact]
        public async Task Put_user_SalesPerson_add_working_hour_wrongly_not_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10020;

            var updateDto = new UserUpdateDto
            {
                Name = "Har Mónika4",
                Email = "hm4@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { 10002 },
                OwnedRolesIds = new List<int>() { 10004 },
                WorkingInfo = new UpdateWorkingInfoDto
                {
                    MaxDiscountPercent = 5,
                    MinDiscountPercent = 25,
                    OfficeIds = new List<int>() { 1 },
                    SupervisorUserId = 10007,
                    WorkingHours = new List<WorkingHourDto>(){
                        new WorkingHourDto()
                              {
                                 DayTypeId = 1,
                                 From = new DateTime(0001,01,01,08,00,00),
                                 To = new DateTime(0001,01,01,16,30,00),
                                 Id = 10020,
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 2,
                                 From = new DateTime(0001,01,01,9,00,00),
                                 To = new DateTime(0001,01,01,18,00,00)
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 3,
                                 From = new DateTime(0001,01,01,6,00,00),
                                 To = new DateTime(0001,01,01,12,00,00)
                              },
                        }
                },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                // Act && Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateUserAsync(userId, updateDto));
            }
        }

        [Fact]
        public async Task Put_user_SalesPerson_update_working_hour_wrongly_not_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10019;

            var updateDto = new UserUpdateDto
            {
                Name = "Nemer Eszti5",
                Email = "ne5@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { 10002 },
                OwnedRolesIds = new List<int>() { 10004 },
                WorkingInfo = new UpdateWorkingInfoDto
                {
                    MaxDiscountPercent = 5,
                    MinDiscountPercent = 25,
                    OfficeIds = new List<int>() { 2 },
                    SupervisorUserId = 10007,
                    WorkingHours = new List<WorkingHourDto>(){
                        new WorkingHourDto()
                              {
                                 DayTypeId = 1,
                                 From = new DateTime(0001,01,01,08,00,00),
                                 Id = 10010,
                                 To = new DateTime(0001,01,01,16,30,00),
                              },
                        new WorkingHourDto
                              {
                                 DayTypeId = 5,
                                 From = new DateTime(0001,01,01,9,00,00),
                                 Id = 10012,
                                 To = new DateTime(0001,01,01,18,00,00)
                              },
                        }
                },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                // Act && Assert
                await Assert.ThrowsAsync<IFPSDomainException>(() => service.UpdateUserAsync(userId, updateDto));
            }
        }

        [Fact]
        public async Task Put_user_Customer_clear_datas_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10021;
            var customerId = 10009;

            var updateDto = new UserUpdateDto
            {
                Name = "Nemer Eszti6",
                Email = "ne6@enco.hu",
                PhoneNumber = "",
                Address = new AddressCreateDto()
                {
                    CountryId = 1,
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Seholse utca 1.",
                },
                CompanyId = null,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>() { 10004 },
                OwnedRolesIds = new List<int>() { 10003 },
                Notifications = new NotificationsDto
                {
                    EventTypeIds = new List<int>() { },
                    NotificationTypeFlags = new List<string>() { },
                },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                var preActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);
                preActUser.Should().NotBeNull();
                preActUser.Claims.Should().NotBeEmpty();
                preActUser.Claims.Should().HaveCount(1);
                preActUser.Claims.Should().Contain(x => x.ClaimId == 10004);

                var customer = await context.Customers
                    .Include(x => x.NotificationModes)
                    .SingleOrDefaultAsync(x => x.Id == customerId);
                customer.Should().NotBeNull();
                customer.ValidTo.Should().BeNull();
                customer.UserId.Should().Equals(userId);
                customer.NotificationModes.Should().HaveCount(4);
                customer.NotificationModes.Should().Contain(x => x.EventTypeId == 1);
                customer.NotificationModes.Should().Contain(x => x.EventTypeId == 2);
                customer.NotificationModes.Should().Contain(x => x.EventTypeId == 3);
                customer.NotificationModes.Should().Contain(x => x.EventTypeId == 4);
                customer.NotificationType.Should().Equals(NotificationTypeFlag.Email | NotificationTypeFlag.SMS);

                // Act
                await service.UpdateUserAsync(userId, updateDto);
            }

            var client2 = factory.CreateClient();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();

                // Assert
                var postActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);

                postActUser.Should().NotBeNull();
                postActUser.Claims.Should().NotBeEmpty();
                postActUser.Claims.Should().HaveCount(1);
                postActUser.Claims.Should().Contain(x => x.ClaimId == 10004);

                var customer = await context.Customers
                    .Include(x => x.NotificationModes)
                    .SingleOrDefaultAsync(x => x.Id == customerId);
                customer.Should().NotBeNull();
                customer.ValidTo.Should().BeNull();
                customer.UserId.Should().Equals(userId);
                customer.NotificationModes.Should().BeEmpty();
                customer.NotificationType.Should().Equals(NotificationTypeFlag.None);
            }
        }

        #endregion Update User Test

        #region Search User

        [Fact]
        public async Task Search_user_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var name = "eszti";

            // Act
            var resp = await client.GetAsync($"api/users/search?name={name}&salesPersonOnly=false&take=10");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var res = JsonConvert.DeserializeObject<List<UserDto>>(stringresp, jsonSerializerSettings);
            res.Should().NotBeEmpty();
            res.Should().HaveCountLessOrEqualTo(10);
            res.Should().NotContain(x => !x.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task Search_user_take_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var name = "eszti";

            // Act
            var resp = await client.GetAsync($"api/users/search?name={name}&userNumber=2");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var res = JsonConvert.DeserializeObject<List<UserDto>>(stringresp, jsonSerializerSettings);
            res.Should().NotBeEmpty();
            res.Should().HaveCountLessOrEqualTo(2);
            res.Should().NotContain(x => !x.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task Search_user_only_sales_persons_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var name = "Ebéd Elek";

            // Act
            var resp = await client.GetAsync($"api/users/search?name={name}&divisionType=Sales&userNumber=2");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var res = JsonConvert.DeserializeObject<List<UserDto>>(stringresp, jsonSerializerSettings);
            res.Should().BeEmpty();
        }

        #endregion Search User

        #region Build Entities
        private static UserCreateDto CreateUser()
        {
            return new UserCreateDto
            {
                Name = "Bor_Virag",
                Email = "bv@enco.eu",
                PhoneNumber = "",
                RoleId = 10005,
                Password = "Password1."
            };
        }

        #endregion
    }
}