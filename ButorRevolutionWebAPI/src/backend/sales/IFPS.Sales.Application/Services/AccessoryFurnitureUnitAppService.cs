using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Repositories;
using System.Threading.Tasks;
using IFPS.Sales.Domain.Helpers;

namespace IFPS.Sales.Application.Services
{
    public class AccessoryFurnitureUnitAppService : ApplicationService, IAccessoryFurnitureUnitAppService
    {
        private const double convertFromPercentage = 100.0;
        private readonly IAccessoryFurnitureUnitRepository accessoryFurnitureUnitRepository;
        private readonly IAccessoryMaterialRepository accessoryMaterialRepository;
        private readonly IFurnitureUnitRepository furnitureUnitRepository;

        public AccessoryFurnitureUnitAppService(IApplicationServiceDependencyAggregate aggregate,
           IAccessoryFurnitureUnitRepository accessoryFurnitureUnitRepository,
           IAccessoryMaterialRepository accessoryMaterialRepository,
           IFurnitureUnitRepository furnitureUnitRepository)
            : base(aggregate)
        {
            this.accessoryFurnitureUnitRepository = accessoryFurnitureUnitRepository;
            this.accessoryMaterialRepository = accessoryMaterialRepository;
            this.furnitureUnitRepository = furnitureUnitRepository;
        }

        public async Task<int> CreateAccessoryFurnitureUnitAsync(AccessoryFurnitureUnitCreateDto accessoryFurnitureUnitCreateDto)
        {
            var accessoryFurnitureUnit = accessoryFurnitureUnitCreateDto.CreateModelObject();

            var material = await accessoryMaterialRepository.SingleIncludingAsync(ent => ent.Id == accessoryFurnitureUnitCreateDto.MaterialId, ent => ent.CurrentPrice.Price);
            var furnitureUnit = await furnitureUnitRepository.SingleIncludingAsync(ent => ent.Id == accessoryFurnitureUnitCreateDto.FurnitureUnitId, ent => ent.CurrentPrice.MaterialCost, ent => ent.CurrentPrice.Price);

            var materialPrice = material.CurrentPrice.Price.Value * accessoryFurnitureUnitCreateDto.Amount;
            furnitureUnit.CurrentPrice.MaterialCost.Add(materialPrice);
            var value = materialPrice + (materialPrice * (material.TransactionMultiplier / convertFromPercentage));
            furnitureUnit.CurrentPrice.Price.Add(value);

            await accessoryFurnitureUnitRepository.InsertAsync(accessoryFurnitureUnit);
            await unitOfWork.SaveChangesAsync();
            return accessoryFurnitureUnit.Id;
        }

        public async Task<AccessoryFurnitureUnitDetailsDto> GetAccessoryFurnitureUnitDetailsAsync(int id)
        {
            var accessoryFurnitureUnit = await accessoryFurnitureUnitRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.Accessory.Image);
            return new AccessoryFurnitureUnitDetailsDto(accessoryFurnitureUnit);
        }

        public async Task UpdateAccessoryFurnitureUnitAsync(int id, AccessoryFurnitureUnitUpdateDto accessoryFurnitureUnitUpdateDto)
        {
            var accessoryFurnitureUnit = await accessoryFurnitureUnitRepository.SingleIncludingAsync(ent => ent.Id == id, 
                                ent => ent.Accessory.Image, ent => ent.Accessory.CurrentPrice.Price,
                                ent => ent.FurnitureUnit.CurrentPrice.Price, ent => ent.FurnitureUnit.CurrentPrice.MaterialCost);

            if (accessoryFurnitureUnit.AccessoryAmount != accessoryFurnitureUnitUpdateDto.Amount || accessoryFurnitureUnit.AccessoryId != accessoryFurnitureUnitUpdateDto.MaterialId)
            {
                var divMaterialPrice = accessoryFurnitureUnit.Accessory.CurrentPrice.Price.Value * accessoryFurnitureUnit.AccessoryAmount;
                accessoryFurnitureUnit.FurnitureUnit.CurrentPrice.MaterialCost.Div(divMaterialPrice);

                var divValue = divMaterialPrice + (divMaterialPrice * (accessoryFurnitureUnit.Accessory.TransactionMultiplier / convertFromPercentage));
                accessoryFurnitureUnit.FurnitureUnit.CurrentPrice.Price.Div(divValue);

                var addMaterialPrice = accessoryFurnitureUnit.Accessory.CurrentPrice.Price.Value * accessoryFurnitureUnitUpdateDto.Amount;
                accessoryFurnitureUnit.FurnitureUnit.CurrentPrice.MaterialCost.Add(addMaterialPrice);

                var addValue = addMaterialPrice + (addMaterialPrice * (accessoryFurnitureUnit.Accessory.TransactionMultiplier / convertFromPercentage));
                accessoryFurnitureUnit.FurnitureUnit.CurrentPrice.Price.Add(addValue);
            }

            accessoryFurnitureUnit = accessoryFurnitureUnitUpdateDto.UpdateModelObject(accessoryFurnitureUnit);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAccessoryFurnitureUnitAsync(int id)
        {
            var accessoryFurnitureUnit = await accessoryFurnitureUnitRepository.SingleAsync(ent => ent.Id == id);
            var material = await accessoryMaterialRepository.SingleIncludingAsync(ent => ent.Id == accessoryFurnitureUnit.AccessoryId, ent => ent.CurrentPrice.Price);
            var furnitureUnit = await furnitureUnitRepository.SingleIncludingAsync(ent => ent.Id == accessoryFurnitureUnit.FurnitureUnitId, ent => ent.CurrentPrice.Price, ent => ent.CurrentPrice.MaterialCost);

            var materialPrice = material.CurrentPrice.Price.Value * accessoryFurnitureUnit.AccessoryAmount;
            furnitureUnit.CurrentPrice.MaterialCost.Div(materialPrice);
            var value = materialPrice + (materialPrice * (material.TransactionMultiplier / convertFromPercentage));
            furnitureUnit.CurrentPrice.Price.Div(value);

            await accessoryFurnitureUnitRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
