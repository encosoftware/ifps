﻿using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<List<EmployeeAbsenceDay>> GetAbsenceDaysByUserIdAsync(int userId, DateRange range);

        Task<Employee> GetByUserIdAsync(int userId);

        Task<Employee> GetByUserIdWithAbsenceDaysAsync(int userId);

        Task<bool> IsEmployeeExistsAsync(int userId);
    }
}