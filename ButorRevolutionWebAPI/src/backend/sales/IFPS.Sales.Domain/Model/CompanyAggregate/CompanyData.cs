using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class CompanyData : FullAuditedEntity, IEntityVersion<Company>
    {
        /// <summary>
        /// Tax number of the company
        /// </summary>
        public string TaxNumber { get; private set; }

        /// <summary>
        /// Register number of the company
        /// </summary>
        public string RegisterNumber { get; private set; }

        /// <summary>
        /// Registered head office of the company
        /// </summary>        
        public Address HeadOffice { get; set; }

        public virtual User ContactPerson { get; set; }
        public int? ContactPersonId { get; set; }

        public Company Core { get; set; }
        public int? CoreId { get; set; }

        public DateTime ValidFrom { get; private set; }
        public DateTime? ValidTo { get; private set; }

        private CompanyData() { }

        public CompanyData(
            string taxNumber,
            string registerNumber,
            Address headOffice,
            int? contactPersonId,
            DateTime validFrom)
        {
            TaxNumber = taxNumber;
            RegisterNumber = registerNumber;
            HeadOffice = headOffice;
            ContactPersonId = contactPersonId;
            ValidFrom = validFrom;
        }

        public void SetValidTo(DateTime? dateTime)
        {
            ValidTo = dateTime;
        }
    }
}
