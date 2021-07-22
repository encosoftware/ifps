using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;

namespace IFPS.Factory.Application.Services
{
    public class DecorBoardMaterialAppService : ApplicationService, IDecorBoardMaterialAppService
    {
        private readonly IDecorBoardMaterialRepository decorBoardMaterialRepository;
        private readonly IFileHandlerService fileHandlerService;

        public DecorBoardMaterialAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IDecorBoardMaterialRepository decorBoardMaterialRepository,
            IFileHandlerService fileHandlerService)
            : base(aggregate)
        {
            this.decorBoardMaterialRepository = decorBoardMaterialRepository;
            this.fileHandlerService = fileHandlerService;
        }

        public async Task CreateBoardsFromFileAsync(string container, string fileName)
        {
            var path = container + fileName;

            using (var reader = new StreamReader(path))
            {
                var headerLine = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var record = reader.ReadLine();
                    var values = record.Split(";");

                    var decorBoardDto = new DecorBoardMaterialCreateFromFileDto();
                    var newDecorBoard = decorBoardDto.CreateModelObject(values[0], double.Parse(values[1], CultureInfo.InvariantCulture), bool.Parse(values[2]));
                    newDecorBoard.ImageId = await fileHandlerService.InsertImage(values[3], values[4]);

                    await decorBoardMaterialRepository.InsertAsync(newDecorBoard);
                }
            }

            await unitOfWork.SaveChangesAsync();

            System.IO.File.Delete(path);
        }
    }
}
