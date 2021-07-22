using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Paging;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Extensions;
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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class RequiredMaterialsTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public RequiredMaterialsTests(IFPSFactoryWebApplicationFactory factory)
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

        // TODO
        [Fact]
        public async Task Get_required_materials_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<RequiredMaterialsListDto>();

            var order = new Order("Test Bestellung-1203") { Id = new Guid("9aa53060-4b7a-4f82-8784-2bcc6313fbd3"), WorkingNumber = "MZ/X-503", Deadline = new DateTime(2019, 8, 25) };
            var companyType = new CompanyType(CompanyTypeEnum.SupplierCompany);
            var supplier = new Company("Test Super Supplier Company", 10002) { Id = 10001, CurrentVersionId = 10000 };
            var packages = new List<MaterialPackage>()
            {
                new MaterialPackage(new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), 10001) { Id = 10000, PackageCode = "PAC-591", PackageDescription = "Package with stuff", Size = 1, Supplier = supplier, SupplierId = supplier.Id },
                new MaterialPackage(new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), 10001) { Id = 10001, PackageCode = "COD-748", PackageDescription = "Stuff", Size = 1, Supplier = supplier, SupplierId = supplier.Id },
                new MaterialPackage(new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), 10001) { Id = 10002, PackageCode = "PDE-249", PackageDescription = "Furniture stuff", Size = 1, Supplier = supplier, SupplierId = supplier.Id },
                new MaterialPackage(new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), 10001) { Id = 10003, PackageCode = "GOD-777", PackageDescription = "Sofa stuff", Size = 1, Supplier = supplier, SupplierId = supplier.Id }
            };
            var material = new Material("DEC", 10) { Id = new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), Description = "beautiful dec" };
            material.AddMaterialPackageList(packages);
            var suppliers = material.Packages.Select(ent => new SupplierDropdownListDto(ent.SupplierId, ent.Supplier.Name)).Distinct(new SupplierDtoEqualityComparer()).ToList();

            var requiredMaterial = new RequiredMaterialsListDto()
            {
                Id = 10000,
                OrderName = order.OrderName,
                WorkingNumber = order.WorkingNumber,
                MaterialCode = material.Code,
                Name = material.Description,
                Amount = 5,
                Deadline = order.Deadline,
                Suppliers = suppliers
            };

            expectedResult.Add(requiredMaterial);
            IPagedList<RequiredMaterialsListDto> pagedList = new PagedList<RequiredMaterialsListDto>()
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
                var service = scope.ServiceProvider.GetRequiredService<IRequiredMaterialsAppService>();
                var requiredMaterialFilterDto = new RequiredMaterialsFilterDto() { MaterialCode = "DEC" };
                var requiredMaterials = await service.GetRequiredMaterialsListAsync(requiredMaterialFilterDto);

                // Assert
                result.Should().BeEquivalentTo(requiredMaterials);
            }
        }

        [Fact]
        public async Task Get_required_materials_filter_materialcode_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRequiredMaterialsAppService>();
                var requiredMaterialFilterDto = new RequiredMaterialsFilterDto() { MaterialCode = "It's so wrong code" };
                var cargos = await service.GetRequiredMaterialsListAsync(requiredMaterialFilterDto);

                // Assert
                cargos.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_required_materials_filter_order_name_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRequiredMaterialsAppService>();
                var requiredMaterialFilterDto = new RequiredMaterialsFilterDto() { Name = "Wrong description/name" };
                var cargos = await service.GetRequiredMaterialsListAsync(requiredMaterialFilterDto);

                // Assert
                cargos.Data.Count().Should().Be(0);
            }
        }
    }
}
