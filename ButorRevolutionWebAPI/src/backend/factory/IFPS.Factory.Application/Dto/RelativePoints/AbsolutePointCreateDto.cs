using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class AbsolutePointCreateDto
    {
        public CoordinateCreateDto X { get; set; }

        public CoordinateCreateDto Y { get; set; }

        public CoordinateCreateDto Z { get; set; }

        public AbsolutePointCreateDto()
        {

        }

        public AbsolutePoint CreateModelObject()
        {
            return new AbsolutePoint(X.Value, Y.Value, Z.Value);
        }

    }
}
