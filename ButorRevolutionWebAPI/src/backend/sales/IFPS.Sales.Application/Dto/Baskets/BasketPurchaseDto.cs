using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class BasketPurchaseDto
    {
        public int? CustomerId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Note { get; set; }
        public string TaxNumber { get; set; }
        public bool GaveEmailConsent { get; set; }
        public AddressCreateDto DelieveryAddress { get; set; }
        public AddressCreateDto BillingAddress { get; set; }

        public Basket UpdateModelObject(Basket basket)
        {
            if (CustomerId == null)
            {
                basket.Name = Name;
                basket.EmailAddress = EmailAddress;
            }
            else
            {
                basket.CustomerId = CustomerId;
            }
            basket.TaxNumber = TaxNumber;
            basket.GaveEmailConsent = GaveEmailConsent;
            basket.DelieveryAddress = DelieveryAddress.CreateModelObject();
            basket.BillingAddress = BillingAddress.CreateModelObject();
            basket.Note = Note;
            return basket;
        }
    }
}
