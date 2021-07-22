using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Factory.Domain.Model
{
    public class UserInspection : FullAuditedEntity
    {
        public virtual User User { get; set; }
        public int UserId { get; set; }

        public virtual Inspection Inspection { get; set; }
        public int InspectionId { get; set; }

        public UserInspection(int userId, int inspectionId)
        {
            UserId = userId;
            InspectionId = inspectionId;
        }
    }
}
