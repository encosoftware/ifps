using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class ConcreteFurnitureUnitListDto
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public PriceListDto CurrentPrice { get; set; }

        public ConcreteFurnitureUnitListDto(ConcreteFurnitureUnit cfu)
        {
            Code = cfu.FurnitureUnit.Code;
            Description = cfu.FurnitureUnit.Description;
            CurrentPrice = new PriceListDto(cfu.FurnitureUnit.CurrentPrice.Price);
        }
    }
}
