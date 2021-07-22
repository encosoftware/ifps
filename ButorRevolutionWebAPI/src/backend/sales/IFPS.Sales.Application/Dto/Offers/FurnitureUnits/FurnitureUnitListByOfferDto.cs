using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitListByOfferDto
    {
        public int OrderedFurnitureUnitId { get; set; }

        public Guid FurnitureUnitId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public double Width { get; set; }

        public double Depth { get; set; }

        public double Height { get; set; }

        public PriceListDto CurrentPrice { get; set; }

        public int Quantity { get; set; }

        public ImageThumbnailDetailsDto Image { get; set; }

        public PriceListDto SubTotal { get; set; }

        public FurnitureUnitListByOfferDto(OrderedFurnitureUnit orderedFurnitureUnit)
        {
            var price = orderedFurnitureUnit.FurnitureUnit.CurrentPrice.Price;

            OrderedFurnitureUnitId = orderedFurnitureUnit.Id;
            FurnitureUnitId = orderedFurnitureUnit.FurnitureUnitId;
            Code = orderedFurnitureUnit.FurnitureUnit.Code;
            Name = orderedFurnitureUnit.FurnitureUnit.Description;
            Width = orderedFurnitureUnit.FurnitureUnit.Width;
            Depth = orderedFurnitureUnit.FurnitureUnit.Depth;
            Height = orderedFurnitureUnit.FurnitureUnit.Height;
            CurrentPrice = new PriceListDto(price);
            Image = new ImageThumbnailDetailsDto(orderedFurnitureUnit.FurnitureUnit.Image);
            Quantity = orderedFurnitureUnit.Quantity;
            SubTotal = new PriceListDto(price, Quantity);
        }
    }
}
