using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class Stock : FullAuditedAggregateRoot
    {
        /// <summary>
        /// What type of packages are stored storage cell
        /// </summary>
        public virtual MaterialPackage Package { get; set; }
        public int PackageId { get; set; }

        /// <summary>
        /// Quantity of the packages in the storage cell
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Storage cell, in where the stock is located
        /// </summary>
        public virtual StorageCell StorageCell { get; set; }
        public int StorageCellId { get; set; }

        /// <summary>
        /// When was the stock created
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// When was the stock ejected
        /// </summary>
        public DateTime? ValidTo { get; set; }

        public Stock(int packageId, int quantity, int storageCellId)
        {
            ValidFrom = Clock.Now;
            PackageId = packageId;
            Quantity = quantity;
            StorageCellId = storageCellId;
        }

        public Stock(int quantity)
        {
            Quantity = quantity;
            ValidFrom = Clock.Now;
        }
    }
}
