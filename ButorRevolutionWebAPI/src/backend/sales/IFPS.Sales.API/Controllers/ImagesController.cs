using IFPS.Sales.API.Common;
using IFPS.Sales.API.Infrastructure.Swagger;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImagesController : IFPSControllerBase
    {
        private const string OPNAME = "Images";

        private readonly IFileHandlerService fileHandlerService;
        private readonly IHttpClientFactory clientFactory;
        private readonly LocalConnectionConfiguration config;

        public ImagesController(
            IFileHandlerService fileHandlerService,
            IHttpClientFactory clientFactory,
            IOptions<LocalConnectionConfiguration> options)
        {
            this.fileHandlerService = fileHandlerService;
            this.clientFactory = clientFactory;
            this.config = options.Value;
        }

        // POST image
        [HttpPost]
        [Authorize]
        [SwaggerFileUpload]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<(string ContainerName, string Filename)> CreateImage([FromForm] IFormFile file, [FromQuery] string container)
        {
            using (var stream = file.OpenReadStream())
            {
                var (containerName, fileName) = await fileHandlerService.UploadFromStreamAsync(stream, container, Path.GetExtension(file.FileName));
                return (containerName, fileName);
            }
        }


        // POST image
        [HttpPost("multiple")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<(string ContainerName, string Filename)>> CreateImages([FromForm] List<IFormFile> files, [FromQuery] string container)
        {
            var images = new List<(string, string)>();
            foreach (var file in files)
            {
                using (var stream = file.OpenReadStream())
                {
                    var (containerName, fileName) = await fileHandlerService.UploadFromStreamAsync(stream, container, Path.GetExtension(file.FileName));
                    images.Add((containerName, fileName));
                }
            }
            return images;
        }

        // GET image
        [HttpGet]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public FileContentResult GetImage([FromQuery]string containerName, [FromQuery]string fileName)
        {
            return File(fileHandlerService.GetImage(containerName, fileName).RawData, System.Net.Mime.MediaTypeNames.Image.Jpeg);
        }

        [HttpGet("synchronize")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<(string ContainerName, string Filename)> SynchronizeImages(string containerName, string fileName)
        {
            var request = $"{config.FactoryURL}/api/images?containerName={containerName}&fileName={fileName}";
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