using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Dto.Cargos;
using System.IO;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface ICargoAppService
    {
        Task<int> CreateCargoFromRequiredMaterialsAsync(CargoCreateDto dto);
        Task<CargoDetailsPreviewDto> GetCargoDetailsForPreviewAsync(int cargoId);
        Task<PagedListDto<CargoListDto>> ListCargos(CargoFilterDto filterDto);
        Task<PagedListDto<CargoListByStockDto>> ListCargosByStock(CargoFilterDto filterDto);
        Task DeleteCargoAsync(int id);
        Task<CargoDetailsDto> CargoDetailsAsync(int cargoId);
        Task UpdateProductsByCargo(int cargoId, CargoUpdateDto dto);
        Task<CargoDetailsWithAllInformationDto> CargoDetailsWithAllInformationAsync(int cargoId);
        Task<CargoStockDetailsDto> GetCargoStockDetailsAsync(int cargoId);
        Task UpdateCargoStock(int cargoId, UpdateCargoStockDto dto);
        Task<string> ExportCsvAsync(Stream stream, CargoFilterDto filterDto);
    }
}
