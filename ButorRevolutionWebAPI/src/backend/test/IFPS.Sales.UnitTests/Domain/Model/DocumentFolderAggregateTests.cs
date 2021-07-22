using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace IFPS.Sales.UnitTests.Domain.Model
{
    public class DocumentFolderAggregateTests
    {
        [Fact]
        public void Can_add_only_allowed_types()
        {
            var folder = new DocumentFolder(DocumentFolderTypeEnum.RenderDocument, true) { Id=1};
            var allowedType1 = new DocumentType(DocumentTypeEnum.Render, 1);
            var allowedType2 = new DocumentType(DocumentTypeEnum.ProductionRequest, 1);
            var allowedType3 = new DocumentType(DocumentTypeEnum.OnSiteMeasurement, 1);
            var deniedType1 = new DocumentType(DocumentTypeEnum.Other, 2);
            var deniedType2 = new DocumentType(DocumentTypeEnum.None, 2);

            folder.AddDocumentType(allowedType1);
            folder.AddDocumentType(allowedType2);
            folder.AddDocumentType(allowedType3);

            Assert.True(folder.CanAddDocument(allowedType1));
            Assert.True(folder.CanAddDocument(allowedType2));
            Assert.True(folder.CanAddDocument(allowedType3));
            Assert.False(folder.CanAddDocument(deniedType1));
            Assert.False(folder.CanAddDocument(deniedType2));
        }

        [Fact]
        public void Add_types_to_folder_success()
        {
            var folder = new DocumentFolder(DocumentFolderTypeEnum.RenderDocument, true) { Id = 1 };
            var type1 = new DocumentType(DocumentTypeEnum.Render, 1);
            var type2 = new DocumentType(DocumentTypeEnum.Offer, 1);
            var type3 = new DocumentType(DocumentTypeEnum.PaymentRequest, 1);

            Assert.Empty(folder.DocumentTypes);
            Assert.Throws<NullReferenceException>(() => folder.AddDocumentType(null));

            folder.AddDocumentType(type1);
            Assert.NotEmpty(folder.DocumentTypes);
            Assert.Contains(type1, folder.DocumentTypes);

            folder.AddDocumentType(type2);
            Assert.NotEmpty(folder.DocumentTypes);
            Assert.Contains(type2, folder.DocumentTypes);
            Assert.Equal(2, folder.DocumentTypes.Count());

            folder.AddDocumentType(type3);
            Assert.NotEmpty(folder.DocumentTypes);
            Assert.Contains(type3, folder.DocumentTypes);
            Assert.Equal(3, folder.DocumentTypes.Count());
        }
    }
}
