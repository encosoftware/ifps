using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class EmployeeTestSeed : IEntitySeed<Employee>
    {
        public Employee[] Entities => new[]
        {
            new Employee(10000) { Id = 10000 },
            new Employee(10001) { Id = 10001 },
            new Employee(10007) { Id = 10002 },
            new Employee(10010) { Id = 10003 },
            new Employee(10012) { Id = 10004 },
            new Employee(10013) { Id = 10005 },
        };
    }
}