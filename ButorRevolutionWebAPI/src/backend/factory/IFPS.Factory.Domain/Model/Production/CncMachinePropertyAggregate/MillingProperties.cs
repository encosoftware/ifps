using ENCO.DDD.Domain.Model.Values;

namespace IFPS.Factory.Domain.Model
{
    public class MillingProperties : ValueObject<MillingProperties>
    {
        /// <summary>
        /// The speed parameter for the code generation of milling.
        /// </summary>
        public double MillingSpeed { get; set; }

        /// <summary>
        /// The diameter of tool for milling.
        /// </summary>
        public double MillingDiameter { get; set; }

        /// <summary>
        /// The tool's spin direction.
        /// </summary>
        public bool SpinClockwise { get; set; }
    }
}