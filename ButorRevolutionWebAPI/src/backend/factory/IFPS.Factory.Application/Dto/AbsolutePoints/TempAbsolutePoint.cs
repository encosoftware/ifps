using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class TempAbsolutePoint
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public TempAbsolutePoint()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public AbsolutePoint CreateModelObject()
        {
            return new AbsolutePoint( X, Y, Z);
        }
    }
}
