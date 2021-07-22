using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class ConcreteApplianceMaterialListDto
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public PriceListDto CurrentPrice { get; set; }

        public ConcreteApplianceMaterialListDto(ConcreteApplianceMaterial concreteApplianceMaterial)
        {
            Code = concreteApplianceMaterial.ApplianceMaterial.Code;
            Description = concreteApplianceMaterial.ApplianceMaterial.Description;
            CurrentPrice = new PriceListDto(concreteApplianceMaterial.ApplianceMaterial.SellPrice);
        }
    }
}
