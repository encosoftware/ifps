using IFPS.Factory.Domain.Model;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class PreviewCostDetailsDto
    {
        public PriceDetailsDto Shipping { get; set; }
        public PriceDetailsDto SubTotal { get; set; }
        public PriceDetailsDto Total { get; set; }
        public PriceDetailsDto Vat { get; set; }
        public bool IsVat { get; set; }

        public PreviewCostDetailsDto(Cargo cargo)
        {
            Shipping = new PriceDetailsDto(cargo.ShippingCost);
            SubTotal = new PriceDetailsDto(cargo.OrderedPackages.Select(ent => ent.UnitPrice).ToList());
            if (IsVat)
            {
                Vat = new PriceDetailsDto(cargo.Vat, SubTotal.Value);
                SubTotal.Value += Vat.Value;
            }
            else
            {
                Vat = new PriceDetailsDto(cargo.Vat) { Value = 0 };
            }
            Total = new PriceDetailsDto(SubTotal);
        }
    }
}
