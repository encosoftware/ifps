using IFPS.Integration.API.Common;
using IFPS.Integration.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace IFPS.Integration.API.Controllers
{
    [Route("api/synchronization")]
    [ApiController]
    public class SynchronizationsController : IFPSControllerBase
    {
        private const string OPNAME = "Synchronization";

        private readonly ISynchronizationAppService synchronizationAppService;

        public SynchronizationsController(
           ISynchronizationAppService synchronizationAppService)
        {
            this.synchronizationAppService = synchronizationAppService;
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task Synchronize()
        {
            //await synchronizationAppService.SyncronizeImageToFactoryAsync();
            //await synchronizationAppService.SyncronizeImageToSalesAsync();
            //await synchronizationAppService.SyncronizeDocumentToFactoryAsync();
            //await synchronizationAppService.SyncronizeDocumentToSalesAsync();
            await synchronizationAppService.SyncronizeGroupingCategoryToFactoryAsync();
            await synchronizationAppService.SyncronizeAccessoryMaterialToFactoryAsync();
            await synchronizationAppService.SyncronizeApplianceMaterialToFactoryAsync();
            await synchronizationAppService.SyncronizeDecorBoardMaterialToFactoryAsync();
            await synchronizationAppService.SyncronizeFoilMaterialToFactoryAsync();
            await synchronizationAppService.SyncronizeWorktopBoardMaterialToFactoryAsync();
            await synchronizationAppService.SyncronizeFurnitureUnitToFactoryAsync();
            await synchronizationAppService.SyncronizeFurnitureComponentToFactoryAsync();
            await synchronizationAppService.SyncronizeAccessoryFurnitureUnitToFactoryAsync();
            await synchronizationAppService.SyncronizeOrderToFactoryAsync();
            await synchronizationAppService.SyncronizeOrderToSalesAsync();
            await synchronizationAppService.SyncronizeWebshopOrderToFactoryAsync();
        }
    }
}
