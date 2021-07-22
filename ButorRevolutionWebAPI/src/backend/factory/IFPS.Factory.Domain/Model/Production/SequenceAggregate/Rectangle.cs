using System;

namespace IFPS.Factory.Domain.Model
{
    public class Rectangle : Sequence
    {
        /// <summary>
        /// The topleft corner of the rectangle. 
        /// </summary>
        public virtual AbsolutePoint TopLeft { get; set; }
        public int? TopLeftId { get; set; }

        /// <summary>
        /// The bottomleft corner of the rectangle. 
        /// </summary>
        public virtual AbsolutePoint BottomLeft { get; set; }
        public int? BottomLeftId { get; set; }

        /// <summary>
        /// The bootomright corner of the rectangle. 
        /// </summary>
        public virtual AbsolutePoint BottomRight { get; set; }
        public int? BottomRightId { get; set; }

        /// <summary>
        /// The topright corner of the rectangle. 
        /// </summary>
        public int? TopRightId { get; set; }
        public virtual AbsolutePoint TopRight { get; set; }

        private Rectangle()
        {

        }

        public Rectangle(int successionNumber) : base(successionNumber)
        {

        }

        public override double GetDistance(double sizeX, double sizeY, double sizeZ)
        {
            double distance = 0;

            distance += Math.Sqrt(Math.Pow(TopRight.X - TopLeft.X, 2) + Math.Pow(TopRight.Y - TopLeft.Y, 2));
            distance += Math.Sqrt(Math.Pow(TopLeft.X - BottomLeft.X, 2) + Math.Pow(TopLeft.Y - BottomLeft.Y, 2));
            distance += Math.Sqrt(Math.Pow(BottomLeft.X - BottomRight.X, 2) + Math.Pow(BottomLeft.Y - BottomRight.Y, 2));
            distance += Math.Sqrt(Math.Pow(BottomRight.X - TopRight.X, 2) + Math.Pow(BottomRight.Y - TopRight.Y, 2));

            return distance;
        }
    }
}
