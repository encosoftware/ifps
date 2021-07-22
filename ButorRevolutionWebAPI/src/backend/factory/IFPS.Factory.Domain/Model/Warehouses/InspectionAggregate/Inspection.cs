using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class Inspection : FullAuditedAggregateRoot
    {
        /// <summary>
        /// Date of inspection
        /// </summary>
        public DateTime InspectedOn { get; set; }

        /// <summary>
        /// Storage of the inspection
        /// </summary>
        public virtual Storage InspectedStorage { get; set; }
        public int InspectedStorageId { get; set; }

        /// <summary>
        /// Report of the inspection
        /// </summary>
        public virtual Report Report { get; set; }
        public int ReportId { get; set; }

        /// <summary>
        /// Private list of user inspectons
        /// </summary>
        private List<UserInspection> _userInspections;
        public IEnumerable<UserInspection> UserInspections => _userInspections.AsReadOnly();

        private Inspection()
        {
            _userInspections = new List<UserInspection>();
        }

        public Inspection(DateTime inspectedOn, int inspectedStorageId) : this()
        {
            InspectedOn = inspectedOn;
            InspectedStorageId = inspectedStorageId;
        }

        /// <summary>
        /// Update list of user inspections
        /// </summary>
        /// <param name="userIds"></param>
        public void UpdateUserInspections(List<int> userIds)
        {
            var userInspectionsToDelete = UserInspections.Where(ent => !userIds.Any(x => x == ent.UserId)).ToList();
            var userInspectionsToAdd = userIds.Where(x => !UserInspections.Any(ent => ent.UserId == x)).ToList();
            _userInspections.RemoveAll(ent => userInspectionsToDelete.Contains(ent));
            _userInspections.AddRange(userInspectionsToAdd.Select(ent => new UserInspection(ent, Id)).ToList());
        }
    }
}
