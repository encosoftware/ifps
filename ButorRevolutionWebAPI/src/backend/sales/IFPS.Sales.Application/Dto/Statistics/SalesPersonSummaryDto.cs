using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class SalesPersonSummaryDto
    {
        public int NumberOfOffers { get; set; }
        public int NumberOfContracts { get; set; }
        public decimal Effeciency { get; set; }
        public PriceListDto SummaryTotal { get; set; }

        public void FromEntities(List<SalesPersonListDto> salesPersons)
        {
            NumberOfOffers = salesPersons.Sum(x => x.NumberOfOffers);
            NumberOfContracts = salesPersons.Sum(x => x.NumberOfContracts);
            SummaryTotal = new PriceListDto()
            {
                Currency = salesPersons.First().Total.Currency,
                CurrencyId = salesPersons.First().Total.CurrencyId,
                Value = salesPersons.Sum(x => x.Total.Value)
            };

            Effeciency = Math.Round(Convert.ToDecimal((100.0 / NumberOfContracts) * NumberOfOffers));
        }
    }
}