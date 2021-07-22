using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using System.Collections;
using System.Collections.Generic;

namespace IFPS.Sales.Domain.Model
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

        /// <summary>
        /// Type of the UserTeam
        /// </summary>
        public virtual UserTeamType UserTeamType { get; set; }
        public int UserTeamTypeId { get; set; }

        public UserTeam(int companyId, int technicalUserId, int userId, int userTeamTypeId)
        {
            CompanyId = companyId;
            TechnicalUserId = technicalUserId;
            UserId = userId;
            UserTeamTypeId = userTeamTypeId;
        }
    }
}
