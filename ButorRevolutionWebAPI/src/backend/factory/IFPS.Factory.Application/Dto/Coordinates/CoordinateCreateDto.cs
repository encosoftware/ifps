using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class CoordinateCreateDto
    {
        public double Value { get; set; }

        public bool EndSide { get; set; }

        public bool SameSide { get; set; }

        public Coordinate CreateModelObject()
        {
            return new Coordinate() { Value = Value, EndSide = EndSide, SameSide = SameSide };
        }
    }
}
