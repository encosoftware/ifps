using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Sales.Domain.Model
{
    public class SalesPersonOffice : FullAuditedEntity
    {
        public virtual SalesPerson SalesPerson { get; set; }
        public int SalesPersonId { get; set; }

        public virtual Venue Office { get; set; }
        public int OfficeId { get; set; }

        public SalesPersonOffice(int officeId, int salesPersonId)
        {
            this.SalesPersonId = salesPersonId;
            this.OfficeId = officeId;
        }
    }
}
