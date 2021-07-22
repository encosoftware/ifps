using FluentAssertions;
using IFPS.Sales.Domain.Model;
using System;
using Xunit;

namespace IFPS.Sales.UnitTests.Domain.Model
{
    public class CompanyAggregateTests
    {
        [Fact]
        public void Cant_add_null_version()
        {
            var company = new Company("ENCO", 1);

            Action action = () => company.AddVersion(null);

            action.Should().ThrowExactly<NullReferenceException>();
        }

        [Fact]
        public void Add_new_version_success()
        {
            var company = new Company("ENCO", 1);
            var address = new Address(1117, "Budapest", "Irinyi", 1);
            var companyData = new CompanyData("1111", "1111", address, null, DateTime.Now);
            company.AddVersion(companyData);

            Assert.Equal(company.CurrentVersion, companyData);
            Assert.Null(companyData.ValidTo);
        }

        [Fact]
        public void Close_previous_version()
        {
            var company = new Company("ENCO", 1);
            var address = new Address(1117, "Budapest", "Irinyi", 1);
            var companyData = new CompanyData("1111", "1111", address, null, DateTime.Now);
            company.AddVersion(companyData);
            var newCompanyData = new CompanyData("2222", "2222", address, null, DateTime.Now);
            company.AddVersion(newCompanyData);

            Assert.Equal(company.CurrentVersion, newCompanyData);
            Assert.NotNull(companyData.ValidTo);
        }

        [Fact]
        public void Cant_add_null_user_team()
        {
            var company = new Company("ENCO", 1);

            Action action = () => company.AddUserTeam(null);

            action.Should().ThrowExactly<NullReferenceException>();
        }

        [Fact]
        public void Cant_add_null_opening_hours()
        {
            var company = new Company("ENCO", 1);

            Action action = () => company.AddOpeningHours(null);

            action.Should().ThrowExactly<NullReferenceException>();
        }
    }
}