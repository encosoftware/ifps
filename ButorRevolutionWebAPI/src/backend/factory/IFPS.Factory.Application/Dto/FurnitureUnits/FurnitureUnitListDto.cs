using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class FurnitureUnitListDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public CategoryListDto Category { get; set; }
        public bool HasCncFile { get; set; }
        public ImageThumbnailListDto ImageThumbnail { get; set; }

        public FurnitureUnitListDto(FurnitureUnit furnitureUnit)
        {
            Id = furnitureUnit.Id;
            Code = furnitureUnit.Code;
            Description = furnitureUnit.Description;
            Category = new CategoryListDto(furnitureUnit.Category);
            HasCncFile = furnitureUnit.HasCncFile;
            ImageThumbnail = new ImageThumbnailListDto(furnitureUnit.Image);
        }

        public static Func<FurnitureUnit, FurnitureUnitListDto> FromEntity = entity => new FurnitureUnitListDto(entity);
    }
}