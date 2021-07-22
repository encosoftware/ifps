using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FinancialDetailsByContractDto
    {
        public PriceInformationByOfferDto PaymentDetails { get; set; }
        public PriceListDto FirstPayment { get; set; }
        public DateTime? FirstPaymentDate { get; set; }
        public PriceListDto SecondPayment { get; set; }
        public DateTime? SecondPaymentDate { get; set; }

        public FinancialDetailsByContractDto()
        {
            PaymentDetails = new PriceInformationByOfferDto();
        }
    }
}
