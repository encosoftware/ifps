using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class RequiresCreateDto
    {
        public bool IsPrivatePerson { get; set; }
        public PriceCreateDto Budget { get; set; }
        public string Description { get; set; }

        public RequiresCreateDto()
        {

        }

        public Order UpdateModelObject(Order order)
        {
            order.IsPrivatePerson = IsPrivatePerson;
            order.Budget = Budget.CreateModelObject();
            order.DescriptionByOffer = Description;

            return order;
        }
    }
}
