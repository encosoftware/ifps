using IFPS.Factory.API.Common;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
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
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<(string ContainerName, string Filename)> CreateImage([FromForm] IFormFile file, [FromQuery] string container)
        {
            using (var stream = file.OpenReadStream())
            {
                var (containerName, fileName) = await fileHandlerService.UploadFromStreamAsync(stream, container, Path.GetExtension(file.FileName));
                return (containerName, fileName);
            }
        }

        // GET image
        [HttpGet]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public FileContentResult GetImage(string containerName, string fileName)
        {
            return File(fileHandlerService.GetImage(containerName, fileName).RawData, System.Net.Mime.MediaTypeNames.Image.Jpeg);
        }

        [HttpGet("synchronize")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<(string ContainerName, string Filename)> SynchronizeImages(string containerName, string fileName)
        {
            var request = $"{config.SalesURL}/api/images?containerName={containerName}&fileName={Uri.EscapeUriString(fileName)}";
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