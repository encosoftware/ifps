using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Factory.Domain.Model
{
    public class Machine : FullAuditedAggregateRoot
    {
        /// <summary>
        /// The version of machine's software
        /// </summary>
        public string SoftwareVersion { get; set; }

        /// <summary>
        /// The serial number of machine
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// The code of the machine
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The year of manufacture of the machine
        /// </summary>
        public int YearOfManufacture { get; set; }        

        public string Name { get; set; }
        public virtual Company Brand { get; set; }
        public int? BrandId { get; set; }

        private Machine() { }

        public Machine(string name, int? companyId)
        {
            Name = name;
            BrandId = companyId;
        }
    }
}
