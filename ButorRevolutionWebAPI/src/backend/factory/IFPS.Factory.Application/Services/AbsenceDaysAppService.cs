using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Exceptions;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class AbsenceDaysAppService : ApplicationService, IAbsenceDaysAppService
    {


        private readonly IEmployeeRepository employeeRepository;

        public AbsenceDaysAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IEmployeeRepository employeeRepository
            ) : base(aggregate)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task AddOrUpdateAbsenceDaysAsync(int userId, List<AbsenceDayDto> dtos)
        {
            var employee = await employeeRepository.GetByUserIdWithAbsenceDaysAsync(userId);
            foreach (var dtoDay in dtos)
            {
                employee.AddOrUpdateAbsenceDay(new EmployeeAbsenceDay
                {
                    AbsenceType = dtoDay.AbsenceType,
                    Date = dtoDay.Date,
                    EmployeeId = employee.Id,
                });
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<AbsenceDayDto>> GetAbsenceDaysAsync(int userId, int year, int month)
        {
            var validationErrors = ValidateDates(year, month);
            if (validationErrors.Any())
                throw new IFPSValidationAppException(validationErrors);

            var absenceDays = await employeeRepository.GetAbsenceDaysByUserIdAsync(
                userId,
                new DateRange(
                    new DateTime(year, month, 1),
                    new DateTime(year, month, DateTime.DaysInMonth(year, month)))
                );
            var result = absenceDays.Select(ad => new AbsenceDayDto
            {
                Date = ad.Date,
                AbsenceType = ad.AbsenceType
            }).ToList();

            return result;
        }

        private Dictionary<string, List<string>> ValidateDates(int year, int month)
        {
            Dictionary<string, List<string>> validationErrors = new Dictionary<string, List<string>>();
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
                validationErrors.Add(nameof(year), new List<string>() { $"Year parameter must be between {DateTime.MinValue.Year} and { DateTime.MaxValue.Year}!" });
            if (month < 1 || month > 12)
                validationErrors.Add(nameof(month), new List<string>() { $"Month parameter must be between {1} and { 12}!" });

            return validationErrors;
        }

        public async Task DeleteAbsenceDaysAsync(int userId, AbsenceDaysDeleteDto absenceDays)
        {
            var employee = await employeeRepository.GetByUserIdWithAbsenceDaysAsync(userId);
            foreach (var date in absenceDays.Dates)
            {
                employee.DeleteAbsenceDay(date);
            }

            await unitOfWork.SaveChangesAsync();
        }
    }
}
