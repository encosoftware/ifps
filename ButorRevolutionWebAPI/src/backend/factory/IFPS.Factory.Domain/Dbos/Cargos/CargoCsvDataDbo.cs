using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Dbos
{
    public class CargoCsvDataDbo
    {
        public string CargoName { get; set; }
        public string Status { get; set; }
        public DateTime? ArrivedOn { get; set; }
        public string SupplierName { get; set; }
        public string BookedByUser { get; set; }

        public CargoCsvDataDbo(Cargo cargo)
        {
            CargoName = cargo.CargoName;
            Status = cargo.Status.CurrentTranslation.Name;
            ArrivedOn = cargo.ArrivedOn;
            SupplierName = cargo.Supplier.Name;
            BookedByUser = cargo.BookedBy.CurrentVersion.Name;
        }
    }
}
