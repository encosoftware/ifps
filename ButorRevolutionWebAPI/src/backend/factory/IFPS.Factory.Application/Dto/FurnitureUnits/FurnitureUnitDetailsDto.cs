using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class FurnitureUnitDetailsDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public ImageThumbnailDetailsDto ImageDetailsDto { get; set; }
        public List<FurnitureComponentListDto> FrontFurnitureComponents { get; set; }
        public List<FurnitureComponentListDto> CorpusFurnitureComponents { get; set; }
        public List<AccessoryFurnitureUnitListDto> AccessoryFurnitureComponents { get; set; }

        public FurnitureUnitDetailsDto(FurnitureUnit furnitureUnit)
        {
            Code = furnitureUnit.Code;
            Description = furnitureUnit.Description;
            CategoryId = furnitureUnit.CategoryId.GetValueOrDefault();
            Width = furnitureUnit.Width;
            Height = furnitureUnit.Height;
            Depth = furnitureUnit.Depth;
            ImageDetailsDto = new ImageThumbnailDetailsDto(furnitureUnit.Image);
            FrontFurnitureComponents = furnitureUnit.Components.Where(ent => ent.Type == FurnitureComponentTypeEnum.Front).Select(ent => new FurnitureComponentListDto(ent)).ToList();
            CorpusFurnitureComponents = furnitureUnit.Components.Where(ent => ent.Type == FurnitureComponentTypeEnum.Corpus).Select(ent => new FurnitureComponentListDto(ent)).ToList();
            AccessoryFurnitureComponents = furnitureUnit.Accessories.Select(ent => new AccessoryFurnitureUnitListDto(ent)).ToList();
        }
    }
}