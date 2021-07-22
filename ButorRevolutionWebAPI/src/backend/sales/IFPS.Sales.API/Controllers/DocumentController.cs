using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using IFPS.Sales.API.Infrastructure.Swagger;
using System;
using System.Net.Http;
using IFPS.Sales.Domain.Model;
using Microsoft.Extensions.Options;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentController : IFPSControllerBase
    {
        private const string OPNAME = "Documents";

        private readonly IDocumentAppService documentAppService;
        private readonly IFileHandlerService fileHandlerService;
        private readonly IHttpClientFactory clientFactory;
        private readonly LocalConnectionConfiguration config;

        public DocumentController(
            IDocumentAppService documentAppService,
            IFileHandlerService fileHandlerService,
            IHttpClientFactory clientFactory,
            IOptions<LocalConnectionConfiguration> options)
        {
            this.documentAppService = documentAppService;
            this.fileHandlerService = fileHandlerService;
            this.clientFactory = clientFactory;
            this.config = options.Value;
        }

        // POST document
        [HttpPost]
        [SwaggerFileUpload]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<(string ContainerName, string Filename)> UploadDocument([FromForm] IFormFile file, [FromQuery] string container)
        {
            using (var stream = file.OpenReadStream())
            {
                var (containerName, fileName) = await fileHandlerService.UploadFromStreamAsync(stream, container, Path.GetExtension(file.FileName).ToLower());
                return (containerName, fileName);
            }
        }

        // GET Document
        [HttpGet]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<FileStreamResult> GetDocument(Guid id)
        {
            var document = await documentAppService.GetDocumentAsync(id);
            var stream = new MemoryStream();

            await fileHandlerService.DownloadFromContainer(stream, document.FileName, document.ContainerName);
            stream.Position = 0;

            return File(stream, "application/octet-stream", document.DisplayName);
        }

        // GET document types list
        [HttpGet("types")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<DocumentTypeDto>> GetDocumentTypes()
        {
            return await documentAppService.GetDocumentTypesAsync();
        }

        // GET document folders list
        [HttpGet("folders")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<DocumentFolderTypeDto>> GetDocumentFolders()
        {
            return await documentAppService.GetDocumentFolderTypesAsync();
        }

        [HttpGet("synchronize")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<(string ContainerName, string Filename)> SynchronizeImages(Guid id, string containerName, string fileName)
        {
            var request = $"{config.FactoryURL}/api/documents?id={id}";
            var client = clientFactory.CreateClient();
            var response = await client.GetAsync(request);
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var (containerNameToReturn, fileNameToReturn) = await fileHandlerService.UploadFromStreamAsync(stream, containerName, Path.GetExtension(fileName));
                return (containerNameToReturn, fileNameToReturn);
            }
        }
    }
}