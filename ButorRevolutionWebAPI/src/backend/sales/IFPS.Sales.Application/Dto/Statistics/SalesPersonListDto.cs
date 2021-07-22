using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class SalesPersonListDto
    {
        public string Name { get; set; }
        public int NumberOfOffers { get; set; }
        public int NumberOfContracts { get; set; }
        public decimal Efficiency { get; set; }
        public PriceListDto Total { get; set; }

        public void SetTotal(Price firstPayment, Price secondPayment)
        {
            if (Total == null)
            {
                Total = new PriceListDto(firstPayment);
                Total.Value += secondPayment.Value;
            }
            else
            {
                Total.Value += firstPayment.Value;
                Total.Value += secondPayment.Value;
            }
        }
    }
}
