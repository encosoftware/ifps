using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class FurnitureUnitWithComponentsDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public FurnitureUnitWithComponentsDto(FurnitureComponent component)
        {
            Id = component.Id;
            Name = component.Name;
        }
    }
}
