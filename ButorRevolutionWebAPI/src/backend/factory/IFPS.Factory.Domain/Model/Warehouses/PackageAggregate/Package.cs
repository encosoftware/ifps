using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Factory.Domain.Model
{
    public abstract class Package : FullAuditedAggregateRoot
    {
        /// <summary>
        /// Redundant information of the stored product - automatically generated from material code and package size
        /// </summary>
        public string PackageCode { get; set; }

        /// <summary>
        /// Redundant information of the stored product
        /// </summary>
        public string PackageDescription { get; set; }

        public int Size { get; set; }
    }
}
