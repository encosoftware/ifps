using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Factory.Domain.Model
{
    public class OrderedMaterialPackage : FullAuditedAggregateRoot
    {
        public MaterialPackage MaterialPackage { get; set; }
        public int MaterialPackageId { get; set; }

        public Cargo Cargo { get; set; }
        public int CargoId { get; set; }

        public Price UnitPrice { get; set; }

        public int OrderedAmount { get; set; }
        public int MissingAmount { get; set; }
        public int RefusedAmount { get; set; }
    }
}
