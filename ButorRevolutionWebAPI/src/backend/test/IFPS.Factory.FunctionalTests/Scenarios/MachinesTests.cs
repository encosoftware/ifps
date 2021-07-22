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
    public class MachinesTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public MachinesTests(IFPSFactoryWebApplicationFactory factory)
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
        public async Task Get_machines_for_dropdown_should_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var resp = await client.GetAsync("api/machines/dropdown/machines");

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IMachineAppService>();
                var machines = await service.GetMachinesDropdownAsync();

                // Assert
                resp.EnsureSuccessStatusCode();
                machines.Should().BeEquivalentTo(BuildMachineList());
            }
        }

        #region BuildEntities
        private List<MachinesDropdownDto> BuildMachineList()
        {
            List<MachinesDropdownDto> dto = new List<MachinesDropdownDto>();
            Machine machine1 = new Machine("Test Strong machine", companyId: 10004) { Id = 10000 };
            dto.Add(new MachinesDropdownDto(machine1));

            Machine machine2 = new Machine("The Test strongest machine", companyId: 10004) { Id = 10001 };
            dto.Add(new MachinesDropdownDto(machine2));

            Machine machine3 = new CncMachine("Test CNC machine", brandId: 10000)
            {
                Id = 10002,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Code = "10000"
            };
            dto.Add(new MachinesDropdownDto(machine3));

            Machine machine4 = new CncMachine("Test CNC machine with fairy", brandId: 10000)
            {
                Id = 10003,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Code = "10000"
            };
            dto.Add(new MachinesDropdownDto(machine4));

            Machine machine5 = new CncMachine("Test CNC machine with dwarf", brandId: 10000) { Id = 10004 };
            dto.Add(new MachinesDropdownDto(machine5));

            Machine machine6 = new CncMachine("Test CNC machine with elf", brandId: 10000) { Id = 10005 };
            dto.Add(new MachinesDropdownDto(machine6));

            Machine machine7 = new CncMachine("Test CNC controller machine", brandId: 10000)
            {
                Id = 10010,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Code = "10000"
            };
            dto.Add(new MachinesDropdownDto(machine7));

            Machine machine8 = new CncMachine("Test CNC controller machine", brandId: 10000)
            {
                Id = 10012,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Code = "10000"
            };
            dto.Add(new MachinesDropdownDto(machine8));

            Machine machine9 = new CuttingMachine("Test Cutting machine", brandId: 10000) { Id = 10006 };
            dto.Add(new MachinesDropdownDto(machine9));

            Machine machine10 = new CuttingMachine("Test Cutting machine with unicorn", brandId: 10000) { Id = 10007 };
            dto.Add(new MachinesDropdownDto(machine10));

            Machine machine11 = new CuttingMachine("Test Cutting machine with rainbow", brandId: 10000) { Id = 10008 };
            dto.Add(new MachinesDropdownDto(machine11));

            Machine machine12 = new CuttingMachine("Test Cutting machine with frogs", brandId: 10000)
            {
                Id = 10009,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000"
            };
            dto.Add(new MachinesDropdownDto(machine12));

            Machine machine13 = new CuttingMachine("Test Cutting machine with dragons", brandId: 10000)
            {
                Id = 10011,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000"
            };
            dto.Add(new MachinesDropdownDto(machine13));

            Machine machine14 = new EdgingMachine("Test Edging machine", brandId: 10000) { Id = 10013 };
            dto.Add(new MachinesDropdownDto(machine14));

            Machine machine15 = new EdgingMachine("Test Edging machine with fairy", brandId: 10000) { Id = 10014 };
            dto.Add(new MachinesDropdownDto(machine15));

            Machine machine16 = new EdgingMachine("Test Edging machine with dwarf", brandId: 10000) { Id = 10015 };
            dto.Add(new MachinesDropdownDto(machine16));

            Machine machine17 = new EdgingMachine("Test Edging machine with elf", brandId: 10000)
            {
                Id = 10016,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000"
            };
            dto.Add(new MachinesDropdownDto(machine17));

            Machine machine18 = new EdgingMachine("Test Edging controller machine", brandId: 10000)
            {
                Id = 10017,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000"
            };
            dto.Add(new MachinesDropdownDto(machine18));

            Machine machine19 = new EdgingMachine("Test Edging controller machine", brandId: 10000)
            {
                Id = 10018,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000"
            };
            dto.Add(new MachinesDropdownDto(machine19));
            return dto;
        }

        #endregion
    }
}
