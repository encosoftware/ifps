using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace IFPS.Sales.UnitTests.Domain.Model
{
    public class DocumentGroupAggregateTests
    {
        [Fact]
        public void Add_new_version_to_historized_success()
        {
            var group = new DocumentGroup(new Order("test", 1, 1, Clock.Now,Price.GetDefaultPrice()), 
                new DocumentFolder(DocumentFolderTypeEnum.RenderDocument, true));
            var version1 = new DocumentGroupVersion(group, new DocumentState(DocumentStateEnum.Empty));
            var version2 = new DocumentGroupVersion(group, new DocumentState(DocumentStateEnum.Empty));

            Assert.Empty(group.Versions);
            group.AddNewVersion(version1);
            Assert.NotEmpty(group.Versions);
            Assert.Contains(version1, group.Versions);


            group.AddNewVersion(version2);
            Assert.NotEmpty(group.Versions);
            Assert.Contains(version2, group.Versions);
            Assert.Equal(2, group.Versions.Count());
        }

        [Fact]
        public void Cant_add_second_version_to_not_historized()
        {
            var group = new DocumentGroup(new Order("test", 1, 1, Clock.Now,Price.GetDefaultPrice()), 
                new DocumentFolder(DocumentFolderTypeEnum.RenderDocument, false));
            var version1 = new DocumentGroupVersion(group, new DocumentState(DocumentStateEnum.Empty));
            var version2 = new DocumentGroupVersion(group, new DocumentState(DocumentStateEnum.Empty));

            Assert.Empty(group.Versions);
            group.AddNewVersion(version1);
            Assert.NotEmpty(group.Versions);
            Assert.Contains(version1, group.Versions);

            Assert.Throws<Exception>(() => group.AddNewVersion(version2));
            Assert.NotEmpty(group.Versions);
            Assert.DoesNotContain(version2, group.Versions);
            Assert.Single(group.Versions);
        }

        [Fact]
        public void Cant_add_version_with_incorrect_core()
        {
            var group = new DocumentGroup(new Order("test", 1, 1, Clock.Now,Price.GetDefaultPrice()), 
                new DocumentFolder(DocumentFolderTypeEnum.RenderDocument, false));
            var version1 = new DocumentGroupVersion(new DocumentGroup(new Order("test", 1, 1, Clock.Now,Price.GetDefaultPrice()), 
                new DocumentFolder(DocumentFolderTypeEnum.RenderDocument, false)), new DocumentState(DocumentStateEnum.Empty));

            Assert.Empty(group.Versions);

            Assert.Throws<NullReferenceException>(() => group.AddNewVersion(null));
            Assert.Throws<Exception>(() => group.AddNewVersion(version1));
            Assert.Empty(group.Versions);
        }
    }
}
