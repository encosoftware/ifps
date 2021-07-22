using ENCO.DDD.Domain.Model.Entities;

namespace IFPS.Factory.Domain.Model
{
    public class RelativePoint : Entity
    {
        /// <summary>
        /// Relative coordinate of the X axis.
        /// </summary>
        public virtual Coordinate X { get; set; }

        /// <summary>
        /// Relative coordinate of the Y axis.
        /// </summary>
        public virtual Coordinate Y { get; set; }

        /// <summary>
        /// Relative coordinate of the Z axis.
        /// </summary>
        public virtual Coordinate Z { get; set; }

        public RelativePoint()
        {

        }

        public AbsolutePoint GetAbsolutePoint(double sizeX, double sizeY, double sizeZ)
        {
            var absolutePoint = new AbsolutePoint(
                X.GetAbsolute(sizeX),
                Y.GetAbsolute(sizeY),
                Z.GetAbsolute(sizeZ));

            return absolutePoint;
        }
    }
}
