using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class RequiresDetailsDto
    {
        public bool IsPrivatePerson { get; set; }
        public string CompanyName { get; set; }
        public PriceListDto Budget { get; set; }
        public string Description { get; set; }
        public int? CompanyId { get; set; }

        public RequiresDetailsDto(Order order)
        {
            IsPrivatePerson = order.IsPrivatePerson;
            if(IsPrivatePerson == false)
            {
                CompanyName = order.Customer.User.Company.Name;
            }
            Budget = new PriceListDto(order.Budget);
            Description = order.DescriptionByOffer;
            CompanyId = order.Customer.User.CompanyId;
        }
    }
}