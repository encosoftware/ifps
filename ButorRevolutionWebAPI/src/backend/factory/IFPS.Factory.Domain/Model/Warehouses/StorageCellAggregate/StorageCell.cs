using ENCO.DDD.Domain.Model.Entities.Auditing;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class StorageCell : FullAuditedAggregateRoot
    {
        /// <summary>
        /// Name of the storage cell. Unique
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Metainformation associated with the cell
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// The storagecell is located in this warehouse
        /// </summary>
        public virtual Storage Warehouse { get; set; }
        public int WarehouseId { get; set; }

        /// <summary>
        /// Private list of the stocks, which are stored in the storage cell
        /// </summary>
        private List<Stock> _stocks;
        public virtual IEnumerable<Stock> Stocks => _stocks.AsReadOnly();

        private StorageCell()
        {
            _stocks = new List<Stock>();
        }

        public StorageCell(string name, int warehouseId) : this()
        {
            Name = name;
            WarehouseId = warehouseId;
        }
    }
}
