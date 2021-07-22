using FluentAssertions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using Xunit;

namespace IFPS.Sales.UnitTests.Domain.Model
{
    public class DayTypeAggregateTests
    {
        [Fact]
        public void Cant_add_null_translation()
        {
            var company = new DayType(DayTypeEnum.Friday, 1);

            Action action = () => company.AddTranslation(null);

            action.Should().ThrowExactly<NullReferenceException>();
        }
    }
}