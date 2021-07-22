using ENCO.DDD.Domain.Model.Values;
using IFPS.Factory.Domain.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class DrillProperties : ValueObject<DrillProperties>
    {
        /// <summary>
        /// The speed parameter for the code generation of drilling.
        /// </summary>
        public double DrillSpeed { get; set; }

        /// <summary>
        /// The type of drilling.
        /// </summary>
        public DrillTypeEnum DrillType { get; set; }
    }
}