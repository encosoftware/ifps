using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Repositories;
using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class CompanyAppService : ApplicationService, ICompanyAppService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IUserRepository userRepository;
        private readonly IUserTeamRepository userTeamRepository;


        public CompanyAppService(
            IApplicationServiceDependencyAggregate aggregate,
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            IUserTeamRepository userTeamRepository)
            : base(aggregate)
        {
            this.companyRepository = companyRepository;
            this.userRepository = userRepository;
            this.userTeamRepository = userTeamRepository;
        }

        public async Task<int> CreateCompanyAsync(CompanyCreateDto companyCreateDto)
        {
            var newCompany = new Company(companyCreateDto.Name, companyCreateDto.CompanyTypeId);
            newCompany.AddVersion(new CompanyData(companyCreateDto.TaxNumber, companyCreateDto.RegisterNumber, companyCreateDto.Address.CreateModelObject(), null, Clock.Now));
            await companyRepository.InsertAsync(newCompany);
            await unitOfWork.SaveChangesAsync();

            return newCompany.Id;
        }

        public async Task<PagedListDto<CompanyListDto>> GetCompaniesAsync(CompanyFilterDto filterDto)
        {
            Expression<Func<Company, bool>> filter = (Company c) => c.Name != null;

            if (filterDto != null)
            {
                if (!string.IsNullOrWhiteSpace(filterDto.Name))
                {
                    filter = filter.And(ent => ent.Name.ToLower().Contains(filterDto.Name.ToLower().Trim()));
                }

                if (filterDto.CompanyType != 0)
                {
                    filter = filter.And(ent => ent.CompanyType.Type.Equals(filterDto.CompanyType));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Address))
                {
                    var filterArray = filterDto.Address.ToLower().Trim().Split(" ");

                    filter = filter.And(ent => filterArray.Any(e => (ent.CurrentVersion.HeadOffice.AddressValue
                    + ent.CurrentVersion.HeadOffice.City
                    + ent.CurrentVersion.HeadOffice.PostCode.ToString()).ToLower().Contains(e)));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.ContactPersonEmail))
                {
                    filter = filter.And(ent => ent.CurrentVersion.ContactPerson
                    .Email.ToLower().Contains(filterDto.ContactPersonEmail.ToLower().Trim()));
                }
            }

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<Company>(
                CompanyFilterDto.GetOrderingMapping(), nameof(Company.Id));

            var companies = await companyRepository.GetCompaniesWithIncludesAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);

            return companies.ToPagedList(CompanyListDto.FromEntity);
        }

        public async Task<CompanyDetailsDto> GetCompanyDetailsAsync(int id)
        {
            var company = await companyRepository.GetCompanyAsync(id);

            var employees = await userRepository.GetUsersWithRolesAndCurrentVersionAsync(ent => ent.CompanyId.HasValue && 
            ent.CompanyId == id && !ent.IsTechnicalAccount);

            var teams = company.UserTeams.GroupBy(ent => ent.TechnicalUserId);
            return new CompanyDetailsDto(company)
            {
                Employees = employees.Select(ent => new EmployeeListDto(ent)).ToList(),
                UserTeams = teams.Select(x => new UserTeamListDto()
                {
                    Id = x.Key,
                    Name = x.First().TechnicalUser?.CurrentVersion.Name,
                    UserTeamTypeName = x.First().UserTeamType.CurrentTranslation.Name,
                    Users = x.Select(ut => new UserDto(ut.User)).ToList(),
                }).ToList(),
            };
        }

        public async Task UpdateCompanyAsync(int id, CompanyUpdateDto companyUpdateDto)
        {
            var company = await companyRepository.GetCompanyAsync(id);

            if (companyUpdateDto.OpeningHours != null)
            {
                var openingHoursToDelete = company.OpeningHours.Where(ent =>
                        !companyUpdateDto.OpeningHours.Any(x => x.DayTypeId == ent.DayTypeId && x.From == ent.Interval.From && x.To == ent.Interval.To))
                    .ToList();

                var openingHoursToAdd = companyUpdateDto.OpeningHours.Where(x =>
                        !company.OpeningHours.Any(ent => x.DayTypeId == ent.DayTypeId && x.From == ent.Interval.From && x.To == ent.Interval.To))
                    .ToList();

                company.RemoveOpeningHours(openingHoursToDelete.Select(x => x.Id).ToList());

                company.AddOpeningHours(
                    openingHoursToAdd.Select(ent =>
                        new CompanyDateRange(company, ent.CreateModelObject(), ent.DayTypeId))
                    .ToList());
            }

            var newVersion = new CompanyData(companyUpdateDto.TaxNumber, companyUpdateDto.RegisterNumber,
                                                companyUpdateDto.Address.CreateModelObject(), null, Clock.Now);
            if (companyUpdateDto.ContactPersonId != null)
            {
                var contactPerson = await userRepository.FirstOrDefaultAsync(ent => ent.Id == companyUpdateDto.ContactPersonId)
                    ?? throw new EntityNotFoundException(typeof(User), companyUpdateDto.ContactPersonId);
                newVersion.ContactPersonId = contactPerson.Id;
            }
            company.AddVersion(newVersion);

            company.CompanyTypeId = companyUpdateDto.CompanyTypeId;

            if (companyUpdateDto.UserTeams != null)
            {
                var groups = await userRepository.GetAllListAsync(ent => ent.CompanyId == id && ent.IsTechnicalAccount);

                var groupsToDelete = groups.Where(ent => !companyUpdateDto.UserTeams.Any(x => x.Id == ent.Id)).ToList();
                var groupsToAdd = companyUpdateDto.UserTeams.Where(x => !groups.Any(ent => ent.Id == x.Id)).ToList();

                foreach (var group in groupsToDelete)
                {   
                    await userRepository.DeleteAsync(group.Id);                    
                }
                await userTeamRepository.DeleteAsync(ent => groupsToDelete.Select(t => t.Id).Contains(ent.TechnicalUserId));

                foreach (var userTeamDto in groupsToAdd)
                {
                    var technicalUser = new User(userTeamDto.Name) { IsTechnicalAccount = true, CompanyId = id };
                    technicalUser.AddVersion(new UserData(userTeamDto.Name, "", Clock.Now, Address.GetEmptyAddress()));
                    userRepository.Insert(technicalUser);

                    foreach (int userId in userTeamDto.UserIds)
                    {
                        company.AddUserTeam(new UserTeam(id, technicalUser.Id, userId, userTeamDto.UserTeamTypeId));
                    }
                }
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCompanyAsync(int id)
        {
            await companyRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<string> GetCompanyNameByIdAsync(int id)
        {
          return (await companyRepository.SingleAsync(ent => ent.Id == id)).Name;
        }
    }
}
