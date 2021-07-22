using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class PriceDetailsWithCurrencyIdDto
    {
        public double? Value { get; set; }
        public int? CurrencyId { get; set; }
        public PriceDetailsWithCurrencyIdDto(Price price)
        {
            Value = price?.Value;
            CurrencyId = price?.CurrencyId;
        }
    }
}
