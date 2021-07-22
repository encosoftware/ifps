using System.Threading.Tasks;

namespace IFPS.Integration.Application.Interfaces
{
    public interface ISynchronizationAppService
    {
        Task SyncronizeAccessoryMaterialToFactoryAsync();
        Task SyncronizeApplianceMaterialToFactoryAsync();
        Task SyncronizeDecorBoardMaterialToFactoryAsync();
        Task SyncronizeFoilMaterialToFactoryAsync();
        Task SyncronizeWorktopBoardMaterialToFactoryAsync();
        Task SyncronizeFurnitureUnitToFactoryAsync();
        Task SyncronizeFurnitureComponentToFactoryAsync();
        Task SyncronizeOrderToFactoryAsync();
        Task SyncronizeOrderToSalesAsync();
        Task SyncronizeImageToFactoryAsync();
        Task SyncronizeImageToSalesAsync();
        Task SyncronizeDocumentToFactoryAsync();
        Task SyncronizeDocumentToSalesAsync();
        Task SyncronizeGroupingCategoryToFactoryAsync();
        Task SyncronizeWebshopOrderToFactoryAsync();
        Task SyncronizeAccessoryFurnitureUnitToFactoryAsync();
    }
}
