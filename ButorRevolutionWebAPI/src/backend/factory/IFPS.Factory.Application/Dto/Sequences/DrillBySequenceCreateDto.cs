using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class DrillBySequenceCreateDto
    {
        public int SuccessionNumber { get; set; }
        public List<HoleCreateDto> Holes { get; set; }
        public PlaneTypeEnum Plane { get; set; }
        public double Diameter { get; set; }

        public Drill CreateModelObject()
        {
            return new Drill(0) { Plane = Plane, Diameter = Diameter };
        }
    }
}
