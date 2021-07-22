using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Domain.Model
{
    public abstract class File : FullAuditedAggregateRoot<Guid>
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string ContainerName { get; set; }

        protected File()
        {
            Id = Guid.NewGuid();
        }

        public File(string fileName, string extension, string containerName) : this()
        {
            this.FileName = fileName;
            this.Extension = extension;
            this.ContainerName = containerName;
        }
    }
}
