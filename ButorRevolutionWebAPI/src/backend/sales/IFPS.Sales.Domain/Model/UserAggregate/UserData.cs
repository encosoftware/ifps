using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class UserData : FullAuditedEntity, IEntityVersion<User>
    {
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Phone number of the user
        /// </summary>
        public string Phone { get; private set; }

        /// <summary>
        /// Contact address of the user. If it is the same as home address, then its empty
        /// </summary>
        public Address ContactAddress { get; set; }       

        /// <summary>
        /// Google account of the user
        /// </summary>
        public string GoogleEmail { get; set; }
        /// <summary>
        /// True, if the user wants to synchronize the framework's calendar with the google calendar
        /// </summary>
        public bool IsGoogleSynchronizationRequired { get; set; }
        public bool GaveEmailConsent { get; set; }

        public User Core { get; set; }
        public int? CoreId { get; set; }

        public DateTime ValidFrom { get; private set; }
        public DateTime? ValidTo { get; set; }

        public byte[] RowVersion { get; set; }

        private UserData()
        {
        }

        public UserData(string name, string phone, DateTime validFrom, Address address = null)
        {            
            this.Name = name;            
            this.Phone = phone;
            this.ContactAddress = address;
            this.ValidFrom = validFrom;
        }

        public void SetValidTo(DateTime? validTo)
        {
            this.ValidTo = validTo;
        }
    }
}
