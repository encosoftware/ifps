using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IFPS.Sales.UnitTests.Domain.Model
{
    public class SalesPersonAggregateTests
    {
        [Fact]
        public void Check_sales_person_initializing()
        {
            var date = DateTime.Now;
            var salesPerson = new SalesPerson(1, date);

            Assert.Equal(date, salesPerson.ValidFrom);
            Assert.Null(salesPerson.ValidTo);
            Assert.False(salesPerson.IsDeleted);

            Assert.Null(salesPerson.Supervisor);
            Assert.NotNull(salesPerson.Supervisees);
            Assert.Empty(salesPerson.Supervisees);
            Assert.NotNull(salesPerson.Offices);
            Assert.Empty(salesPerson.Offices);
            Assert.NotNull(salesPerson.DefaultTimeTable);
            Assert.Empty(salesPerson.DefaultTimeTable);
        }

        [Fact]
        public void Close_sales_person()
        {
            var date = DateTime.Now;
            var salesPerson = new SalesPerson(1, date);

            Assert.Equal(date, salesPerson.ValidFrom);
            Assert.Null(salesPerson.ValidTo);
            Assert.False(salesPerson.IsDeleted);

            salesPerson.Close();

            Assert.NotNull(salesPerson.ValidTo);
        }

        [Fact]
        public void Add_and_remove_supervisor_success()
        {
            var date = DateTime.Now;
            var salesPerson = new SalesPerson(1, date);
            var supervisor = new SalesPerson(2, date) { Id = 3 };

            Assert.Null(salesPerson.Supervisor);
            salesPerson.Supervisor = supervisor;

            Assert.NotNull(salesPerson.Supervisor);
            Assert.Equal(3, salesPerson.Supervisor.Id);

            salesPerson.Supervisor = null;
            Assert.Null(salesPerson.Supervisor);
        }

        [Fact]
        public void Add_and_remove_supervisee_success()
        {
            var date = DateTime.Now;
            var salesPerson = new SalesPerson(1, date);
            var supervisee1 = new SalesPerson(2, date) { Id = 10 };
            var supervisee2 = new SalesPerson(3, date) { Id = 11 };

            Assert.NotNull(salesPerson.Supervisees);
            Assert.Empty(salesPerson.Supervisees);

            salesPerson.AddSupervisedSalesPersons(new List<SalesPerson>() { supervisee1, supervisee2 });
            Assert.NotEmpty(salesPerson.Supervisees);
            Assert.Contains(salesPerson.Supervisees, svs => svs.Id == 10);
            Assert.Contains(salesPerson.Supervisees, svs => svs.Id == 11);

            salesPerson.RemoveSupervisedSalesPersons(new List<SalesPerson>() { supervisee1 });
            Assert.NotEmpty(salesPerson.Supervisees);
            Assert.DoesNotContain(salesPerson.Supervisees, svs => svs.Id == 10);
            Assert.Contains(salesPerson.Supervisees, svs => svs.Id == 11);

            salesPerson.RemoveSupervisedSalesPersons(new List<int>() { supervisee2.Id });
            Assert.Empty(salesPerson.Supervisees);
            Assert.DoesNotContain(salesPerson.Supervisees, svs => svs.Id == 10);
            Assert.DoesNotContain(salesPerson.Supervisees, svs => svs.Id == 11);
        }

        [Fact]
        public void Add_DefaultTimeTable_success()
        {
            var date = DateTime.Now;
            var salesPerson = new SalesPerson(1, date);

            Assert.NotNull(salesPerson.DefaultTimeTable);
            Assert.Empty(salesPerson.DefaultTimeTable);

            salesPerson.AddDefaultTimeTableEntry(1, new DateTime(2019, 06, 11, 8, 0, 0), new DateTime(2019, 06, 11, 18, 0, 0));
            Assert.NotEmpty(salesPerson.DefaultTimeTable);
            Assert.Single(salesPerson.DefaultTimeTable);

            salesPerson.AddDefaultTimeTableEntry(2, new DateTime(2019, 06, 11, 9, 0, 0), new DateTime(2019, 06, 11, 19, 0, 0));
            Assert.Equal(2, salesPerson.DefaultTimeTable.Count());

            Assert.Equal(8, salesPerson.DefaultTimeTable.First().Interval.From.Hour);
            Assert.Equal(18, salesPerson.DefaultTimeTable.First().Interval.To.Hour);
        }

        [Fact]
        public void Add_new_office_success()
        {
            var salesPerson = new SalesPerson(1, DateTime.Now);

            salesPerson.AddOffices(new List<int>() { 1, 2, 3 });

            Assert.Equal(3, salesPerson.Offices.Count());
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 1);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 2);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 3);
            Assert.All(salesPerson.Offices, o => Assert.Equal(salesPerson.Id, o.SalesPersonId));
        }

        [Fact]
        public void Delete_office_success()
        {
            var salesPerson = new SalesPerson(1, DateTime.Now);

            salesPerson.AddOffices(new List<int>() { 1, 2, 3 });

            Assert.Equal(3, salesPerson.Offices.Count());
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 1);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 2);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 3);

            salesPerson.RemoveOffices(new List<int>() { 2 });
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 1 && o.IsDeleted == false);
            Assert.DoesNotContain(salesPerson.Offices, o => o.OfficeId == 2 && o.IsDeleted == false);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 3 && o.IsDeleted == false);

            salesPerson.RemoveOffices(new List<int>() { 1 });
            Assert.DoesNotContain(salesPerson.Offices, o => o.OfficeId == 1 && o.IsDeleted == false);
            Assert.DoesNotContain(salesPerson.Offices, o => o.OfficeId == 2 && o.IsDeleted == false);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 3 && o.IsDeleted == false);

            salesPerson.RemoveOffices(new List<int>() { 3 });
            Assert.DoesNotContain(salesPerson.Offices, o => o.OfficeId == 1 && o.IsDeleted == false);
            Assert.DoesNotContain(salesPerson.Offices, o => o.OfficeId == 2 && o.IsDeleted == false);
            Assert.DoesNotContain(salesPerson.Offices, o => o.OfficeId == 3 && o.IsDeleted == false);
        }

        [Fact]
        public void Add_offices_more_than_once_success()
        {
            var salesPerson = new SalesPerson(1, DateTime.Now);

            salesPerson.AddOffices(new List<int>() { 1, 2 });

            Assert.Equal(2, salesPerson.Offices.Count());
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 1);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 2);

            salesPerson.AddOffices(new List<int>() { 2, 3 });

            Assert.Equal(3, salesPerson.Offices.Count());
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 1);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 2);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 3);
        }

        [Fact]
        public void Add_duplicated_offices_success()
        {
            var salesPerson = new SalesPerson(1, DateTime.Now);

            salesPerson.AddOffices(new List<int>() { 1, 2, 2 });

            Assert.Equal(2, salesPerson.Offices.Count());
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 1);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 2);
        }

        [Fact]
        public void Add_delete_add_office_success()
        {
            var salesPerson = new SalesPerson(1, DateTime.Now);

            salesPerson.AddOffices(new List<int>() { 1, 2, 3 });

            Assert.Equal(3, salesPerson.Offices.Count());
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 1);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 2);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 3);

            salesPerson.RemoveOffices(new List<int>() { 2 });
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 1 && o.IsDeleted == false);
            Assert.DoesNotContain(salesPerson.Offices, o => o.OfficeId == 2 && o.IsDeleted == false);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 3 && o.IsDeleted == false);

            salesPerson.AddOffices(new List<int>() { 2 });
            Assert.Equal(4, salesPerson.Offices.Count());
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 1);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 2);
            Assert.Contains(salesPerson.Offices, o => o.OfficeId == 3);
        }
    }
}