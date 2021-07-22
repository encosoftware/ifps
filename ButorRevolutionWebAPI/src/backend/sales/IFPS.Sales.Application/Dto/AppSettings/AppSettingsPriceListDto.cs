namespace IFPS.Sales.Application.Dto
{
    public class AppSettingsPriceListDto
    {
        public double VAT { get; set; }
        public double Assembly { get; set; }
        public double Installation { get; set; }

        public AppSettingsPriceListDto(double vat, double assembly, double installation)
        {
            VAT = vat;
            Assembly = assembly;
            Installation = installation;
        }
    }
}
