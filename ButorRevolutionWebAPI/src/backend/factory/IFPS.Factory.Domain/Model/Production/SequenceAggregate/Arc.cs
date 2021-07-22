using System;

namespace IFPS.Factory.Domain.Model
{
    public class Arc : Command
    {
        /// <summary>
        /// Center distance on axis x from point.
        /// </summary>
        public double CenterDictanceAxisX { get; set; }

        /// <summary>
        /// Center distance on axis y from point.
        /// </summary>
        public double CenterDictanceAxisY { get; set; }

        /// <summary>
        /// The arc move clockwise direction around the center.
        /// </summary>
        public bool Clockwise { get; set; }

        private Arc()
        {

        }

        public Arc(int succession_number, AbsolutePoint pt) : base(succession_number, pt)
        {
            
        }

        public override Tuple<double, double> GetArcParam() { return new Tuple<double, double>(CenterDictanceAxisX, CenterDictanceAxisY); }
    }
}