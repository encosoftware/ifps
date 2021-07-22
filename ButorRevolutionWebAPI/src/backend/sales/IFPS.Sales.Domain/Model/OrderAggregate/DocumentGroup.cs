using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class DocumentGroup : FullAuditedEntity
    {

        public Order Order { get; private set; }
        public Guid OrderId { get; private set; }

        public DocumentFolder DocumentFolder { get; private set; }
        public int DocumentFolderId { get; private set; }

        public bool IsHistorized { get; private set; }

        private List<DocumentGroupVersion> _versions;
        public IEnumerable<DocumentGroupVersion> Versions { get { return _versions.AsReadOnly(); } private set { } }

        private DocumentGroup()
        {
            _versions = new List<DocumentGroupVersion>();
        }

        public DocumentGroup(Order order, DocumentFolder documentFolder) : this()
        {
            this.Order = order;
            this.DocumentFolder = documentFolder;
            this.IsHistorized = documentFolder.IsHistorized;
        }

        public void AddNewVersion(DocumentGroupVersion version)
        {
            Ensure.That(IsHistorized || !_versions.Any());
            Ensure.NotNull(version);
            Ensure.That(version.Core == this || version.CoreId == this.Id);

            _versions.Add(version);
        }

        public static DocumentGroup FromSeedData(Guid orderId, int documentFolderId, bool isHistorized, int id)
        {
            return new DocumentGroup()
            {
                Id = id,
                OrderId = orderId,
                DocumentFolderId = documentFolderId,
                IsHistorized = isHistorized,
            };
        }
        public static DocumentGroup FromSeedData(Order order, int documentFolderId, bool isHistorized, int id)
        {
            return new DocumentGroup()
            {
                Id = id,
                Order = order,
                OrderId = order.Id,
                DocumentFolderId = documentFolderId,
                IsHistorized = isHistorized,
            };
        }
    }
}
