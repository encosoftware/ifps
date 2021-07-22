using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.EF;
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

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class UsersTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public UsersTests(IFPSFactoryWebApplicationFactory factory)
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
            var expectedResult = new List<User>() { GetUser2(), GetUser3() };

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 2
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);
            //result.Data[1].AddRole("Financial Expert");

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
            var expectedResult = new List<User>() { GetUser2() };

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
                UserFilterDto userFilterDto = new UserFilterDto() { Name = "Mihail" };
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
            var expectedResult = new List<User>() { GetUser3() };

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);
            //result.Data[0].AddRole("Financial Expert");

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { PhoneNumber = "+198419841984" };
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
            var expectedResult = new List<User>() { GetUser3() };

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);
            //result.Data[0].AddRole("Financial Expert");

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { CompanyName = "Machine Inc." };
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
            var expectedResult = new List<User>() { GetUser3() };

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);
            //result.Data[0].AddRole("Financial Expert");

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { RoleId = 10005 };
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
                users.Data.Count().Should().BeGreaterOrEqualTo(2);
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

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto() { AddedOnFrom = new DateTime(2019, 04, 01) };
                var users = await service.GetUsersAsync(userFilterDto);

                // Assert
                users.Data.Count().Should().BeGreaterOrEqualTo(2);
            }
        }

        [Fact]
        public async Task Get_users_filter_AddedOn_Range_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<User>() { GetUser2(), GetUser3() };

            PagedList<User> pagedList = new PagedList<User>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 2
            };
            var result = pagedList.ToPagedList(UserListDto.FromEntity);
            //result.Data[1].AddRole("Financial Expert");

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                UserFilterDto userFilterDto = new UserFilterDto()
                {
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
        public async Task Post_user_with_production_role_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var createDto = new UserCreateDto
            {
                Name = "Csik_Lorant",
                Email = "csl@enco.eu",
                PhoneNumber = "",
                RoleId = 10002,
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
                var userAppService = scope.ServiceProvider.GetRequiredService<IUserAppService>();
                //var customerEntityExist = await userAppService.SearchUserAsync();

                //customerEntityExist.Should().BeTrue();
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

            var userId = 10001;

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

            var createDto = new UserCreateDto
            {
                Name = "Bor_Virag",
                Email = "bv@enco.eu",
                PhoneNumber = "",
                RoleId = 10005,
                Password = "Password1"
            };
            var content = new StringContent(JsonConvert.SerializeObject(createDto), Encoding.UTF8, "application/json");
            var createResp = await client.PostAsync("api/users", content);
            createResp.EnsureSuccessStatusCode();
            var userId = int.Parse(await createResp.Content.ReadAsStringAsync());

            // Act
            var resp = await client.DeleteAsync($"api/users/{userId}");

            // Assert
            resp.EnsureSuccessStatusCode();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
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
        public async Task Get_user_all_details_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var userId = 10006;
            var expectedResult = new UserDetailsDto
            {
                Id = userId,
                Name = "George Orwell",
                Email = "george@enco.test.hu",
                PhoneNumber = "+198419841984",
                Address = new AddressDetailsDto(new Address(1076, "Budapest", "Garay tér 2.", 1)),
                Company = new CompanyDto { Id = 10004, Name = "Machine Inc." },
                IsActive = true,
                IsEmployee = false,
                Language = LanguageTypeEnum.EN,
                OwnedClaimsIds = new List<int>() { },
                OwnedRolesIds = new List<int>() { 10005 }
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

            var userId = 10007;

            var preActResult = new UserDetailsDto
            {
                Id = userId,
                Name = "Para Zita",
                Email = "pa@enco.hu",
                PhoneNumber = "06301234567",
                Address = new AddressDetailsDto(new Address(1117, "Budapest", "Seholse utca 1.", 1)),
                Company = new CompanyDto { Id = 10002, Name = "Super Supplier Company" },
                IsActive = true,
                IsEmployee = false,
                Language = LanguageTypeEnum.HU,
                OwnedClaimsIds = new List<int>(),
                OwnedRolesIds = new List<int>() { 10000 }
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
                Company = new CompanyDto { Id = 10001, Name = "Test Super Supplier Company" },
                IsActive = true,
                IsEmployee = true,
                Language = LanguageTypeEnum.EN,
                OwnedClaimsIds = new List<int>() { 10000 },
                OwnedRolesIds = new List<int>() { 10006 }
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

            var userId = 10013;

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
                var context = scope.ServiceProvider.GetRequiredService<IFPSFactoryContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                var preActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .Include(ent => ent.Roles)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);
                preActUser.Should().NotBeNull();
                preActUser.Claims.Should().HaveCount(3);

                // Act
                await service.UpdateUserAsync(userId, updateDto);
            }

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSFactoryContext>();
                // Assert
                var postActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .Include(ent => ent.Roles)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);

                postActUser.Should().NotBeNull();
                postActUser.Claims.Where(c => !c.IsDeleted).Should().BeEmpty();
                postActUser.Roles.Where(r => !r.IsDeleted).Should().BeEmpty();
            }
        }

        [Fact]
        public async Task Put_user_add_claims_roles_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10011;

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
                OwnedRolesIds = new List<int>() { 10003, 10004, 10000 },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSFactoryContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                var preActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .Include(ent => ent.Roles)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);

                preActUser.Should().NotBeNull();
                preActUser.Claims.Should().BeEmpty();
                preActUser.Roles.Should().HaveCount(1);

                // Act
                await service.UpdateUserAsync(userId, updateDto);
            }

            var client2 = factory.CreateClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSFactoryContext>();

                // Assert
                var postActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .Include(ent => ent.Roles)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);

                postActUser.Should().NotBeNull();
                postActUser.Claims.Should().NotBeEmpty();
                postActUser.Roles.Should().NotBeEmpty();
                postActUser.Claims.Should().HaveCount(3);
                postActUser.Roles.Should().HaveCount(3);

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

            var userId = 10013;

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
                OwnedRolesIds = new List<int>() { },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSFactoryContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                var preActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);
                preActUser.Should().NotBeNull();
                preActUser.Claims.Should().NotBeEmpty();
                preActUser.Claims.Should().HaveCount(3);
                preActUser.Claims.Should().Contain(x => x.ClaimId == 10000);
                preActUser.Claims.Should().Contain(x => x.ClaimId == 10002);
                preActUser.Claims.Should().Contain(x => x.ClaimId == 10004);

                // Act
                await service.UpdateUserAsync(userId, updateDto);
            }

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSFactoryContext>();

                // Assert
                var postActUser = await context.Users
                                    .Include(ent => ent.Claims)
                                    .SingleOrDefaultAsync(ent => ent.Id == userId);

                postActUser.Should().NotBeNull();
                postActUser.Claims.Should().NotBeEmpty();
                postActUser.Claims.Should().HaveCount(3);
                postActUser.Claims.Should().Contain(x => x.ClaimId == 10001);
                postActUser.Claims.Should().Contain(x => x.ClaimId == 10003);
                postActUser.Claims.Should().Contain(x => x.ClaimId == 10005);
            }
        }

        [Fact]
        public async Task Put_user_add_wrong_claims_not_works()
        {
            // Arrange
            var client = factory.CreateClient();

            var userId = 10011;

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
                OwnedRolesIds = new List<int>() { 10003, 10004 },
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

            var userId = 10012;

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
                var context = scope.ServiceProvider.GetRequiredService<IFPSFactoryContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

                // Act & Assert
                await service.UpdateUserAsync(userId, updateDto);
                var user = await context.Users.SingleAsync(ent => ent.Id == userId);
                user.Roles.Should().HaveCount(1);
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
                OwnedClaimsIds = new List<int>() { 10000, 10002, 10004 },
                OwnedRolesIds = new List<int>() { },
            };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSFactoryContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

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

            var userId = 10009;

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
                var context = scope.ServiceProvider.GetRequiredService<IFPSFactoryContext>();
                var service = scope.ServiceProvider.GetRequiredService<IUserAppService>();

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

            var userId = 10010;

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
                var context = scope.ServiceProvider.GetRequiredService<IFPSFactoryContext>();
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

        #endregion Update User Test

        #region Search User

        [Fact]
        public async Task Search_user_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var name = "Yevgeny";

            // Act
            var resp = await client.GetAsync($"api/users/search?name={name}&take=10");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var res = JsonConvert.DeserializeObject<List<UserDto>>(stringresp, jsonSerializerSettings);
            res.Should().NotBeEmpty();
            res.Should().HaveCountLessOrEqualTo(10);
        }

        [Fact]
        public async Task Search_user_take_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var name = "Yevgeny";

            // Act
            var resp = await client.GetAsync($"api/users/search?name={name}&take=2");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var res = JsonConvert.DeserializeObject<List<UserDto>>(stringresp, jsonSerializerSettings);
            res.Should().NotBeEmpty();
            res.Should().HaveCountLessOrEqualTo(2);
        }

        [Fact]
        public async Task Search_user_only_production_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var name = "Ebéd Elek";

            // Act
            var resp = await client.GetAsync($"api/users/search?name={name}&divisionType=Production&take=2");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var res = JsonConvert.DeserializeObject<List<UserDto>>(stringresp, jsonSerializerSettings);
            res.Should().BeEmpty();
        }

        #endregion Search User

        public static Role GetAdminRole()
        {
            var adminRole = new Role("Admin", divisionId: 10000) { Id = 10000 };
            adminRole.AddTranslation(GetAdminRoleTranslation().First());
            adminRole.AddTranslation(GetAdminRoleTranslation().Last());
            return adminRole;
        }

        public static List<RoleTranslation> GetAdminRoleTranslation()
        {
            return new List<RoleTranslation>()
            {
                new RoleTranslation(10000, "Admin", LanguageTypeEnum.EN) { Id = 10000 },
                new RoleTranslation(10000, "Rendszergazda", LanguageTypeEnum.HU) { Id = 10001 }
            };
        }

        public static User GetUser2()
        {
            var user2 = new User("contact@enco.test.hu") { Id = 10004, CreationTime = new DateTime(2019, 05, 10) };
            user2.AddVersion(new UserData("Mihail Bulgakov", "+875345434", Clock.Now, new Address(5000, "Szolnok", "Kossuth tér 3.", 1)) { Id = 10002 });
            //user2.AddRoles(new List<int> { GetAdminRole().Id/*10002*/ });
            user2.AddRoles(new List<Role> { GetAdminRole() });
            return user2;
        }

        public static Role GetFinancialExpertRole()
        {
            var financialExpertRole = new Role("Financial Expert", divisionId: 10000) { Id = 10000 };
            financialExpertRole.AddTranslation(GetFinancialExpertRoleTranslation().First());
            financialExpertRole.AddTranslation(GetFinancialExpertRoleTranslation().Last());
            return financialExpertRole;
        }

        public static List<RoleTranslation> GetFinancialExpertRoleTranslation()
        {
            return new List<RoleTranslation>()
            {
                new RoleTranslation(10005, "Financial Expert", LanguageTypeEnum.EN) { Id = 10010 },
                new RoleTranslation(10005, "Pénzügyi főmufti", LanguageTypeEnum.HU) { Id = 10011 }
            };
        }

        public static User GetUser3()
        {
            var user3 = new User("george@enco.test.hu") { Id = 10006, CreationTime = new DateTime(2019, 05, 10), Company = new Company("Machine Inc.", 10001) };
            user3.AddVersion(new UserData("George Orwell", "+198419841984", Clock.Now, new Address(1076, "Budapest", "Garay tér 2.", 1)) { Id = 10004 });
            //user3.AddRoles(new List<int> { 10005 });
            user3.AddRoles(new List<Role> { GetFinancialExpertRole() });
            return user3;
        }
    }
}