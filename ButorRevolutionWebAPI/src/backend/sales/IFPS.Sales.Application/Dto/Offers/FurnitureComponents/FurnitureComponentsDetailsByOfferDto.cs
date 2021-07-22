using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureComponentsDetailsByOfferDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? BoardMaterialId { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public int Amount { get; set; }

        public ImageThumbnailDetailsDto Image { get; set; }

        public Guid? TopFoilId { get; set; }

        public string TopFoilName { get; set; }

        public Guid? BottomFoilId { get; set; }

        public string BottomFoilName { get; set; }

        public Guid? RightFoilId { get; set; }

        public string RightFoilName { get; set; }

        public Guid? LeftFoilId { get; set; }

        public string LeftFoilName { get; set; }

        public FurnitureComponentsDetailsByOfferDto(FurnitureComponent component)
        {
            Id = component.Id;
            Name = component.Name;
            BoardMaterialId = component.BoardMaterialId;
            Width = component.Width;
            Height = component.Length;
            Amount = component.Amount;
            Image = new ImageThumbnailDetailsDto(component.Image);
            TopFoilId = component.TopFoilId;
            TopFoilName = component.TopFoil.Description;
            BottomFoilId = component.BottomFoilId;
            BottomFoilName = component.BottomFoil.Description;
            RightFoilId = component.RightFoilId;
            RightFoilName = component.RightFoil.Description;
            LeftFoilId = component.LeftFoilId;
            LeftFoilName = component.LeftFoil.Description;
        }
    }
}
