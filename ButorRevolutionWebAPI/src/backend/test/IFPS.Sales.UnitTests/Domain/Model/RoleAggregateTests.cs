using FluentAssertions;
using IFPS.Sales.Domain.Model;
using System;
using Xunit;

namespace IFPS.Sales.UnitTests.Domain.Model
{
    public class RoleAggregateTests
    {
        [Fact]
        public void Cant_add_null_default_claim_list()
        {
            var company = new Role("Test Customer", 1);

            Action action = () => company.AddDefaultRoleClaims(null);

            action.Should().ThrowExactly<NullReferenceException>();
        }
    }
}