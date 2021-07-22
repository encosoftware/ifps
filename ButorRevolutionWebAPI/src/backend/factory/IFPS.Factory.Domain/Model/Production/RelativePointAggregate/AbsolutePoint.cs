using ENCO.DDD.Domain.Model.Values;

namespace IFPS.Factory.Domain.Model
{
    public class AbsolutePoint : ValueObject<AbsolutePoint>
    {
        /// <summary>
        /// Coordinate of the X axis.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Coordinate of the Y axis.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Coordinate of the Z axis.
        /// </summary>
        public double Z { get; set; }

        private AbsolutePoint()
        {

        }
        public AbsolutePoint(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
