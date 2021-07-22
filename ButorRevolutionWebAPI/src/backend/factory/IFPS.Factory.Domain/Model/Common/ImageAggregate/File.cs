using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Factory.Domain.Model
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
            FileName = fileName;
            Extension = extension;
            ContainerName = containerName;
        }
    }
}
