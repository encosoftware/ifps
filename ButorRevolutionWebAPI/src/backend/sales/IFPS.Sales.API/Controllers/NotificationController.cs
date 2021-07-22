using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : IFPSControllerBase
    {
        private const string OPNAME = "Notifications";
        private readonly INotificationAppService notificationAppService;

        public NotificationController(INotificationAppService notificationAppService)
        {
            this.notificationAppService = notificationAppService;
        }

        // GET Event Type list
        [HttpGet("events")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<EventTypeDto>> GetEvents()
        {
            return notificationAppService.GetNotificationEventTypes();
        }
    }
}