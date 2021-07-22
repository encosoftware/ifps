using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Factory.Domain.Model
{
    public class UserTeam : FullAuditedAggregateRoot
    {
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public int TechnicalUserId { get; set; }
        /// <summary>
        /// Technical user, which represents a group or team (shipping team always consists of 2 or 3 people)
        /// </summary>
        public virtual User TechnicalUser { get; set; }

        public int UserId { get; set; }
        /// <summary>
        /// Group or team member
        /// </summary>
        public virtual User User { get; set; }

        public UserTeam(int companyId, int technicalUserId, int userId)
        {
            CompanyId = companyId;
            TechnicalUserId = technicalUserId;
            UserId = userId;
        }
    }
}
