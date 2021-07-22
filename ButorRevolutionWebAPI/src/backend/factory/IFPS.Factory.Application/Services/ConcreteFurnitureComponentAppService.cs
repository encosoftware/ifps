using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.FileHandling;
using IFPS.Factory.Domain.Repositories;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IFPS.Factory.Domain.Services.Interfaces;

namespace IFPS.Factory.Application.Services
{
    public class ConcreteFurnitureComponentAppService : ApplicationService, IConcreteFurnitureComponentAppService
    {
        private readonly IConcreteFurnitureComponentRepository concreteFurnitureComponentRepository;
        private readonly IFileStorage fileStorage;
        private readonly IFileHandlerService fileHandlerService;

        public ConcreteFurnitureComponentAppService(
            IConcreteFurnitureComponentRepository concreteFurnitureComponentRepository,
            IFileStorage fileStorage,
            IApplicationServiceDependencyAggregate aggregate,
            IFileHandlerService fileHandlerService) : base(aggregate)
        {
            this.concreteFurnitureComponentRepository = concreteFurnitureComponentRepository;
            this.fileStorage = fileStorage;
            this.fileHandlerService = fileHandlerService;
        }

        public async Task GenerateQRCodeAsync(int cfcId)
        {
            var cfc = await concreteFurnitureComponentRepository.SingleIncludingAsync(ent => ent.Id == cfcId, ent => ent.ConcreteFurnitureUnit);
            var qrText = new
            {
                concreteFurnitureComponentId = cfcId,
                orderId = cfc.ConcreteFurnitureUnit.OrderId
            };
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            var imageName = $"qr_{cfcId}.png";
            var containerName = "QRCodes";
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, ImageFormat.Png);
            }
            var bitmapBytes = BitmapToBytes(qrCodeImage); //Convert bitmap into a byte array
            using (MemoryStream stream = new MemoryStream(bitmapBytes))
            {
                await fileStorage.SaveFileAsync(stream, containerName, imageName, true);
            }
            cfc.QRCodeId = await fileHandlerService.InsertImage(containerName, imageName);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ConcreteFurnitureComponentInformationListDto>> GetConcreteFurnitureComponentsAsync(Guid orderId)
        {
            var cfcs = await concreteFurnitureComponentRepository.GetAllListIncludingAsync(ent => ent.ConcreteFurnitureUnit.OrderId == orderId && ent.QRCodeId == null);
            foreach (var cfc in cfcs)
            {
                await GenerateQRCodeAsync(cfc.Id);
            }
            return await concreteFurnitureComponentRepository.GetAllListAsync(ent => ent.ConcreteFurnitureUnit.OrderId == orderId, ConcreteFurnitureComponentInformationListDto.Projection);
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }

        }
    }
}
