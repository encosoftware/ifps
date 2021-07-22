using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Sales.Domain.Model
{
    public class OfferInformation : FullAuditedEntity
    {
        /// <summary>
        /// Appliance and furniture price sum
        /// </summary>
        public Price ProductsPrice { get; set; }

        /// <summary>
        /// Service price sum
        /// </summary>
        public Price ServicesPrice { get; set; }

        /// <summary>
        /// Does the customer should pay VAT
        /// </summary>
        public bool IsVatRequired { get; set; }
        public OfferInformation()
        { }

        public OfferInformation(bool vatRequired)
        {
            IsVatRequired = vatRequired;
        }

        public OfferInformation(Price productsPrice, Price servicesPrice)
        {
            ProductsPrice = productsPrice;
            ServicesPrice = servicesPrice;
        }
    }
}
