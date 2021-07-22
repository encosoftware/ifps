using ENCO.DDD.Domain.Model.Enums;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    /*
    These tests check a contract by an order (indeed it is OrderTest instead of ContractTest)
        */

    public class ContractTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public ContractTests(IFPSSalesWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
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
        public async Task Get_contract_by_order_should_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var expectedResult = new ContractDetailsDto(BuildOrderForGetContract());

            // Act
            var response = await client.GetAsync($"api/orders/{new Guid("545c7d12-36af-4792-9dde-ba5179a1705f")}/contract");

            // Assert
            response.EnsureSuccessStatusCode();
            response.Should().Equals(expectedResult);
        }

        [Fact]
        public async Task Update_order_create_contract_by_order_should_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var first = new PriceCreateDto() { Value = (decimal)18000.0, CurrencyId = BuildCurrency().Id };
            var second = new PriceCreateDto() { Value = (decimal)23000.0, CurrencyId = BuildCurrency().Id };

            var contractCreateDto = new ContractCreateDto()
            {
                Additional = "Die Welt geht runter",
                ContractDate = new DateTime(2019, 8, 30),
                FirstPayment = first,
                FirstPaymentDate = new DateTime(2019, 11, 5),
                SecondPayment = second,
                SecondPaymentDate = new DateTime(2020, 2, 8)
            };

            var content = new StringContent(JsonConvert.SerializeObject(contractCreateDto), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync($"api/orders/{new Guid("04cd23d7-3ec1-4354-9512-c4c239f47260")}/contract", content);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var modifiedOrder = await repository.SingleIncludingAsync(ent => ent.Id == new Guid("04cd23d7-3ec1-4354-9512-c4c239f47260"), x => x.FirstPayment, x => x.SecondPayment);
                //Assert
                modifiedOrder.FirstPayment.Price.Should().Equals(first.CreateModelObject());
                modifiedOrder.FirstPayment.Deadline.Should().Equals(contractCreateDto.FirstPaymentDate);
                modifiedOrder.SecondPayment.Price.Should().Equals(second.CreateModelObject());
                modifiedOrder.SecondPayment.Deadline.Should().Equals(contractCreateDto.SecondPaymentDate);
                modifiedOrder.ContractDate.Should().Equals(contractCreateDto.ContractDate);
            }

            response.EnsureSuccessStatusCode();

        }

        private Currency BuildCurrency()
        {
            return new Currency("HUF") { Id = 10900 };
        }

        private UserData BuildUserData()
        {
            return new UserData("John Doe", "45348976", Clock.Now) { Id = 10720, ContactAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1) };
        }

        private Company BuildCompany()
        {
            var firstCompanyType = new CompanyType(CompanyTypeEnum.MyCompany) { Id = 10700 };
            var contact = new User("contact@contact.de", LanguageTypeEnum.HU) { Id = 10702, CurrentVersionId = BuildUserData().Id, CurrentVersion = BuildUserData() };
            var companyData = new CompanyData("Tax-150", "Reg-762", new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), null, Clock.Now) { Id = 10780, ContactPerson = contact, ContactPersonId = contact.Id };
            return new Company("Bayerische Motor Werk", firstCompanyType.Id) { Id = 10700, CurrentVersionId = companyData.Id, CurrentVersion = companyData };
        }

        private User BuildFirstUserForCustomer()
        {
            return new User("kunden@kunden.de", LanguageTypeEnum.HU) { Id = 10700, CompanyId = BuildCompany().Id, Company = BuildCompany(), CurrentVersionId = BuildUserData().Id, CurrentVersion = BuildUserData() };
        }

        private User BuildSecondUserForSalesPerson()
        {
            return new User("sales@sales.de", LanguageTypeEnum.HU) { Id = 10701, CompanyId = BuildCompany().Id, Company = BuildCompany(), CurrentVersionId = BuildUserData().Id, CurrentVersion = BuildUserData() };
        }

        private Customer BuildCustomer()
        {
            return new Customer(BuildFirstUserForCustomer().Id, Clock.Now) { Id = 10700, User = BuildFirstUserForCustomer() };
        }

        private SalesPerson BuildSalesPerson()
        {
            return new SalesPerson(BuildSecondUserForSalesPerson().Id, Clock.Now) { Id = 10700, User = BuildSecondUserForSalesPerson() };
        }

        private OrderPrice BuildFirstPayment()
        {
            return new OrderPrice() { Id = 10002, Price = new Price(42000.0, BuildCurrency().Id) { Currency = BuildCurrency() }, Deadline = Clock.Now };
        }

        private OrderPrice BuildSecondPayment()
        {
            return new OrderPrice() { Id = 10003, Price = new Price(42000.0, BuildCurrency().Id) { Currency = BuildCurrency() }, Deadline = Clock.Now };
        }

        private Order BuildOrderForGetContract()
        {
            var order = new Order("Get Contract Order", BuildCustomer().Id, BuildSalesPerson().Id, Clock.Now, null)
            {
                Id = new Guid("545c7d12-36af-4792-9dde-ba5179a1705f"),
                Customer = BuildCustomer(),
                SalesPerson = BuildSalesPerson(),
                FirstPayment = BuildFirstPayment(),
                SecondPayment = BuildSecondPayment(),
                Budget = new Price(0.0, BuildCurrency().Id) { Currency = BuildCurrency() },
                WorkingNumberSerial = 900,
                WorkingNumberYear = 3230,
                OfferInformation = BuildOfferInformation()
            };

            return order;
        }

        private Order BuildOrderForCreateContract()
        {
            var order = new Order("Create Contract Order", BuildCustomer().Id, BuildSalesPerson().Id, Clock.Now, null)
            {
                Id = new Guid("04cd23d7-3ec1-4354-9512-c4c239f47260"),
                Customer = BuildCustomer(),
                SalesPerson = BuildSalesPerson(),

                Budget = new Price(0.0, BuildCurrency().Id) { Currency = BuildCurrency() },
                WorkingNumberSerial = 910,
                WorkingNumberYear = 3231
            };

            return order;
        }

        private OfferInformation BuildOfferInformation()
        {
            return new OfferInformation()
            {
                Id = 10000,
                ProductsPrice = new Price(30000.0, BuildCurrency().Id) { Currency = BuildCurrency() },
                ServicesPrice = new Price(25000.0, BuildCurrency().Id) { Currency = BuildCurrency() }
            };
        }
    }
    
}
