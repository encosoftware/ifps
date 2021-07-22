using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Domain.Seed.Employees
{
    public class EmployeeSeed : IEntitySeed<Employee>
    {
        public Employee[] Entities => new[]
        {
            new Employee(1) { Id = 1 },
            new Employee(2) { Id = 2 },
            new Employee(3) { Id = 3 },
            new Employee(5) { Id = 4 },
            new Employee(7) { Id = 5 },
        };
    }
}
