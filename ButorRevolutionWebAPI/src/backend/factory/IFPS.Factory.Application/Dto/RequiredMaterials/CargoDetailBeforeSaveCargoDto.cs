using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class CargoDetailBeforeSaveCargoDto
    {
        public string CargoName { get; set; }
        public string BookedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ContractingPartyName { get; set; }
        public string SupplierName { get; set; }
        public PriceListDto NetCost { get; set; }
        public PriceListDto Vat { get; set; }
        public PriceListDto TotalGrossCost { get; set; }
        public double VatValue { get; set; }

        public CargoDetailBeforeSaveCargoDto(double vat)
        {
            VatValue = vat;
        }

        public void CreateCargoDetails(MaterialPackage package, User user)
        {
            CargoName = DateTime.Now.Date.ToString("yyyy. MM. dd.");
            BookedBy = user.CurrentVersion.Name;
            CreatedOn = DateTime.Now;
            SupplierName = package.Supplier.Name;
            SetDefaultPrices(package);
        }

        private void SetDefaultPrices(MaterialPackage package)
        {
            var defaultPrice = Price.GetDefaultPrice();
            defaultPrice.Currency = package.Price.Currency;
            NetCost = new PriceListDto(defaultPrice);
            Vat = new PriceListDto(defaultPrice);
            TotalGrossCost = new PriceListDto(defaultPrice);
        }

    }
}
