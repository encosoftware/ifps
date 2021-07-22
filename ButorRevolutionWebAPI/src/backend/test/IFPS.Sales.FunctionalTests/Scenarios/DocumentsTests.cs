using IFPS.Sales.Application.Dto;
﻿using ENCO.DDD.Domain.Model.Enums;
using FluentAssertions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Implementations;
using IFPS.Sales.Domain.Services.Interfaces;
using IFPS.Sales.EF;
using IFPS.Sales.EF.Repositories;
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

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class DocumentsTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public DocumentsTests(IFPSSalesWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }
        private async Task<string> GetAccessToken()
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

        private DocumentService InitDocumentService(IServiceScope scope, out DocumentRepositoryMock documentRepositoryMock)
        {
            var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
            documentRepositoryMock = new DocumentRepositoryMock(context);
            var fileHandleService = scope.ServiceProvider.GetRequiredService<IFileHandlerService>();
            var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
            var documentStateRepository = scope.ServiceProvider.GetRequiredService<IDocumentStateRepository>();

            return new DocumentService(documentRepositoryMock, fileHandleService, orderRepository, documentStateRepository);
        }

        #region Document creation
        [Fact]
        public async Task Create_document_success()
        {
            // Arrange
            var client = factory.CreateClient();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var documentService = InitDocumentService(scope, out var documentRepositoryMock);
                var documentType = new DocumentType(DocumentTypeEnum.None, 1) { Id = 10801 };
                var userData = new UserData("Nagy Gertrúd", "45348976", Clock.Now) { Id = 10800, ContactAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1) };
                var user = new User("beviz.elek@envotest.hu", LanguageTypeEnum.HU) { Id = 10800, CurrentVersionId = userData.Id, CurrentVersion = userData };
                // Act
                var firstDocument = await documentService.CreateDocumentAsync("OrderTests", "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf", documentType, user);
                var secondDocument = await documentService.CreateDocumentAsync("OrderTests", "8c6c9f0f-0d73-4359-b082-6a411b030e73.png", documentType, user);

                // Assert
                var insertedDocuments = documentRepositoryMock.InsertedDocuments;
                insertedDocuments.Count.Should().Be(2);
                insertedDocuments.Single(x => x.Id == firstDocument.Id).FileName.Should().Be("637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf");
                insertedDocuments.Single(x => x.Id == firstDocument.Id).UploadedBy.Id.Should().Be(user.Id);
                insertedDocuments.Single(x => x.Id == firstDocument.Id).Extension.Should().Be(".pdf");
                insertedDocuments.Single(x => x.Id == firstDocument.Id).FileExtensionType.Should().Be(FileExtensionTypeEnum.Pdf);
                insertedDocuments.Single(x => x.Id == firstDocument.Id).DocumentType.Id.Should().Be(documentType.Id);

                insertedDocuments.Single(x => x.Id == secondDocument.Id).FileName.Should().Be("8c6c9f0f-0d73-4359-b082-6a411b030e73.png");
                insertedDocuments.Single(x => x.Id == secondDocument.Id).UploadedBy.Id.Should().Be(user.Id);
                insertedDocuments.Single(x => x.Id == secondDocument.Id).Extension.Should().Be(".png");
                insertedDocuments.Single(x => x.Id == secondDocument.Id).FileExtensionType.Should().Be(FileExtensionTypeEnum.Picture);
                insertedDocuments.Single(x => x.Id == secondDocument.Id).DocumentType.Id.Should().Be(documentType.Id);
            };
        }
        [Fact]
        public async Task Create_document_wrong_file_name_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var documentService = InitDocumentService(scope, out var documentRepositoryMock);
                var documentType = new DocumentType(DocumentTypeEnum.None, 1) { Id = 10801 };
                var userData = new UserData("Nagy Gertrúd", "45348976", Clock.Now) { Id = 10800, ContactAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1) };
                var user = new User("beviz.elek@envotest.hu", LanguageTypeEnum.HU) { Id = 10800, CurrentVersionId = userData.Id, CurrentVersion = userData };
                // Act & Assert
                await Assert.ThrowsAsync<IFPSDomainException>(() => documentService.CreateDocumentAsync("OrderTests", "637af3ea-60f4-4c3f-b98c-bb000000.pdf", documentType, user));
            };
        }
        [Fact]
        public async Task Create_document_wrong_container_fails()
        {
            // Arrange
            var client = factory.CreateClient();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var documentService = InitDocumentService(scope, out var documentRepositoryMock);
                var documentType = new DocumentType(DocumentTypeEnum.None, 1) { Id = 10801 };
                var userData = new UserData("Nagy Gertrúd", "45348976", Clock.Now) { Id = 10800, ContactAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1) };
                var user = new User("beviz.elek@envotest.hu", LanguageTypeEnum.HU) { Id = 10800, CurrentVersionId = userData.Id, CurrentVersion = userData };

                // Act & Assert
                await Assert.ThrowsAsync<IFPSDomainException>(() => documentService.CreateDocumentAsync("non", "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf", documentType, user));
            };
        }
        [Fact]
        public async Task Create_document_set_file_extension_type_works()
        {
            // Arrange
            var client = factory.CreateClient();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var documentService = InitDocumentService(scope, out var documentRepositoryMock);
                var documentType = new DocumentType(DocumentTypeEnum.None, 1) { Id = 10801 };
                // Act
                var firstDocument = await documentService.CreateDocumentAsync("OrderTests", "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf", documentType, null);
                var secondDocument = await documentService.CreateDocumentAsync("OrderTests", "8c6c9f0f-0d73-4359-b082-6a411b030e73.png", documentType, null);
                var thirdDocument = await documentService.CreateDocumentAsync("OrderTests", "691b627a-0d17-4cd0-9711-43ad975bdc43.docx", documentType, null);
                var fourthDocument = await documentService.CreateDocumentAsync("OrderTests", "86c01738-c97b-4345-80f5-f5a9694e3009.xlsx", documentType, null);

                // Assert
                var insertedDocuments = documentRepositoryMock.InsertedDocuments;
                insertedDocuments.Count.Should().Be(4);
                insertedDocuments.Single(x => x.Id == firstDocument.Id).Extension.Should().Be(".pdf");
                insertedDocuments.Single(x => x.Id == firstDocument.Id).FileExtensionType.Should().Be(FileExtensionTypeEnum.Pdf);
                insertedDocuments.Single(x => x.Id == secondDocument.Id).Extension.Should().Be(".png");
                insertedDocuments.Single(x => x.Id == secondDocument.Id).FileExtensionType.Should().Be(FileExtensionTypeEnum.Picture);
                insertedDocuments.Single(x => x.Id == thirdDocument.Id).Extension.Should().Be(".docx");
                insertedDocuments.Single(x => x.Id == thirdDocument.Id).FileExtensionType.Should().Be(FileExtensionTypeEnum.Word);
                insertedDocuments.Single(x => x.Id == fourthDocument.Id).Extension.Should().Be(".xlsx");
                insertedDocuments.Single(x => x.Id == fourthDocument.Id).FileExtensionType.Should().Be(FileExtensionTypeEnum.Spreadsheet);
            };
        }
        #endregion

        #region Adding documents to DocumentGroupVersions
        [Fact]
        public async Task Add_document_to_existing_version_works()
        {
            // Arrange
            var client = factory.CreateClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var documentService = InitDocumentService(scope, out var documentRepositoryMock);
                var documentTypeRepository = scope.ServiceProvider.GetRequiredService<IDocumentTypeRepository>();
                var documentType = await documentTypeRepository.SingleAsync(x => x.Type == DocumentTypeEnum.TechnicalDrawing);

                // Act
                var firstDocument = await documentService.CreateDocumentAsync("OrderTests", "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf", documentType, null);
                var secondDocument = await documentService.CreateDocumentAsync("OrderTests", "8c6c9f0f-0d73-4359-b082-6a411b030e73.png", documentType, null);
                var thirdDocument = await documentService.CreateDocumentAsync("OrderTests", "86c01738-c97b-4345-80f5-f5a9694e3009.xlsx", documentType, null);

                var orderId = new Guid("43fe7efe-714b-4811-9872-395046428a7f");//10800;
                var groupId = 10009;
                var versionId = 10009;

                var insertedDocuments = documentRepositoryMock.InsertedDocuments;
                insertedDocuments.Count.Should().Be(3);

                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var documentGroup = await orderRepository.GetDocumentGroupWithFolderAsync(orderId, groupId);


                var version = await documentService.AddDocumentsToExistingVersionAsync(documentGroup, versionId, insertedDocuments, documentType);


                // Assert
                version.Documents.Count().Should().Be(3);
                Assert.Contains(version.Documents, x => x.Id == firstDocument.Id);
                version.Documents.Single(x => x.Id == firstDocument.Id).DisplayName.Should().Be("MSZ1111-2022_TechnicalDrawing_4.pdf");
                Assert.Contains(version.Documents, x => x.Id == secondDocument.Id);
                version.Documents.Single(x => x.Id == secondDocument.Id).DisplayName.Should().Be("MSZ1111-2022_TechnicalDrawing_5.png");
                Assert.Contains(version.Documents, x => x.Id == thirdDocument.Id);
                version.Documents.Single(x => x.Id == thirdDocument.Id).DisplayName.Should().Be("MSZ1111-2022_TechnicalDrawing_6.xlsx");
                version.Id.Should().Be(versionId);
                version.Core.Id.Should().Be(groupId);
                version.State.State.Should().Be(DocumentStateEnum.Uploaded);

            }
        }


        [Fact]
        public async Task Add_document_to_new_version_works()
        {
            // Arrange
            var client = factory.CreateClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var documentService = InitDocumentService(scope, out var documentRepositoryMock);
                var documentTypeRepository = scope.ServiceProvider.GetRequiredService<IDocumentTypeRepository>();
                var documentType = await documentTypeRepository.SingleAsync(x => x.Type == DocumentTypeEnum.Contract);

                // Act
                var firstOrder = await documentService.CreateDocumentAsync("OrderTests", "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf", documentType, null);
                var secondOrder = await documentService.CreateDocumentAsync("OrderTests", "8c6c9f0f-0d73-4359-b082-6a411b030e73.png", documentType, null);

                var orderId = new Guid("43fe7efe-714b-4811-9872-395046428a7f");
                var groupId = 10008;
                var versionId = 10008;

                var insertedDocuments = documentRepositoryMock.InsertedDocuments;
                insertedDocuments.Count.Should().Be(2);

                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var documentGroup = await orderRepository.GetDocumentGroupWithFolderAsync(orderId, groupId);

                var version = await documentService.AddDocumentsToNewVersionAsync(documentGroup, insertedDocuments, documentType);

                // Assert
                version.Documents.Count().Should().Be(2);
                Assert.Contains(version.Documents, x => x.Id == firstOrder.Id);
                version.Documents.Single(x => x.Id == firstOrder.Id).DisplayName.Should().Be("MSZ1111-2022_Contract_3.pdf");
                Assert.Contains(version.Documents, x => x.Id == secondOrder.Id);
                version.Documents.Single(x => x.Id == secondOrder.Id).DisplayName.Should().Be("MSZ1111-2022_Contract_4.png");
                version.Id.Should().NotBe(versionId);
                version.Id.Should().BeLessOrEqualTo(0);
                version.Core.Id.Should().Be(groupId);
                version.State.State.Should().Be(DocumentStateEnum.WaitingForApproval);

            };
        }
        [Fact]
        public async Task Add_new_version_to_non_historized_folder_fails()
        {
            // Arrange
            var client = factory.CreateClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var documentService = InitDocumentService(scope, out var documentRepositoryMock);
                var documentTypeRepository = scope.ServiceProvider.GetRequiredService<IDocumentTypeRepository>();
                var documentType = await documentTypeRepository.SingleAsync(x => x.Type == DocumentTypeEnum.TechnicalDrawing);

                // Act
                await documentService.CreateDocumentAsync("OrderTests", "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf", documentType, null);
                await documentService.CreateDocumentAsync("OrderTests", "8c6c9f0f-0d73-4359-b082-6a411b030e73.png", documentType, null);
                await documentService.CreateDocumentAsync("OrderTests", "86c01738-c97b-4345-80f5-f5a9694e3009.xlsx", documentType, null);

                var orderId = new Guid("43fe7efe-714b-4811-9872-395046428a7f");
                var groupId = 10009;

                var insertedDocuments = documentRepositoryMock.InsertedDocuments;
                insertedDocuments.Count.Should().Be(3);

                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var documentGroup = await orderRepository.GetDocumentGroupWithFolderAsync(orderId, groupId);


                await Assert.ThrowsAsync<IFPSDomainException>(() => documentService.AddDocumentsToNewVersionAsync(documentGroup, insertedDocuments, documentType));
            };
        }
        [Fact]
        public async Task Add_wrong_type_of_document_fails()
        {
            // Arrange
            var client = factory.CreateClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var documentService = InitDocumentService(scope, out var documentRepositoryMock);
                var documentTypeRepository = scope.ServiceProvider.GetRequiredService<IDocumentTypeRepository>();
                var documentType = await documentTypeRepository.SingleAsync(x => x.Type == DocumentTypeEnum.Render);

                // Act
                await documentService.CreateDocumentAsync("OrderTests", "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf", documentType, null);
                await documentService.CreateDocumentAsync("OrderTests", "8c6c9f0f-0d73-4359-b082-6a411b030e73.png", documentType, null);
                await documentService.CreateDocumentAsync("OrderTests", "86c01738-c97b-4345-80f5-f5a9694e3009.xlsx", documentType, null);

                var orderId = new Guid("43fe7efe-714b-4811-9872-395046428a7f");
                var groupId = 10009;

                var insertedDocuments = documentRepositoryMock.InsertedDocuments;
                insertedDocuments.Count.Should().Be(3);

                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var documentGroup = await orderRepository.GetDocumentGroupWithFolderAsync(orderId, groupId);

                await Assert.ThrowsAsync<IFPSDomainException>(() => documentService.AddDocumentsToNewVersionAsync(documentGroup, insertedDocuments, documentType));

            };
        }
        [Fact]
        public async Task Add_document_to_approved_version_fails()
        {
            // Arrange
            var client = factory.CreateClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var documentService = InitDocumentService(scope, out var documentRepositoryMock);
                var documentTypeRepository = scope.ServiceProvider.GetRequiredService<IDocumentTypeRepository>();
                var documentType = await documentTypeRepository.SingleAsync(x => x.Type == DocumentTypeEnum.TechnicalDrawing);

                // Act
                await documentService.CreateDocumentAsync("OrderTests", "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf", documentType, null);
                await documentService.CreateDocumentAsync("OrderTests", "8c6c9f0f-0d73-4359-b082-6a411b030e73.png", documentType, null);
                await documentService.CreateDocumentAsync("OrderTests", "86c01738-c97b-4345-80f5-f5a9694e3009.xlsx", documentType, null);

                var orderId = new Guid("8687d8ae-ce64-4d0d-bbf9-598e5aa40bf2");
                var groupId = 10016;
                var versionId = 10016;

                var insertedDocuments = documentRepositoryMock.InsertedDocuments;
                insertedDocuments.Count.Should().Be(3);

                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var documentGroup = await orderRepository.GetDocumentGroupWithFolderAsync(orderId, groupId);


                await Assert.ThrowsAsync<IFPSDomainException>(() => documentService.AddDocumentsToExistingVersionAsync(documentGroup, versionId, insertedDocuments, documentType));
            };
        }
        [Fact]
        public async Task Add_document_to_declined_version_fails()
        {
            // Arrange
            var client = factory.CreateClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var documentService = InitDocumentService(scope, out var documentRepositoryMock);
                var documentTypeRepository = scope.ServiceProvider.GetRequiredService<IDocumentTypeRepository>();
                var documentType = await documentTypeRepository.SingleAsync(x => x.Type == DocumentTypeEnum.TechnicalDrawing);

                // Act
                await documentService.CreateDocumentAsync("OrderTests", "637af3ea-60f4-4c3f-b98c-bb0373c0e676.pdf", documentType, null);
                await documentService.CreateDocumentAsync("OrderTests", "8c6c9f0f-0d73-4359-b082-6a411b030e73.png", documentType, null);
                await documentService.CreateDocumentAsync("OrderTests", "86c01738-c97b-4345-80f5-f5a9694e3009.xlsx", documentType, null);

                var orderId = new Guid("8687d8ae-ce64-4d0d-bbf9-598e5aa40bf2");
                var groupId = 10017;
                var versionId = 10017;

                var insertedDocuments = documentRepositoryMock.InsertedDocuments;
                insertedDocuments.Count.Should().Be(3);

                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var documentGroup = await orderRepository.GetDocumentGroupWithFolderAsync(orderId, groupId);


                await Assert.ThrowsAsync<IFPSDomainException>(() => documentService.AddDocumentsToExistingVersionAsync(documentGroup, versionId, insertedDocuments, documentType));
            };
        }
        #endregion

    }

    public class DocumentRepositoryMock : DocumentRepository
    {
        public List<Document> InsertedDocuments { get; private set; }
        public DocumentRepositoryMock(IFPSSalesContext context) : base(context)
        {
            InsertedDocuments = new List<Document>();
        }

        public override Task<Document> InsertAsync(Document entity)
        {
            InsertedDocuments.Add(entity);
            //entity.Id = InsertedDocuments.Count;
            entity.DocumentTypeId = entity.DocumentType.Id;
            return Task.FromResult(entity);
        }

        public override Task<int> GetCountOfSameTypeOfDocumentAsync(Guid orderId, int documentTypeId)
        {
            return Task.FromResult(InsertedDocuments.Count);
        }

    }
}
