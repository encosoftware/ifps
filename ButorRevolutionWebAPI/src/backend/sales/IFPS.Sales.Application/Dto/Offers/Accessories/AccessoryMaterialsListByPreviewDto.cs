using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class AccessoryMaterialsListByPreviewDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public PriceListDto Price { get; set; }

        public int Quantity { get; set; }

        public ImageThumbnailDetailsDto Image { get; set; }

        public PriceListDto SubTotal { get; set; }

        public AccessoryMaterialsListByPreviewDto(AccessoryMaterialFurnitureUnit accessory)
        {
            var price = accessory.Accessory.CurrentPrice.Price;

            Code = accessory.Accessory.Code;
            Name = accessory.Name;
            Price = new PriceListDto(price);
            Quantity = accessory.AccessoryAmount;
            Image = new ImageThumbnailDetailsDto(accessory.Accessory.Image);
            SubTotal = new PriceListDto(price, Quantity);
        }
    }
}
