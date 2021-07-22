using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class FurnitureComponentWithSequenceDetailsDto
    {
        public string Name { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Length { get; set; }

        public FurnitureComponentWithSequenceDetailsDto(FurnitureComponent component)
        {
            Name = component.Name;
            Width = component.Width;
            Length = component.Length;
        }
    }
}
