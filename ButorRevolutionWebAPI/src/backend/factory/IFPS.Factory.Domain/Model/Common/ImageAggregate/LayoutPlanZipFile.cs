using System;

namespace IFPS.Factory.Domain.Model
{
    public class LayoutPlanZipFile : File
    {
        public Optimization Optimization { get; set; }
        public Guid OptimizationId { get; set; }

        private LayoutPlanZipFile() { }

        public LayoutPlanZipFile(string fileName, string containerName)
            : base(fileName, ".zip", containerName) { }
    }
}
