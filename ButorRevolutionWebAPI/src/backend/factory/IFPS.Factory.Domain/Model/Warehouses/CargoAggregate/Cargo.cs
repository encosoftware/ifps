using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class Cargo : FullAuditedAggregateRoot
    {
        /// <summary>
        /// It's equivalent of CargoId in design
        /// </summary>
        public string CargoName { get; set; }

        public int SupplierId { get; set; }
        /// <summary>
        /// Supplier of the cargo
        /// </summary>
        public Company Supplier { get; set; }

        /// <summary>
        /// Status of the cargo
        /// </summary>
        public CargoStatusType Status { get; set; }
        public int StatusId { get; set; }

        /// <summary>
        /// Person, who ordered the cargo
        /// </summary>
        public User BookedBy { get; set; }
        public int BookedById { get; set; }

        public Price NetCost { get; set; }
        public Price Vat { get; set; }
        public Address ShippingAddress { get; set; }
        public Price ShippingCost { get; set; }

        public string Notes { get; set; }

        public DateTime BookedOn { get; set; }
        public DateTime? ArrivedOn { get; set; }
        public DateTime? StockedOn { get; set; }

        /// <summary>
        /// Private list of ordered packages
        /// </summary>
        private List<OrderedMaterialPackage> _orderedPackages;
        public IEnumerable<OrderedMaterialPackage> OrderedPackages => _orderedPackages.AsReadOnly();

        public void AddOrderedPackage(OrderedMaterialPackage orderedPackage)
        {
            Ensure.NotNull(orderedPackage);

            _orderedPackages.Add(orderedPackage);
        }

        public Cargo()
        {
            _orderedPackages = new List<OrderedMaterialPackage>();
        }

        public Cargo(string cargoName, int supplierId, int statusId, int bookedById, Price netCost, Price vat, Address shippingAddress, Price shippingCost, string notes, DateTime bookedOn, DateTime? arrivedOn, DateTime? stockedOn) : this()
        {
            CargoName = cargoName;
            SupplierId = supplierId;
            StatusId = statusId;
            BookedById = bookedById;
            NetCost = netCost;
            Vat = vat;
            ShippingAddress = shippingAddress;
            ShippingCost = shippingCost;
            Notes = notes;
            BookedOn = bookedOn;
            ArrivedOn = arrivedOn;
            StockedOn = stockedOn;
        }

        public Cargo(string cargoName, int supplierId, int bookedById, Price netCost, Price vat, Address shippingAddress, Price shippingCost, string notes, DateTime bookedOn, DateTime? arrivedOn, DateTime? stockedOn) : this()
        {
            CargoName = cargoName;
            SupplierId = supplierId;
            BookedById = bookedById;
            NetCost = netCost;
            Vat = vat;
            ShippingAddress = shippingAddress;
            ShippingCost = shippingCost;
            Notes = notes;
            BookedOn = bookedOn;
            ArrivedOn = arrivedOn;
            StockedOn = stockedOn;
        }

    }
}
