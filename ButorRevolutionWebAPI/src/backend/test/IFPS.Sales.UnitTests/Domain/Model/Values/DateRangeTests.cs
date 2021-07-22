using FluentAssertions;
using IFPS.Sales.Domain.Model;
using System;
using Xunit;

namespace IFPS.Sales.UnitTests.Domain.Model.Values
{
    public class DateRangeTests
    {
        private readonly DateTime now = DateTime.Now;

        [Fact]
        public void Two_daterange_equals_if_contains_the_same_dates()
        {
            var range1 = new DateRange(now, now.AddDays(1));
            var range2 = new DateRange(now, now.AddDays(1));

            range1.Should().Be(range2);
            range1.GetHashCode().Should().Be(range2.GetHashCode());
        }

        [Fact]
        public void Two_daterange_is_not_equal_if_contains_different_dates()
        {
            var range1 = new DateRange(now, now.AddDays(1));
            var range2 = new DateRange(now, now.AddDays(2));

            range1.Should().NotBe(range2);
            range1.GetHashCode().Should().NotBe(range2.GetHashCode());
        }

        [Fact]
        public void To_date_should_be_greater_than_from_date()
        {
            Action action1 = () => new DateRange(now, now.AddDays(-1));
            Action action2 = () => new DateRange(now, now);
            Action action3 = () => new DateRange(now, now.AddDays(1));

            action1.Should().ThrowExactly<ArgumentException>();
            action2.Should().ThrowExactly<ArgumentException>();
            action3.Should().NotThrow();
        }
    }
}