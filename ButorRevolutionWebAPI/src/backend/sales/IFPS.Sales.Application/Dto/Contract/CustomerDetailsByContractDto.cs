using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class CustomerDetailsByContractDto
    {
        public string Name { get; set; }
        public AddressDetailsDto CorrespondentAddress { get; set; }
        public AddressDetailsDto ShippingAddress { get; set; }

        public CustomerDetailsByContractDto(Order order)
        {
            if(order.IsPrivatePerson == true)
            {
                Name = order.Customer.User.CurrentVersion.Name;
                CorrespondentAddress = new AddressDetailsDto(order.Customer.User.CurrentVersion.ContactAddress);
                ShippingAddress = new AddressDetailsDto(order.Customer.User.CurrentVersion.ContactAddress);
            }
            else
            {
                Name = order.Customer.User.Company.Name;
                CorrespondentAddress = new AddressDetailsDto(order.Customer.User.Company.CurrentVersion.HeadOffice);
                ShippingAddress = new AddressDetailsDto(order.Customer.User.Company.CurrentVersion.HeadOffice);
            }
        }
    }
}
