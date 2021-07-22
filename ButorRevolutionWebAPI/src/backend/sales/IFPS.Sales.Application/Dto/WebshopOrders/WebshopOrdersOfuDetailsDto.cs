using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopOrdersOfuDetailsDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public int Quantity { get; set; }
        public PriceListDto UnitPrice { get; set; }
        public PriceListDto SubTotal { get; set; }
        public ImageDetailsDto Image { get; set; }

        public WebshopOrdersOfuDetailsDto(OrderedFurnitureUnit orderedFurnitureUnit)
        {
            Name = orderedFurnitureUnit.FurnitureUnit.Code;
            Description = orderedFurnitureUnit.FurnitureUnit.Description;
            Width = orderedFurnitureUnit.FurnitureUnit.Width;
            Height = orderedFurnitureUnit.FurnitureUnit.Height;
            Depth = orderedFurnitureUnit.FurnitureUnit.Depth;
            Quantity = orderedFurnitureUnit.Quantity;
            UnitPrice = new PriceListDto(orderedFurnitureUnit.UnitPrice);
            SubTotal = new PriceListDto(orderedFurnitureUnit.UnitPrice) { Value = orderedFurnitureUnit.UnitPrice.Value * orderedFurnitureUnit.Quantity };
            Image = new ImageDetailsDto(orderedFurnitureUnit.FurnitureUnit.Image);
        }
    }
}
