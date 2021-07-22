using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Services.Interfaces;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class DataImportAppService : ApplicationService, IDataImportAppService
    {

        private readonly IAccessoryMaterialService accessoryMaterialService;
        private readonly IApplianceMaterialService applianceMaterialService;
        private readonly IFoilMaterialService foilMaterialService;
        private readonly IDecorBoardMaterialService decorBoardMaterialService;
        private readonly IWorktopBoardMaterialService worktopBoardMaterialService;

        public DataImportAppService(IApplicationServiceDependencyAggregate aggregate,
            IAccessoryMaterialService accessoryMaterialService,
            IApplianceMaterialService applianceMaterialService,
            IFoilMaterialService foilMaterialService,
            IDecorBoardMaterialService decorBoardMaterialService,
            IWorktopBoardMaterialService worktopBoardMaterialService
            )
            : base(aggregate)
        {
            this.accessoryMaterialService = accessoryMaterialService;
            this.applianceMaterialService = applianceMaterialService;
            this.foilMaterialService = foilMaterialService;
            this.decorBoardMaterialService = decorBoardMaterialService;
            this.worktopBoardMaterialService = worktopBoardMaterialService;
        }

        public async Task ImportMaterialsFromCsv(MaterialImportDto materialImportDto)
        {
            await accessoryMaterialService.CreateAccessoryMaterialsFromCsv(materialImportDto.ContainerName, materialImportDto.AccessoryCsvFileName);
            await applianceMaterialService.CreateApplianceMaterialsFromCsv(materialImportDto.ContainerName, materialImportDto.ApplianceCsvFileName);
            await foilMaterialService.CreateFoilMaterialsFromCsv(materialImportDto.ContainerName, materialImportDto.FoilCsvFileName);
            await decorBoardMaterialService.CreateDecorBoardMaterialsFromCsv(materialImportDto.ContainerName, materialImportDto.DecorBoardCsvFileName);
            await worktopBoardMaterialService.CreateWorktopBoardMaterialsFromCsv(materialImportDto.ContainerName, materialImportDto.WorktopBoardCsvFileName);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
