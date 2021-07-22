using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitPreviewDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public CategoryListDto Category { get; set; }
        public ImageThumbnailListDto ImageThumbnail { get; set; }

        public FurnitureUnitPreviewDto()
        {
        }

        public static Func<FurnitureUnit, FurnitureUnitPreviewDto> FromEntity = entity => new FurnitureUnitPreviewDto()
        {
            Id = entity.Id,
            Code = entity.Code,
            Description = entity.Description,
            Category = new CategoryListDto(entity.Category),
            ImageThumbnail = new ImageThumbnailListDto(entity.Image),
        };
    }
}
