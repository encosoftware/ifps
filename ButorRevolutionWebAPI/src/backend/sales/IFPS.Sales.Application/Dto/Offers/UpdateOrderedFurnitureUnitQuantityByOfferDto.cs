using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class UpdateOrderedFurnitureUnitQuantityByOfferDto
    {

        public int Quantity { get; set; }

        public UpdateOrderedFurnitureUnitQuantityByOfferDto()
        {

        }

        public OrderedFurnitureUnit UpdateModelObject(OrderedFurnitureUnit orderedFurnitureUnit)
        {
            orderedFurnitureUnit.Quantity = Quantity;

            return orderedFurnitureUnit;
        }
    }
}
