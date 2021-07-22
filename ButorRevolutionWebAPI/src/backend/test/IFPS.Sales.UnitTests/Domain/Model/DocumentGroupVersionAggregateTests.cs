using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace IFPS.Sales.UnitTests.Domain.Model
{
    public class DocumentGroupVersionAggregateTests
    {
        [Fact]
        public void Modify_version_that_not_historized_success()
        {
            var group = new DocumentGroup(new Order("test", 1, 1, Clock.Now, Price.GetDefaultPrice()), 
                new DocumentFolder(DocumentFolderTypeEnum.RenderDocument, false));
            var state = new DocumentState(DocumentStateEnum.None);
            var version = new DocumentGroupVersion(group, state);


            version.State = new DocumentState(DocumentStateEnum.Empty);
            Assert.True(version.CanModifyVersion());

            version.State = new DocumentState(DocumentStateEnum.Uploaded);
            Assert.True(version.CanModifyVersion());
        }


        [Fact]
        public void Modify_version_that_historized_success()
        {
            var group = new DocumentGroup(new Order("test", 1, 1, Clock.Now,Price.GetDefaultPrice()), 
                new DocumentFolder(DocumentFolderTypeEnum.RenderDocument, true));
            var state = new DocumentState(DocumentStateEnum.None);
            var version = new DocumentGroupVersion(group, state);


            version.State = new DocumentState(DocumentStateEnum.Empty);
            Assert.True(version.CanModifyVersion());

            version.State = new DocumentState(DocumentStateEnum.Uploaded);
            Assert.True(version.CanModifyVersion());

            version.State = new DocumentState(DocumentStateEnum.WaitingForApproval);
            Assert.True(version.CanModifyVersion());

            version.State = new DocumentState(DocumentStateEnum.Approved);
            Assert.False(version.CanModifyVersion());

            version.State = new DocumentState(DocumentStateEnum.Declined);
            Assert.False(version.CanModifyVersion());
        }


        [Fact]
        public void Add_document_success()
        {
            var group = new DocumentGroup(new Order("test", 1, 1, Clock.Now,Price.GetDefaultPrice()), new DocumentFolder(DocumentFolderTypeEnum.RenderDocument, true));
            var state = new DocumentState(DocumentStateEnum.None);
            var version = new DocumentGroupVersion(group, state);

            var doc = new Document("", "", "","", FileExtensionTypeEnum.None, new DocumentType(DocumentTypeEnum.None,1), null);

            Assert.Empty(version.Documents);

            version.AddDocument(doc);

            Assert.NotEmpty(version.Documents);
            Assert.Contains(doc, version.Documents);
        }
    }
}
