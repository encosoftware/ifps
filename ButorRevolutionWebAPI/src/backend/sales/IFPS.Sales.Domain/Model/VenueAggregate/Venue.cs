using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class Venue : FullAuditedAggregateRoot
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

        /// <summary>
        /// Address of the office building
        /// </summary>        
        public Address OfficeAddress { get; set; }

        /// <summary>
        /// Private collection of meeting rooms
        /// </summary>
        private List<MeetingRoom> _meetingRooms;

        /// <summary>
        /// Private list of opening hours
        /// </summary>
        private List<VenueDateRange> _openingHours;

        public IEnumerable<VenueDateRange> OpeningHours => _openingHours.AsReadOnly();


        /// <summary>
        /// Public collection of this._versions private list
        /// </summary>

        public IEnumerable<MeetingRoom> MeetingRooms
        {
            get
            {
                return _meetingRooms;
            }
            set
            {
                if (value == null)
                {
                    throw new IFPSDomainException($"Error setting MeetingRooms, value is null.");
                }
                _meetingRooms = value.ToList();
            }
        }

        private Venue()
        {
            _meetingRooms = new List<MeetingRoom>();
            _openingHours = new List<VenueDateRange>();
            IsActive = true;
        }

        public Venue(string name) : this()
        {
            this.Name = name;
        }

        public Venue(string name,
            string phoneNumber, string email, int companyId, Address address) : this()
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            CompanyId = companyId;
            OfficeAddress = address;
        }

        public void AddMeetingRoom(MeetingRoom meetingRoom)
        {
            Ensure.NotNull(meetingRoom);

            _meetingRooms.Add(meetingRoom);
        }

        public void AddOpeningHour(VenueDateRange venueDateRange)
        {
            Ensure.NotNull(venueDateRange);

            _openingHours.Add(venueDateRange);
        }

        public void RemoveOpeningHours(List<int> dateRangeIds)
        {
            _openingHours.RemoveAll(entity => dateRangeIds.Contains(entity.Id));
        }
    }
}
