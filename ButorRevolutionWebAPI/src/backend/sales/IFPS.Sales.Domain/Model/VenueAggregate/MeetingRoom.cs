using ENCO.DDD.Domain.Model.Entities.Auditing;
using System.Collections.Generic;

namespace IFPS.Sales.Domain.Model
{
    public class MeetingRoom : FullAuditedEntity
    {
        /// <summary>
        /// The name of the meeting room
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path, how to get to the meeting room
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Office building, where the room is located
        /// </summary>
        public virtual Venue Venue { get; set; }
        public int VenueId { get; set; }

        private List<Appointment> _appointments;
        public IEnumerable<Appointment> Appointments => _appointments.AsReadOnly();

        private MeetingRoom()
        {
            _appointments = new List<Appointment>();
        }

        public MeetingRoom(string name, string description, int officeBuildingId) : this()
        {
            Name = name;
            Description = description;
            VenueId = officeBuildingId;
        }
    }
}
