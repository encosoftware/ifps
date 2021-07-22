using System;

namespace IFPS.Factory.Domain.Model
{
    public class Circle : Sequence
    {
        /// <summary>
        /// A point of the outline.
        /// </summary>
        public virtual RelativePoint StartPoint { get; set; }
        public int? StartPointId { get; set; }

        /// <summary>
        /// Center distance on axis x from point.
        /// </summary>
        public double CenterDistanceAxisX { get; set; }

        /// <summary>
        /// Center distance on axis y from point.
        /// </summary>
        public double CenterDistanceAxisY { get; set; }

        private Circle()
        {

        }

        public Circle(int succesionnumber)
        {

        }

        public override double GetDistance(double sizeX, double sizeY, double sizeZ)
        {
            return Math.PI * 2 * Math.Sqrt(Math.Pow(CenterDistanceAxisX, 2) + Math.Pow(CenterDistanceAxisY, 2));
        }
    }
}