using System;

namespace IFPS.Factory.Domain.Model
{
    public class ScheduleZipFile : File
    {
        public Optimization Optimization { get; set; }
        public Guid OptimizationId { get; set; }

        private ScheduleZipFile() { }

        public ScheduleZipFile(string fileName, string containerName)
            : base(fileName, ".html", containerName) { }
    }
}
