using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Factory.Domain.Enums;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class DocumentGroupVersion : FullAuditedEntity
    {
        public DocumentGroup Core { get; private set; }
        public int? CoreId { get; private set; }

        public DocumentState State { get; set; }
        public int StateId { get; set; }

        public DateTime ValidFrom { get; private set; }

        public DateTime? ValidTo { get; private set; }

        private List<Document> _documents = new List<Document>();
        public IEnumerable<Document> Documents => _documents.AsReadOnly();

        private DocumentGroupVersion()
        {

        }

        public DocumentGroupVersion(DocumentGroup core, DocumentState documentState)
        {
            Core = core;
            State = documentState;
            ValidFrom = Clock.Now;
        }

        public DocumentGroupVersion(DocumentGroup core, DocumentState documentState, DateTime validFrom, DateTime? validTo = null) : this(core, documentState)
        {
            ValidFrom = validFrom;
            ValidTo = validTo;
        }

        public void Close()
        {
            ValidTo = Clock.Now;
        }

        public bool CanModifyVersion()
        {
            Ensure.NotNull(State);
            Ensure.NotNull(Core);

            return Core.IsHistorized ?
                State.State == DocumentStateEnum.Absent || State.State == DocumentStateEnum.Uploaded || State.State == DocumentStateEnum.WaitingForApproval :
                true;
        }

        public void AddDocument(Document document)
        {
            Ensure.NotNull(this.Core);
            Ensure.NotNull(this.Core.Order);
            Ensure.NotNull(document);
            _documents.Add(document);
        }

        public static DocumentGroupVersion FromSeedData(int? coreId, int documentStateId, DateTime validFrom, DateTime? validTo, int id)
        {
            return new DocumentGroupVersion()
            {
                Id = id,
                CoreId = coreId,
                StateId = documentStateId,
                ValidFrom = validFrom,
                ValidTo = validTo,
            };
        }

        public static DocumentGroupVersion FromSeedData(DocumentGroup core, int documentStateId, DateTime validFrom, DateTime? validTo, int id)
        {
            return new DocumentGroupVersion()
            {
                Id = id,
                Core = core,
                CoreId = core.Id,
                StateId = documentStateId,
                ValidFrom = validFrom,
                ValidTo = validTo,
            };
        }
    }
}
