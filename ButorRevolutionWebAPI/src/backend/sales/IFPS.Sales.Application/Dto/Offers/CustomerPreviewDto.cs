using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class CustomerPreviewDto
    {
        public string Name { get; set; }
        public AddressDetailsDto ShippingAddress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public CustomerPreviewDto(Customer customer, Order order)
        {
            if (order.IsPrivatePerson)
            {
                Name = customer.User.CurrentVersion.Name;
                ShippingAddress = order.ShippingAddress == null ? new AddressDetailsDto(customer.User.CurrentVersion.ContactAddress) : new AddressDetailsDto(order.ShippingAddress);
                Email = customer.User.Email;
                Phone = customer.User.CurrentVersion.Phone;
            }
            else
            {
                Name = customer.User.Company.Name;
                ShippingAddress = new AddressDetailsDto(customer.User.Company.CurrentVersion.HeadOffice);
                Email = customer.User.Company.CurrentVersion.ContactPerson?.Email;
                Phone = customer.User.Company.CurrentVersion.ContactPerson?.CurrentVersion?.Phone;
            }
        }
    }
}
