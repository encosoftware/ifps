using ENCO.DDD.Domain.Model.Values;

namespace IFPS.Factory.Domain.Model
{
    public class EstimatorProperties : ValueObject<EstimatorProperties>
    {
        /// <summary>
        /// The milling's speed, not necessary equal with the machine milling speed.
        /// </summary>
        public double EstimatedMillingSpeed { get; set; }

        /// <summary>
        /// The drilling's speed, not necessary equal with the machine drilling speed.
        /// </summary>
        public double EstimatedDrillSpeed { get; set; }

        /// <summary>
        /// The rapid movement's speed.
        /// </summary>
        public double EstimatedRapidSpeed { get; set; }

        /// <summary>
        /// Tool changing time.
        /// </summary>
        public double ToolChangeTime { get; set; }

        /// <summary>
        /// Plane changing time.
        /// </summary>
        public double PlaneChangeTime { get; set; }
    }
}