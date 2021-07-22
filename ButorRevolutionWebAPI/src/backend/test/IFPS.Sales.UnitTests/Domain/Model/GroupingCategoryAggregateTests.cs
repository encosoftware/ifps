using FluentAssertions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq;
using Xunit;

namespace IFPS.Sales.UnitTests.Domain.Model
{
    public class GroupingCategoryAggregateTests
    {
        [Fact]
        public void Empty_grouping_category_has_zero_children_and_translation()
        {
            var groupingCategory = new GroupingCategory(GroupingCategoryEnum.MaterialType);

            groupingCategory.Children.Count().Should().Be(0);
            groupingCategory.Translations.Count().Should().Be(0);
        }

        [Fact]
        public void Cant_add_null_translation()
        {
            var groupingCategory = new GroupingCategory(GroupingCategoryEnum.MaterialType);

            Action action = () => groupingCategory.AddTranslation(null);

            action.Should().ThrowExactly<NullReferenceException>();
        }

        [Fact]
        public void New_category_has_the_same_type_as_parent()
        {
            var parent = new GroupingCategory(GroupingCategoryEnum.MaterialType);
            var child = new GroupingCategory(parent);

            child.CategoryType.Should().Be(parent.CategoryType);
        }
    }
}