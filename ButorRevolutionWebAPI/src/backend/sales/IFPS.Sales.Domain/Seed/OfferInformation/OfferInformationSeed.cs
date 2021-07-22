using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class OfferInformationSeed : IEntitySeed<OfferInformation>
    {
        public OfferInformation[] Entities => new[]
        {
            new OfferInformation(true) { Id = 1 },
            new OfferInformation(false) { Id = 2 }
        };

        //public OfferInformation[] Entities => new OfferInformation[] { };
    }
}
