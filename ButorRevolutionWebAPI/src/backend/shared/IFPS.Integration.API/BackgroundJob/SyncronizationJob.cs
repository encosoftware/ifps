using ENCO.DDD.Service;
using Hangfire;
using IFPS.Integration.Application.Interfaces;
using System.Threading.Tasks;

namespace IFPS.Integration.API.BackgroundJob
{
    public class SynchronizationJob : ApplicationService, ISynchronizationJob
    {
        private readonly ISynchronizationAppService synchronizationAppService;

        private const int NUMBER_OF_ATTEMPTS = 1;

        public SynchronizationJob(
            IApplicationServiceDependencyAggregate aggregate,
            ISynchronizationAppService synchronizationAppService) : base(aggregate)
        {
            this.synchronizationAppService = synchronizationAppService;
        }

        [AutomaticRetry(Attempts = NUMBER_OF_ATTEMPTS)]
        public async Task Syncronise()
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
