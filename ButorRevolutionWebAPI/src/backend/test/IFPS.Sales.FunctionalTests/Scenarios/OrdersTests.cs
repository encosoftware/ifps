using ENCO.DDD.Application.Dto;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Seed;
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
using Xunit.Abstractions;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class OrdersTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;
        private readonly ITestOutputHelper output;

        public OrdersTests(IFPSSalesWebApplicationFactory factory, ITestOutputHelper output)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
            };
            this.output = output;
        }

        private async Task<string> getCustomerAccessToken()
        {
            var loginDto = new LoginDto()
            {
                Email = "beviz.elek@envotest.hu",
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

        private async Task<string> getCustomer2AccessToken()
        {
            var loginDto = new LoginDto()
            {
                Email = "kunden@kunden.de",
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

        private async Task<string> getCustomer3AccessToken()
        {
            var loginDto = new LoginDto()
            {
                Email = "approve@encotest.hu",
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

        private async Task<string> processResponse(HttpResponseMessage response)
        {
            var stringresp = await response.Content.ReadAsStringAsync();
            output.WriteLine(stringresp + "\n");
            return stringresp;
        }

        #region Order listing
        [Fact]
        public async Task Get_order_success()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var resp = await client.GetAsync("api/orders");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().BeGreaterThan(0);
        }
        [Fact]
        public async Task Get_order_filter_orderId_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = "Teszt or";
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.OrderId)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(1);
            responseContent.Data.First().OrderName.Should().Contain(filterValue);
        }
        [Fact]
        public async Task Get_order_filter_wrong_orderId_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var filterValue = "xxx";
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.OrderId)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(0);
        }
        [Fact]
        public async Task Get_order_filter_working_number_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = "MSZ0888";
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.WorkingNumber)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(1);
            responseContent.Data.First().WorkingNumber.Should().Contain(filterValue);
        }
        [Fact]
        public async Task Get_order_filter_wrong_workingnumber_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = "MXXX";
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.WorkingNumber)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(0);
        }
        [Fact]
        public async Task Get_order_filter_statusId_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = "3";
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.CurrentStatusId)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().BeGreaterThan(0);
            //responseContent.Data.All(x => x.CurrentStatus.Equals == OrderStateTranslationSeed.WaitingForFirstPayment).Should().BeTrue();
        }
        [Fact]
        public async Task Get_order_filter_wrong_statusId_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = "16";
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.CurrentStatusId)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(0);
        }
        
        [Fact]
        public async Task Get_order_filter_status_deadline_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValueFrom = Clock.Now.Date;
            var filterValueTo = Clock.Now.AddDays(7).Date;
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.StatusDeadlineFrom)}={filterValueFrom}&{nameof(OrderFilterDto.StatusDeadlineTo)}={filterValueTo}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().BeGreaterThan(0);
            responseContent.Data.All(x => x.StatusDeadline >= filterValueFrom && x.StatusDeadline <= filterValueTo).Should().BeTrue();
        }
        [Fact]
        public async Task Get_order_filter_wrong_status_deadline_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = new DateTime(2050, 01, 01);
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.StatusDeadlineFrom)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(0);
        }
        
        //[Fact]
        //public async Task Get_order_filter_responsible_works()
        //{
        //    // Arrange
        //    var client = factory.CreateClient();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

        //    // Act
        //    var filterValue = "ADMIN";
        //    var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.Responsible)}={filterValue}");

        //    // Assert
        //    var stringresp = await processResponse(resp);
        //    resp.EnsureSuccessStatusCode();
        //    jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        //    var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
        //    responseContent.TotalCount.Should().BeGreaterThan(0);
        //    responseContent.Data.All(x => x.ResponsibleName.ToLower().Contains(filterValue.ToLower())).Should().BeTrue();
        //}
        [Fact]
        public async Task Get_order_filter_wrong_responsible_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = "XXX";
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.Responsible)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(0);
        }
        
        [Fact]
        public async Task Get_order_filter_customer_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = "Ger";
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.Customer)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().BeGreaterThan(0);
            responseContent.TotalCount.Should().Be(2);
            responseContent.Data.All(x => x.CustomerName.ToLower().Contains(filterValue.ToLower())).Should().BeTrue();
        }
        [Fact]
        public async Task Get_order_filter_wrong_customer_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = "XXX";
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.Customer)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(0);
        }
        [Fact]
        public async Task Get_order_filter_sales_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = "doe";
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.Sales)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().BeGreaterThan(0);
            responseContent.TotalCount.Should().Be(25);
            responseContent.Data.All(x => x.SalesName.ToLower().Contains(filterValue)).Should().BeTrue();
        }
        [Fact]
        public async Task Get_order_filter_wrong_sales_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = "XXX";
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.Sales)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(0);
        }
        [Fact]
        public async Task Get_order_filter_created_on_from_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = new DateTime(2019, 06, 01);
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.CreatedOnFrom)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().BeGreaterThan(0);
            responseContent.Data.All(x => x.CreatedOn >= filterValue).Should().BeTrue();
        }
        [Fact]
        public async Task Get_order_filter_wrong_created_on_from_works()
        {

            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = new DateTime(2050, 06, 01);
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.CreatedOnFrom)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(0);
        }
        [Fact]
        public async Task Get_order_filter_created_on_to_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = new DateTime(2050, 06, 01);
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.CreatedOnTo)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().BeGreaterThan(0);
            responseContent.Data.All(x => x.CreatedOn <= filterValue).Should().BeTrue();
        }
        [Fact]
        public async Task Get_order_filter_wrong_created_on_to_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = new DateTime(2010, 06, 01);
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.CreatedOnTo)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(0);
        }
        [Fact]
        public async Task Get_order_filter_deadline_from_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = new DateTime(2019, 06, 01);
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.DeadlineFrom)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().BeGreaterThan(0);
            responseContent.Data.All(x => x.Deadline >= filterValue).Should().BeTrue();
        }
        [Fact]
        public async Task Get_order_filter_wrong_deadline_from_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = new DateTime(2050, 06, 01);
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.DeadlineFrom)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(0);
        }
        [Fact]
        public async Task Get_order_filter_deadline_to_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = new DateTime(2050, 06, 01);
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.DeadlineTo)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().BeGreaterThan(0);
            responseContent.Data.All(x => x.Deadline <= filterValue).Should().BeTrue();
        }
        [Fact]
        public async Task Get_order_filter_wrong_deadline_to_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());


            // Act
            var filterValue = new DateTime(2010, 06, 01);
            var resp = await client.GetAsync($"api/orders?{nameof(OrderFilterDto.DeadlineTo)}={filterValue}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<PagedListDto<OrderListDto>>(stringresp);
            responseContent.TotalCount.Should().Be(0);
        }

        #endregion

        #region Update order
        [Fact]
        public async Task Put_order_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new OrderEditDto
            {
                Deadline = Clock.Now.AddYears(1),
                CustomerUserId = 10017,
                SalesPersonUserId = 10010,
                ShippingAddress = new AddressCreateDto()
                {
                    Address = "AddresssTessst",
                    City = "CitttyTesssst",
                    CountryId = 1,
                    PostCode = 4500,
                },
                AssignedToUserId = 10010,
                CurrentStatusId = 2,
                StatusDeadline = Clock.Now.AddDays(1).Date,
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var orderId = new Guid("1bca75cd-6105-42c2-97b7-af2239c987a2");
            var resp = await client.PutAsync($"api/orders/{orderId}", content);

            // Assert
            await processResponse(resp);
            resp.EnsureSuccessStatusCode();

            var respGet = await client.GetAsync($"api/orders/{orderId}");
            var stringresp = await processResponse(respGet);
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<OrderSalesHeaderDto>(stringresp);
            responseContent.Deadline.Should().Be(dto.Deadline);
            responseContent.Customer.Id.Should().Be(dto.CustomerUserId);
            responseContent.Sales.Id.Should().Be(dto.SalesPersonUserId);
            responseContent.StatusDeadline.Value.Date.Should().Be(dto.StatusDeadline);
            responseContent.CurrentStatus.Id.Should().Be(dto.CurrentStatusId);
        }
        [Fact]
        public async Task Put_order_wrong_customer_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new OrderEditDto
            {
                Deadline = Clock.Now.AddYears(1),
                CustomerUserId = 999,
                SalesPersonUserId = 1,
                ShippingAddress = new AddressCreateDto()
                {
                    Address = "AddresssTessst",
                    City = "CitttyTesssst",
                    CountryId = 1,
                    PostCode = 4500,
                },
                AssignedToUserId = 1,
                CurrentStatusId = 1,
                StatusDeadline = Clock.Now.AddMonths(months: 1),
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var orderId = new Guid("1bca75cd-6105-42c2-97b7-af2239c987a2");
            var resp = await client.PutAsync($"api/orders/{orderId}", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task Put_order_wrong_sales_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new OrderEditDto
            {
                Deadline = Clock.Now.AddYears(1),
                CustomerUserId = 10006,
                SalesPersonUserId = 999,
                ShippingAddress = new AddressCreateDto()
                {
                    Address = "AddresssTessst",
                    City = "CitttyTesssst",
                    CountryId = 1,
                    PostCode = 4500,
                },
                AssignedToUserId = 1,
                CurrentStatusId = 1,
                StatusDeadline = Clock.Now.AddMonths(months: 1),
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var orderId = new Guid("1bca75cd-6105-42c2-97b7-af2239c987a2");
            var resp = await client.PutAsync($"api/orders/{orderId}", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Put_order_wrong_assigned_to_user_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new OrderEditDto
            {
                Deadline = Clock.Now.AddYears(1),
                CustomerUserId = 10006,
                SalesPersonUserId = 1,
                ShippingAddress = new AddressCreateDto()
                {
                    Address = "AddresssTessst",
                    City = "CitttyTesssst",
                    CountryId = 1,
                    PostCode = 4500,
                },
                AssignedToUserId = 999,
                CurrentStatusId = 1,
                StatusDeadline = Clock.Now.AddMonths(months: 1),
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var orderId = new Guid("1bca75cd-6105-42c2-97b7-af2239c987a2");
            var resp = await client.PutAsync($"api/orders/{orderId}", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Put_order_wrong_status_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new OrderEditDto
            {
                Deadline = Clock.Now.AddYears(1),
                CustomerUserId = 10006,
                SalesPersonUserId = 1,
                ShippingAddress = new AddressCreateDto()
                {
                    Address = "AddresssTessst",
                    City = "CitttyTesssst",
                    CountryId = 1,
                    PostCode = 4500,
                },
                AssignedToUserId = 1,
                CurrentStatusId = 999,
                StatusDeadline = Clock.Now.AddMonths(months: 1),
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var orderId = new Guid("1bca75cd-6105-42c2-97b7-af2239c987a2");
            var resp = await client.PutAsync($"api/orders/{orderId}", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Put_order_empty_address_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new OrderEditDto
            {
                Deadline = Clock.Now.AddYears(1),
                CustomerUserId = 1,
                SalesPersonUserId = 1,
                ShippingAddress = null,
                AssignedToUserId = 1,
                CurrentStatusId = 1,
                StatusDeadline = Clock.Now.AddMonths(months: 1),
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var orderId = 10799;
            var resp = await client.PutAsync($"api/orders/{orderId}", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Create order

        [Fact]
        public async Task Post_order_success()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var content = new StringContent(JsonConvert.SerializeObject(BuildOrderDto()), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/orders", content);

            // Assert
            await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsAsync(typeof(Guid));
            stringresp.Should().BeOfType(typeof(Guid));

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var documentGroups = await orderRepository.GetDocumentGroupsAsync((Guid)stringresp, DocumentGroupDto.Projection);

                documentGroups.Should().NotBeEmpty();
                documentGroups.Should().HaveCount(6);
                documentGroups.Where(x => x.IsVersionable == false).All(x => x.Versions.Count() == 1).Should().BeTrue();
                documentGroups.Where(x => x.IsVersionable == false).All(x => x.Versions.First().DocumentState.State == DocumentStateEnum.Empty).Should().BeTrue();
            }
        }

        [Fact]
        public async Task Post_order_wrong_customer_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new OrderCreateDto
            {
                OrderName = "Create Order Test",
                Deadline = new DateTime(2021, 01, 01),
                CustomerUserId = 999,
                SalesPersonUserId = 1,
                ShippingAddress = new AddressCreateDto()
                {
                    Address = "AddresssTessst",
                    City = "CitttyTesssst",
                    CountryId = 1,
                    PostCode = 4500,
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/orders", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_furnitureunit_by_offer_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var foilMaterial = new FoilMaterial("Test-FM-1001") { Id = new Guid("D2CBCC5E-2136-432F-98C2-B37553EFC555"), Description = "Foil unicorn" };
            var content = new StringContent(JsonConvert.SerializeObject(BuildBasefurnitureUnitDto(foilMaterial)), Encoding.UTF8, "application/json");

            //Act
            var resp = await client.PostAsync($"api/orders/{new Guid("FC7DA6DC-9959-4C40-ACD8-A02422FEC76E")}/offers/furnitureunits/{new Guid("74564600-8BD1-4E67-82CC-ED510C974A82")}", content);

            //Assert
            resp.EnsureSuccessStatusCode();

            var stringresp = await resp.Content.ReadAsAsync(typeof(Guid));
            stringresp.Should().BeOfType(typeof(Guid));
        }        

        [Fact]
        public async Task Post_order_wrong_sales_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new OrderCreateDto
            {
                OrderName = "Create Order Test",
                Deadline = new DateTime(2021, 01, 01),
                CustomerUserId = 10006,
                SalesPersonUserId = 999,
                ShippingAddress = new AddressCreateDto()
                {
                    Address = "AddresssTessst",
                    City = "CitttyTesssst",
                    CountryId = 1,
                    PostCode = 4500,
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/orders", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_order_wrong_deadline_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new OrderCreateDto
            {
                OrderName = "Create Order Test",
                Deadline = new DateTime(2018, 01, 01),
                CustomerUserId = 1,
                SalesPersonUserId = 1,
                ShippingAddress = new AddressCreateDto()
                {
                    Address = "AddresssTessst",
                    City = "CitttyTesssst",
                    CountryId = 1,
                    PostCode = 4500,
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/orders", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_order_empty_address_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new OrderCreateDto
            {
                OrderName = "Create Order Test",
                Deadline = new DateTime(2021, 01, 01),
                CustomerUserId = 1,
                SalesPersonUserId = 1,
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/orders", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Order details
        [Fact]
        public async Task Get_order_details_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var orderId = new Guid("fbba3ce4-f622-4500-9206-ae8ba8aa6ce7");
            var expected = BuildOrdesSalesDto();

            // Act
            var resp = await client.GetAsync($"api/orders/{orderId}");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<OrderSalesHeaderDto>(stringresp);

            // we must compare just the dates beacuse due to domain events there are some seconds delay
            responseContent.StatusDeadline = responseContent.StatusDeadline.Value.Date;

            //Always changing -> ignore at check
            expected.CreatedOn = responseContent.CreatedOn;
            responseContent.Should().BeEquivalentTo(expected);
        }
       
        [Fact]
        public async Task Get_order_deatils_wrong_orderId_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var orderId = new Guid("FBBA3CE4-F622-4500-9206-AE8BA8AA6456");

            // Act
            var resp = await client.GetAsync($"api/orders/{orderId}");
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        #endregion

        #region Delete Order
        
        [Fact]
        public async Task Delete_order_success()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var createdOrderId = new Guid("4aa425db-791f-4a09-ae70-e409287210eb");
            var respGet = await client.GetAsync($"api/orders/{createdOrderId}");

            // Assert
            await processResponse(respGet);
            respGet.EnsureSuccessStatusCode();

            //Act
            var resp = await client.DeleteAsync($"api/orders/{createdOrderId}");

            // Assert
            await processResponse(resp);
            resp.EnsureSuccessStatusCode();

            //Act
            var respGet2 = await client.GetAsync($"api/orders/{createdOrderId}");

            //Assert
            respGet2.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_order_wrong_orderId_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var createdOrderId = new Guid("4aa425db-791f-4a09-ae51-e409287210eb");
            var resp = await client.DeleteAsync($"api/orders/{createdOrderId}");

            // Assert
            await processResponse(resp);
            resp.EnsureSuccessStatusCode();
        }
        #endregion

        #region Order Documents

        [Fact]
        public async Task Get_order_documents_empty_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var orderId = new Guid("1bca75cd-6105-42c2-97b7-af2239c987a2");

            // Act
            var resp = await client.GetAsync($"api/orders/{orderId}/documents");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<List<DocumentGroupDto>>(stringresp);
            responseContent.Count.Should().Be(5);
            responseContent.All(x => x.Versions.Count == 1).Should().BeTrue();
            responseContent.Count(x => x.IsVersionable).Should().Be(3);
            responseContent.Count(x => !x.IsVersionable).Should().Be(2);
            responseContent.All(x => x.Versions.All(y => y.DocumentState.State == DocumentStateEnum.Empty && !y.Documents.Any())).Should().BeTrue();

        }
        [Fact]
        public async Task Get_order_documents_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var orderId = new Guid("43fe7efe-714b-4811-9872-395046428a7f");

            // Act
            var resp = await client.GetAsync($"api/orders/{orderId}/documents");

            // Assert
            var stringresp = await processResponse(resp);
            resp.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.DeserializeObject<List<DocumentGroupDto>>(stringresp);
            responseContent.Count.Should().Be(5);
            responseContent.Count(x => x.IsVersionable).Should().Be(3);
            responseContent.Count(x => !x.IsVersionable).Should().Be(2);
            responseContent.Single(x => x.DocumentGroupId == 10006).Versions.First().Documents.Count.Should().BeGreaterOrEqualTo(2);

        }
        // TODO buildnél elfailelt
        // SQLite foreign key
        [Fact]
        public async Task Get_order_documents_wrong_orderId_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var orderId = new Guid("1bca75cd-6105-42c2-97b7-af2239c987f2");

            // Act
            var resp = await client.GetAsync($"api/orders/{orderId}/documents");
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Post_order_documents_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new DocumentUploadDto
            {
                DocumentGroupId = 10007,
                DocumentGroupVersionId = 10007,
                DocumentTypeId = 2,
                UploaderUserId = 1,
                Documents = new List<DocumentCreateDto>()
                {
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf",
                    },
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "691b627a-0d17-4cd0-9711-43ad975bdc43.docx",
                    },
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var orderId = new Guid("43fe7efe-714b-4811-9872-395046428a7f");


            // Act
            var resp = await client.PostAsync($"api/orders/{orderId}/documents", content);

            // Assert
            await processResponse(resp);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var documentGroups = await orderRepository.GetDocumentGroupsAsync(orderId, DocumentGroupDto.Projection);

                documentGroups.Should().NotBeEmpty();
                documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Single(x => x.Id == 10007).Documents.Should().NotBeEmpty();
                documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Single(x => x.Id == 10007).Documents.Count.Should().Be(2);
                Assert.Contains(documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Single(x => x.Id == 10007).Documents, d => d.FileExtensionType == FileExtensionTypeEnum.Pdf);
                Assert.Contains(documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Single(x => x.Id == 10007).Documents, d => d.FileExtensionType == FileExtensionTypeEnum.Word);
                documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Single(x => x.Id == 10007).Documents.Count.Should().Be(2);
                documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Single(x => x.Id == 10007).DocumentState.State.Should().Be(DocumentStateEnum.WaitingForApproval);


                var documentRepository = scope.ServiceProvider.GetRequiredService<IDocumentRepository>();
                var documents = await documentRepository.GetAllListAsync(x => x.DocumentGroupVersionId == 10007);
                Assert.Contains(documents, d => d.FileName == dto.Documents[0].FileName.ToLower());
                Assert.Contains(documents, d => d.FileName == dto.Documents[1].FileName.ToLower());
                documents.Single(x => x.FileExtensionType == FileExtensionTypeEnum.Word).Extension.Should().Be(".docx");
            }
        }
        [Fact]
        public async Task Post_order_documents_wrong_orderId_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new DocumentUploadDto
            {
                DocumentGroupId = 10007,
                DocumentGroupVersionId = 10007,
                DocumentTypeId = 2,
                UploaderUserId = 1,
                Documents = new List<DocumentCreateDto>()
                {
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf",
                    },
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "691b627a-0d17-4cd0-9711-43ad975bdc43.docx",
                    },
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var orderId = new Guid("43fe7eff-714b-4811-9872-395046428a7f");


            // Act
            var resp = await client.PostAsync($"api/orders/{orderId}/documents", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task Post_order_documents_wrong_groupId_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new DocumentUploadDto
            {
                DocumentGroupId = 20007,
                DocumentGroupVersionId = 10007,
                DocumentTypeId = 2,
                UploaderUserId = 1,
                Documents = new List<DocumentCreateDto>()
                {
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf",
                    },
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "691b627a-0d17-4cd0-9711-43ad975bdc43.docx",
                    },
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var orderId = new Guid("43fe7efe-714b-4811-9872-395046428a7f");


            // Act
            var resp = await client.PostAsync($"api/orders/{orderId}/documents", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task Post_order_documents_wrong_uploader_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new DocumentUploadDto
            {
                DocumentGroupId = 10007,
                DocumentGroupVersionId = 10007,
                DocumentTypeId = 2,
                UploaderUserId = 200000,
                Documents = new List<DocumentCreateDto>()
                {
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf",
                    },
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "691b627a-0d17-4cd0-9711-43ad975bdc43.docx",
                    },
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var orderId = new Guid("43fe7efe-714b-4811-9872-395046428a7f");


            // Act
            var resp = await client.PostAsync($"api/orders/{orderId}/documents", content);
            //await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Post_order_documents_wrong_type_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new DocumentUploadDto
            {
                DocumentGroupId = 10007,
                DocumentGroupVersionId = 10007,
                DocumentTypeId = 5,
                UploaderUserId = 1,
                Documents = new List<DocumentCreateDto>()
                {
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf",
                    },
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "691b627a-0d17-4cd0-9711-43ad975bdc43.docx",
                    },
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var orderId = 10800;


            // Act
            var resp = await client.PostAsync($"api/orders/{orderId}/documents", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Post_order_documents_empty_documents_list_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new DocumentUploadDto
            {
                DocumentGroupId = 10007,
                DocumentGroupVersionId = 10007,
                DocumentTypeId = 2,
                UploaderUserId = 1,
                Documents = new List<DocumentCreateDto>()
                {

                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var orderId = 10800;


            // Act
            var resp = await client.PostAsync($"api/orders/{orderId}/documents", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Post_order_documents_wrong_groupVersinId_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new DocumentUploadDto
            {
                DocumentGroupId = 10007,
                DocumentGroupVersionId = 20007,
                DocumentTypeId = 2,
                UploaderUserId = 1,
                Documents = new List<DocumentCreateDto>()
                {
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf",
                    },
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "691b627a-0d17-4cd0-9711-43ad975bdc43.docx",
                    },
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var orderId = new Guid("43fe7efe-714b-4811-9872-395046428a7f");


            // Act
            var resp = await client.PostAsync($"api/orders/{orderId}/documents", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Post_order_documents_add_new_version_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new DocumentUploadDto
            {
                DocumentGroupId = 10007,
                DocumentTypeId = 2,
                UploaderUserId = 1,
                Documents = new List<DocumentCreateDto>()
                {
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "86c01738-c97b-4345-80f5-f5a9694e3009.xlsx",
                    },
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "e6a02783-a344-4aa3-96f3-5cd689d25df8.png",
                    },
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "eb83b85e-6970-44bb-8ae4-1042815004df.pdf",
                    },
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var orderId = new Guid("43fe7efe-714b-4811-9872-395046428a7f");


            // Act
            var resp = await client.PostAsync($"api/orders/{orderId}/documents", content);
            await processResponse(resp);

            // Assert
            resp.EnsureSuccessStatusCode();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var documentGroups = await orderRepository.GetDocumentGroupsAsync(orderId, DocumentGroupDto.Projection);

                documentGroups.Should().NotBeEmpty();
                documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Should().NotBeEmpty();
                documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Count.Should().Be(2);
                var newVersionId = documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Single(x => x.Id != 10007).Id;
                Assert.Contains(documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Single(x => x.Id == newVersionId).Documents, d => d.FileExtensionType == FileExtensionTypeEnum.Pdf);
                Assert.Contains(documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Single(x => x.Id == newVersionId).Documents, d => d.FileExtensionType == FileExtensionTypeEnum.Picture);
                Assert.Contains(documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Single(x => x.Id == newVersionId).Documents, d => d.FileExtensionType == FileExtensionTypeEnum.Spreadsheet);
                documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Single(x => x.Id == newVersionId).Documents.Count.Should().Be(3);
                documentGroups.Single(x => x.DocumentGroupId == 10007).Versions.Single(x => x.Id == newVersionId).DocumentState.State.Should().Be(DocumentStateEnum.WaitingForApproval);

                var documentRepository = scope.ServiceProvider.GetRequiredService<IDocumentRepository>();
                var documents = await documentRepository.GetAllListAsync(x => x.DocumentGroupVersionId == newVersionId);
                Assert.Contains(documents, d => d.FileName == dto.Documents[0].FileName.ToLower());
                Assert.Contains(documents, d => d.FileName == dto.Documents[1].FileName.ToLower());
                Assert.Contains(documents, d => d.FileName == dto.Documents[2].FileName.ToLower());
                documents.Single(x => x.FileExtensionType == FileExtensionTypeEnum.Picture).Extension.Should().Be(".png");
            }
        }
        [Fact]
        public async Task Post_order_documents_add_new_version_to_non_historized_group_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var dto = new DocumentUploadDto
            {
                DocumentGroupId = 10010,
                DocumentTypeId = 10,
                UploaderUserId = 1,
                Documents = new List<DocumentCreateDto>()
                {
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf",
                    },
                    new DocumentCreateDto()
                    {
                        ContainerName = "OrderTests",
                        FileName = "691b627a-0d17-4cd0-9711-43ad975bdc43.docx",
                    },
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var orderId = 10800;


            // Act
            var resp = await client.PostAsync($"api/orders/{orderId}/documents", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_order_documents_approve_version_success()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getCustomer3AccessToken());

            var orderId = new Guid("8687D8AE-CE64-4D0D-BBF9-598E5AA40BF2");
            var versionId = 10016;

            var content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/orders/{orderId}/documents/versions/{versionId}?result={DocumentStateEnum.Approved}", content);
            await processResponse(resp);

            // Assert

            resp.EnsureSuccessStatusCode();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var documentGroups = await orderRepository.GetDocumentGroupsAsync(orderId, DocumentGroupDto.Projection);

                documentGroups.Should().NotBeEmpty();
                Assert.Contains(documentGroups, x => x.Versions.Any(y => y.Id == versionId));
                documentGroups.Single(x => x.Versions.Any(y => y.Id == versionId)).Versions.Single(x => x.Id == versionId).DocumentState.State.Should().Be(DocumentStateEnum.Approved);
            }
        }

        [Fact]
        public async Task Put_order_documents_approve_another_user_documents_should_fail()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getCustomer2AccessToken());

            var orderId = new Guid("228AAC26-1066-4D33-AF02-986EBA4EF15B");
            var versionId = 10011;

            var content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/orders/{orderId}/documents/versions/{versionId}?result={DocumentStateEnum.Approved}", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Put_order_documents_wrong_result_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getCustomer2AccessToken());

            var orderId = 10803;
            var versionId = 10017;

            var content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/orders/{orderId}/documents/versions/{versionId}?result={DocumentStateEnum.Uploaded}", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_order_documents_approve_declined_version_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getCustomer2AccessToken());

            var orderId = 10803;
            var versionId = 10018;

            var content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/orders/{orderId}/documents/versions/{versionId}?result={DocumentStateEnum.Approved}", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_order_documents_approve_non_historized_version_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getCustomer2AccessToken());

            var orderId = 10803;
            var versionId = 10019;

            var content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/orders/{orderId}/documents/versions/{versionId}?result={DocumentStateEnum.Approved}", content);
            await processResponse(resp);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Build Entities
        private OrderSalesHeaderDto BuildOrdesSalesDto()
        {
            return new OrderSalesHeaderDto()
            {
                OrderId = "fbba3ce4-f622-4500-9206-ae8ba8aa6ce7",
                OrderName = "Test Bestellung",
                CurrentStatus = new OrderStateDto()
                {
                    Id = 3,
                    State = OrderStateEnum.WaitingForOffer,
                    Translation = "Várakozás árajánlatra"
                },
                StatusDeadline = Clock.Now.AddDays(2).Date,//new DateTime(2020, 01, 01),
                Responsible = BuildUser("sales@sales.de", 10701, "John Doe", "45348976"),
                Customer = BuildUser("kunden@kunden.de", 10700, "John Doe", "45348976"),
                Sales = BuildUser("sales@sales.de", 10701, "John Doe", "45348976"),
                Deadline = new DateTime(2020, 01, 31),
                CustomerAddress = new AddressDetailsDto() { City = "Budapest", CountryId = 1, PostCode = 1357, Address = "Kossuth tér 1-3." },
                DivisionTypes = new List<DivisionTypeEnum>() { DivisionTypeEnum.Admin }
            };
        }

        private UserDto BuildUser(string email, int id, string name, string phoneNumber)
        {
            return new UserDto { Email = email, Id = id, Name = name, PhoneNumber = phoneNumber };
        }

        private OrderCreateDto BuildOrderDto()
        {
            return new OrderCreateDto
            {
                OrderName = "Create Order Test",
                Deadline = new DateTime(2021, 01, 01),
                CustomerUserId = 10006,
                SalesPersonUserId = 10006,
                ShippingAddress = new AddressCreateDto()
                {
                    Address = "AddresssTessst",
                    City = "CitttyTesssst",
                    CountryId = 1,
                    PostCode = 4500,
                }
            };
        }

        private static FurnitureUnitCreateWithQuantityByOfferDto BuildBasefurnitureUnitDto(FoilMaterial foilMaterial)
        {
            return new FurnitureUnitCreateWithQuantityByOfferDto
            {
                CategoryId = 10000,
                Code = "HA129999",
                Depth = 2,
                Width = 2,
                Height = 2,
                Quantity = 1,
                Description = "Description",
                Corpuses = new List<FurnitureComponentsCreateByOfferDto> {
                    new FurnitureComponentsCreateByOfferDto { Id = new Guid("56A1EB6F-3B04-4928-8551-53677639C6A8"), Amount = 1, BottomFoilId = foilMaterial.Id, LeftFoilId = foilMaterial.Id, RightFoilId = foilMaterial.Id, TopFoilId = foilMaterial.Id, Name = "Test-FC-3120", Height = 500, Width = 500 } },
                Fronts = new List<FurnitureComponentsCreateByOfferDto> { new FurnitureComponentsCreateByOfferDto { Id = new Guid("56a1eb6f-3b04-4928-8551-53677639c6a8"), Width = 500, Height = 100, Amount = 1, Name = "Test-FC-3120", BottomFoilId = foilMaterial.Id, LeftFoilId = foilMaterial.Id, RightFoilId = foilMaterial.Id, TopFoilId = foilMaterial.Id } }
            };
        }

        #endregion
    }
}
