using ENCO.DDD.Domain.Model.Values;

namespace IFPS.Factory.Domain.Model
{
    public class Coordinate : ValueObject<Coordinate>
    {   
        /// <summary>
        /// Distance from the side.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// One of the reference value calculated from the end side.
        /// </summary>
        public bool EndSide { get; set; }

        /// <summary>
        /// The other reference value is the same as the previous reference value's side.
        /// </summary>
        public bool SameSide { get; set; }

        public Coordinate()
        {

        }

        public double GetAbsolute(double maxValue)
        {
            double retValue = Value;
            if (EndSide)
            {
                retValue += maxValue;
            }
            if (SameSide == EndSide)
            {
                retValue += maxValue;
            }
            return retValue/2;
        }
    }
}