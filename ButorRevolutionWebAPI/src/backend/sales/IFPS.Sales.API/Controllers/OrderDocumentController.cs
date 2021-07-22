using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/orders/{orderId}/documents")]
    [ApiController]
    public class OrderDocumentController : IFPSControllerBase
    {
        private const string OPNAME = "OrdersDocuments";

        private readonly IOrderAppService orderAppService;
        private readonly IFileHandlerService fileHandlerService;

        public OrderDocumentController(
            IOrderAppService orderAppService,
            IFileHandlerService fileHandlerService)
        {
            this.orderAppService = orderAppService;
            this.fileHandlerService = fileHandlerService;
        }

        // GET DocumentGroups list
        [HttpGet]
        [Authorize(Policy = "GetOrderDocuments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<DocumentGroupDto>> Get(Guid orderId)
        {
            return orderAppService.GetDocumentsOfOrder(orderId);
        }

        // POST: api/UploadDocument
        [HttpPost]
        [Authorize(Policy = "UpdateOrderDocuments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task Post(Guid orderId, [FromBody] DocumentUploadDto documentDto)
        {
            return orderAppService.UploadDocuments(orderId, documentDto);
        }

        // PUT Approve or decline document group version
        [HttpPut("versions/{documentGroupVersionId}")]
        [Authorize(Policy = "ApproveOrderDocuments")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task ApproveDocuments(Guid orderId, int documentGroupVersionId, [FromQuery] DocumentStateEnum result)
        {
            return orderAppService.ApproveDocuments(orderId, documentGroupVersionId, result, GetCallerId());
        }

        //Download all documents
        [HttpGet("download")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<FileStreamResult> DownloadAllDocuments(Guid orderId)
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

            await orderAppService.DownloadDocuments(orderId, memoryStream);

            memoryStream.Position = 0;
            await memoryStream.FlushAsync();
            return File(memoryStream, "application/octet-stream", $"{ Clock.Now.ToString("yyyy_MM_dd_hh:mm:ss")}.zip");
        }

        // GET DocumentGroupVersionFiles as zip
        [HttpGet("versions/{documentGroupVersionId}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<FileStreamResult> GetDocumentGroupVersionFilesAsZip(int groupVersionId)
        {   
            var stream = new System.IO.MemoryStream();

            await fileHandlerService.DownloadFromContainer(stream, "thumbnail_zanussi_ZDF22002XA.jpg", "Content");
            stream.Position = 0;

            return File(stream, "application/octet-stream", Clock.Now.ToString("yyyyMMdd_hhmmss") + ".jpg");
        }
    }
}
