using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Application.Dto
{
    public class BoardMaterialPreviewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool HasFiberDirection { get; set; }
        public CategoryListDto Category { get; set; }
        public ImageThumbnailListDto Image { get; set; }


        //public static Func<BoardMaterial, BoardMaterialPreviewDto> FromEntity = entity => new BoardMaterialPreviewDto()
        //{
        //    Id = entity.Id,
        //    Name = entity.Translations.GetCurrentTranslation().Name,
        //    Description = entity.Translations.GetCurrentTranslation().Description,
        //    Code = entity.Code,
        //    HasFiberDirection = entity.HasFiberDirection,
        //    Category = new CategoryListDto(entity.Category),
        //    ImageThumbnailListDto = new ImageThumbnailListDto(entity.Image),
        //};


        public static Func<BoardMaterial, BoardMaterialPreviewDto> FromEntity = entity => new BoardMaterialPreviewDto()
        {
            Id = entity.Id,
            Name = entity.Translations.GetCurrentTranslation()?.Name,
            Description = entity.Description,
            Code = entity.Code,
            HasFiberDirection = entity.HasFiberDirection,
            Category = CategoryListDto.FromEntity(entity.Category),
            Image = new ImageThumbnailListDto(entity.Image),
        };
    }
}
