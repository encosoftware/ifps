using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Application.Dto
{
    public class DeliveryPriceCreateDto
    {
        public double? Value { get; set; }
        public int? CurrencyId { get; set; }

        public DeliveryPriceCreateDto()
        {

        }

        public Price CreateModelObject()
        {
            return new Price(Value.Value, CurrencyId.Value) { };
        }
    }
}
