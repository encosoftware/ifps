using ENCO.DDD.Domain.Model.Entities.Auditing;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class Storage : FullAuditedAggregateRoot
    {
        public string Name { get; set; }

        /// <summary>
        /// The company the storage belongs to
        /// </summary>
        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

        /// <summary>
        /// Address of the storage
        /// </summary>
        public virtual Address Address { get; set; }
        public int AddressId { get; set; }

        /// <summary>
        /// Private list of the storage cells
        /// </summary>
        private List<StorageCell> _storageCells;
        public virtual IEnumerable<StorageCell> StorageCells => _storageCells.AsReadOnly();

        private Storage()
        {
            _storageCells = new List<StorageCell>();
        }

        public Storage(string name, int companyId, Address address) : this()
        {
            Name = name;
            CompanyId = companyId;
            Address = address;
        }
    }
}
