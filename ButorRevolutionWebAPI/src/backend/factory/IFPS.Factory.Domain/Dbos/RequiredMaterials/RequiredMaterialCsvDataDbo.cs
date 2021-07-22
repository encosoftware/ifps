using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Dbos
{
    public class RequiredMaterialCsvDataDbo
    {
        public string OrderName { get; set; }
        public string WorkingNumber { get; set; }
        public string MaterialCode { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Deadline { get; set; }

        public RequiredMaterialCsvDataDbo(RequiredMaterial requiredMaterial)
        {
            OrderName = requiredMaterial.Order.OrderName;
            WorkingNumber = requiredMaterial.Order.WorkingNumber;
            MaterialCode = requiredMaterial.Material.Code;
            Name = requiredMaterial.Material.Description;
            Amount = requiredMaterial.RequiredAmount;
            Deadline = requiredMaterial.Order.Deadline;
        }
    }
}
