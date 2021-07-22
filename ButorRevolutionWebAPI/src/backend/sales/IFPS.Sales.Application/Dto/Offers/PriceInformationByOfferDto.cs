using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class PriceInformationByOfferDto
    {
        public PriceListDto Shipping { get; set; }
        public PriceListDto Assembly { get; set; }
        public PriceListDto Installation { get; set; }
        public PriceListDto Products { get; set; }
        public PriceListDto Vat { get; set; }
        public PriceListDto Total { get; set; }
        public bool IsVatRequired { get; set; }
        public AppSettingsPriceListDto AppSettingsPrice { get; set; }

        public PriceInformationByOfferDto() { }

        public void SetPricesValue(Order order, Price productPrice, Price servicePrice,
            double vat, double assemblyPrice, double installationFurnitureUnit, double installationBasicFee, double shippingBasicFee)
        {
            int currencyId = order.OfferInformation.ProductsPrice.CurrencyId;
            string currencyName = order.OfferInformation.ProductsPrice.Currency.Name;

            AppSettingsPrice = new AppSettingsPriceListDto(vat, assemblyPrice, installationFurnitureUnit);

            Shipping = new PriceListDto(shippingBasicFee, currencyId, currencyName);
            Installation = new PriceListDto(installationBasicFee, currencyId, currencyName);
            if (order.OrderedFurnitureUnits.Count() != 0)
            {
                CalculateAssembly(order, assemblyPrice, currencyId, currencyName);
                CalculateInstallation(order, installationFurnitureUnit, installationBasicFee, currencyId, currencyName);
            }

            if (order.Services.Count() != 0)
            {
                if (order.Services.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Shipping))
                {
                    CalculateShipping(order, shippingBasicFee);
                }
                if (order.OrderedFurnitureUnits != null && order.Services.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Assembly))
                {
                    CalculateAssembly(order, assemblyPrice, currencyId, currencyName);
                }
                if (order.OrderedFurnitureUnits != null && order.Services.Any(ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Installation))
                {
                    CalculateInstallation(order, installationFurnitureUnit, installationBasicFee, currencyId, currencyName);
                }
            }

            Products = new PriceListDto(productPrice.Value, productPrice.CurrencyId, productPrice.Currency.Name);
            Vat = new PriceListDto(Math.Round(vat * Products.Value.Value), Products.CurrencyId.Value, Products.Currency);
            Total = new PriceListDto(productPrice.Value + servicePrice.Value, productPrice.CurrencyId, productPrice.Currency.Name);
            IsVatRequired = order.OfferInformation.IsVatRequired;
            if (order.OfferInformation.IsVatRequired)
            {
                Total.Value = Total.Value + Vat.Value.Value;
            }
        }

        private void CalculateInstallation(Order order, double installationFurnitureUnit, double installationBasicFee, int currencyId, string currencyName)
        {
            Installation = new PriceListDto(
                          order.OrderedFurnitureUnits.Select(ent => ent.Quantity).Sum() * installationFurnitureUnit +
                          order.OrderedApplianceMaterials.Select(ent => ent.Quantity).Sum() * installationFurnitureUnit + 
                          installationBasicFee, currencyId, currencyName);
        }

        private void CalculateAssembly(Order order, double assemblyPrice, int currencyId, string currencyName)
        {
            Assembly = new PriceListDto(order.OrderedFurnitureUnits.Select(ent => ent.Quantity).Sum() * 
                assemblyPrice, currencyId, currencyName);
        }

        private void CalculateShipping(Order order, double shippingBasicFee)
        {
            Shipping = new PriceListDto(order.Services.SingleOrDefault(
                        ent => ent.Service.ServiceType.Type == ServiceTypeEnum.Shipping).Service.CurrentPrice.Price);

            Shipping.Value += shippingBasicFee;
        }
    }
}