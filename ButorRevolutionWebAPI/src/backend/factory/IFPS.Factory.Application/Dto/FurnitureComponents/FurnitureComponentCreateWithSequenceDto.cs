using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class FurnitureComponentCreateWithSequenceDto
    {
        public int Id { get; set; } // ez most milyen id akkor? komponens

        public string Name { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Length { get; set; }

        public FurnitureComponent CreateModelObject()
        {
            return new FurnitureComponent(Name, Width, Length) { };
        }
    }
}
