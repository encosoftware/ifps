using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Helpers;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class FurnitureComponentAppService : ApplicationService, IFurnitureComponentAppService
    {
        private const double convertFromPercentage = 100.0;
        private readonly IFurnitureComponentRepository furnitureComponentRepository;
        private readonly IDecorBoardMaterialRepository decorBoardMaterialRepository;
        private readonly IFoilMaterialRepository foilMaterialRepository;
        private readonly IFurnitureUnitRepository furnitureUnitRepository;
        private readonly IFileHandlerService fileHandlerService;

        public FurnitureComponentAppService(IApplicationServiceDependencyAggregate aggregate,
           IFurnitureComponentRepository furnitureComponentRepository,
           IDecorBoardMaterialRepository decorBoardMaterialRepository,
           IFoilMaterialRepository foilMaterialRepository,
           IFurnitureUnitRepository furnitureUnitRepository,
           IFileHandlerService fileHandlerService)
            : base(aggregate)
        {
            this.furnitureComponentRepository = furnitureComponentRepository;
            this.decorBoardMaterialRepository = decorBoardMaterialRepository;
            this.furnitureUnitRepository = furnitureUnitRepository;
            this.foilMaterialRepository = foilMaterialRepository;
            this.fileHandlerService = fileHandlerService;
        }

        public async Task<Guid> CreateFurnitureComponentAsync(FurnitureComponentCreateDto furnitureComponentCreateDto)
        {
            var furnitureComponent = furnitureComponentCreateDto.CreateModelObject();
            var material = await decorBoardMaterialRepository.SingleAsync(ent=> ent.Id == furnitureComponentCreateDto.MaterialId);
            furnitureComponent.ImageId = material.ImageId.GetValueOrDefault();

            var furnitureUnit = await furnitureUnitRepository.SingleIncludingAsync(ent => ent.Id == furnitureComponentCreateDto.FurnitureUnitId, ent => ent.CurrentPrice.MaterialCost, ent => ent.CurrentPrice.Price);
            await AddMaterialPriceAsync(
                furnitureComponentCreateDto.Amount,
                furnitureUnit,
                material.Id,
                furnitureComponentCreateDto.TopFoilId,
                furnitureComponentCreateDto.LeftFoilId,
                furnitureComponentCreateDto.RightFoilId,
                furnitureComponentCreateDto.BottomFoilId);

            await furnitureComponentRepository.InsertAsync(furnitureComponent);
            await unitOfWork.SaveChangesAsync();
            return furnitureComponent.Id;
        }

        public async Task<FurnitureComponentDetailsDto> GetFurnitureComponentDetailsAsync(Guid id)
        {
            var furnitureComponent = await furnitureComponentRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.BoardMaterial, ent => ent.TopFoil, ent => ent.BottomFoil, ent => ent.RightFoil, ent => ent.LeftFoil, ent => ent.Image);
            return new FurnitureComponentDetailsDto(furnitureComponent);
        }

        public async Task UpdateFurnitureComponentAsync(Guid id, FurnitureComponentUpdateDto furnitureComponentUpdateDto)
        {
            var furnitureComponent = await furnitureComponentRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.BoardMaterial, ent => ent.TopFoil, ent => ent.BottomFoil, ent => ent.RightFoil, ent => ent.LeftFoil, ent => ent.Image);
            var furnitureUnit = await furnitureUnitRepository.SingleIncludingAsync(ent => ent.Id == furnitureComponent.FurnitureUnitId, ent => ent.CurrentPrice.Price, ent => ent.CurrentPrice.MaterialCost);

            await CheckPropertiesEqualityAsync(furnitureUnit,
                furnitureComponent.Amount,
                furnitureComponentUpdateDto.Amount,
                furnitureComponent.BoardMaterialId,
                furnitureComponent.TopFoilId,
                furnitureComponent.LeftFoilId,
                furnitureComponent.RightFoilId,
                furnitureComponent.BottomFoilId,
                furnitureComponentUpdateDto.MaterialId,
                furnitureComponentUpdateDto.TopFoilId,
                furnitureComponentUpdateDto.LeftFoilId,
                furnitureComponentUpdateDto.RightFoilId,
                furnitureComponentUpdateDto.BottomFoilId);

            furnitureComponent = furnitureComponentUpdateDto.UpdateModelObject(furnitureComponent);
            furnitureComponent.Image = await fileHandlerService.UpdateImage(furnitureComponent.ImageId, furnitureComponentUpdateDto.ImageUpdateDto.ContainerName, furnitureComponentUpdateDto.ImageUpdateDto.FileName);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteFurnitureComponentAsync(Guid id)
        {
            var furnitureComponent = await furnitureComponentRepository.SingleIncludingAsync(ent => ent.Id == id);
            var furnitureUnit = await furnitureUnitRepository.SingleIncludingAsync(ent => ent.Id == furnitureComponent.FurnitureUnitId, ent => ent.CurrentPrice.Price, ent => ent.CurrentPrice.MaterialCost);

            await DivMaterialPriceAsync(
                furnitureComponent.Amount,
                furnitureUnit,
                furnitureComponent.BoardMaterialId,
                furnitureComponent.TopFoilId,
                furnitureComponent.LeftFoilId,
                furnitureComponent.RightFoilId,
                furnitureComponent.BottomFoilId);

            await furnitureComponentRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        #region Private methods

        private async Task AddMaterialPriceAsync(int amount, FurnitureUnit furnitureUnit, Guid? boardMaterialId, Guid? topFoilId, Guid? leftFoilId, Guid? rightFoilId, Guid? bottomFoilId)
        {
            if (boardMaterialId != null)
            {
                await AddBoardPriceValueAsync(amount, furnitureUnit, boardMaterialId.Value);
            }

            if (topFoilId != null)
            {
                await AddFoilPriceValueAsync(amount, furnitureUnit, topFoilId.Value);
            }

            if (leftFoilId != null)
            {
                await AddFoilPriceValueAsync(amount, furnitureUnit, leftFoilId.Value);
            }

            if (rightFoilId != null)
            {
                await AddFoilPriceValueAsync(amount, furnitureUnit, rightFoilId.Value);
            }

            if (bottomFoilId != null)
            {
                await AddFoilPriceValueAsync(amount, furnitureUnit, bottomFoilId.Value);
            }
        }

        private async Task AddFoilPriceValueAsync(int amount, FurnitureUnit furnitureUnit, Guid foilMaterialId)
        {
            var foilMaterial = await foilMaterialRepository.SingleIncludingAsync(ent => ent.Id == foilMaterialId, ent => ent.CurrentPrice.Price);
            var materialPrice = foilMaterial.CurrentPrice.Price.Value * amount;

            furnitureUnit.CurrentPrice.MaterialCost.Add(materialPrice);
            double multiplier = foilMaterial.TransactionMultiplier / convertFromPercentage;

            var value = materialPrice + (materialPrice * multiplier);
            furnitureUnit.CurrentPrice.Price.Add(value);
        }

        private async Task AddBoardPriceValueAsync(int amount, FurnitureUnit furnitureUnit, Guid foilMaterialId)
        {
            var boardMaterial = await decorBoardMaterialRepository.SingleIncludingAsync(ent => ent.Id == foilMaterialId, ent => ent.CurrentPrice.Price);
            var materialPrice = boardMaterial.CurrentPrice.Price.Value * amount;

            furnitureUnit.CurrentPrice.MaterialCost.Add(materialPrice);
            double multiplier = boardMaterial.TransactionMultiplier / convertFromPercentage;

            var value = (materialPrice) + (materialPrice * multiplier);
            furnitureUnit.CurrentPrice.Price.Add(value);
        }

        private async Task DivMaterialPriceAsync(int amount, FurnitureUnit furnitureUnit, Guid? boardMaterialId, Guid? topFoilId, Guid? leftFoilId, Guid? rightFoilId, Guid? bottomFoilId)
        {
            if (boardMaterialId != null)
            {
                await DivBoardPriceValueAsync(amount, furnitureUnit, boardMaterialId.Value);
            }

            if (topFoilId != null)
            {
                await DivFoilPriceValueAsync(amount, furnitureUnit, topFoilId.Value);
            }

            if (leftFoilId != null)
            {
                await DivFoilPriceValueAsync(amount, furnitureUnit, leftFoilId.Value);
            }

            if (rightFoilId != null)
            {
                await DivFoilPriceValueAsync(amount, furnitureUnit, rightFoilId.Value);
            }

            if (bottomFoilId != null)
            {
                await DivFoilPriceValueAsync(amount, furnitureUnit, bottomFoilId.Value);
            }
        }

        private async Task DivFoilPriceValueAsync(int amount, FurnitureUnit furnitureUnit, Guid foilMaterialId)
        {
            var foilMaterial = await foilMaterialRepository.SingleIncludingAsync(ent => ent.Id == foilMaterialId, ent => ent.CurrentPrice.Price);
            var materialPrice = foilMaterial.CurrentPrice.Price.Value * amount;

            furnitureUnit.CurrentPrice.MaterialCost.Div(materialPrice);
            double multiplier = foilMaterial.TransactionMultiplier / convertFromPercentage;

            var value = (materialPrice) + (materialPrice * multiplier);
            furnitureUnit.CurrentPrice.Price.Div(value);
        }

        private async Task DivBoardPriceValueAsync(int amount, FurnitureUnit furnitureUnit, Guid boardMaterialId)
        {
            var boardMaterial = await decorBoardMaterialRepository.SingleIncludingAsync(ent => ent.Id == boardMaterialId, ent => ent.CurrentPrice.Price);
            var materialPrice = boardMaterial.CurrentPrice.Price.Value * amount;

            furnitureUnit.CurrentPrice.MaterialCost.Div(materialPrice);
            double multiplier = boardMaterial.TransactionMultiplier / convertFromPercentage;

            var value = (materialPrice) + (materialPrice * multiplier);
            furnitureUnit.CurrentPrice.Price.Div(value);
        }

        private async Task CheckPropertiesEqualityAsync(FurnitureUnit furnitureUnit, int modelAmount, int dtoAmount, Guid? modelBoardMaterialId, Guid? modelTopFoilId, 
            Guid? modelLeftFoilId, Guid? modelRightFoilId, Guid? modelBottomFoilId, 
            Guid? dtoBoardMaterialId, Guid? dtoTopFoilId, Guid? dtoLeftFoilId, Guid? dtoRightFoilId, Guid? dtoBottomFoilId)
        {
            if (modelAmount != dtoAmount || modelBoardMaterialId != dtoBoardMaterialId || modelTopFoilId != dtoTopFoilId || modelLeftFoilId != dtoLeftFoilId ||
                modelRightFoilId != dtoRightFoilId || modelBottomFoilId != dtoBottomFoilId)
            {
                await DivMaterialPriceAsync(modelAmount, furnitureUnit, modelBoardMaterialId, modelTopFoilId, modelLeftFoilId, modelRightFoilId, modelBottomFoilId);

                await AddMaterialPriceAsync(dtoAmount, furnitureUnit, dtoBoardMaterialId, dtoTopFoilId, dtoLeftFoilId, dtoRightFoilId, dtoBottomFoilId);
            }
        }

        #endregion
    }
}
