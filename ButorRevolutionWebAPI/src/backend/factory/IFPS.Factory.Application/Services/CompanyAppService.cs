using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Repositories;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
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
            Expression<Func<Company, bool>> filter = CreateFilter(filterDto);

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<Company>(
                CompanyFilterDto.GetOrderingMapping(), nameof(Company.Id));

            var companies = await companyRepository.GetCompaniesWithIncludesAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return companies.ToPagedList(CompanyListDto.FromEntity);
        }


        public async Task<CompanyDetailsDto> GetCompanyDetailsAsync(int id)
        {
            var company = await companyRepository.GetCompanyByIdAsync(id);
            var employees = await userRepository.GetAllListAsync(ent => ent.CompanyId.HasValue && ent.CompanyId == id, EmployeeListDto.Projection);

            var teams = company.UserTeams.GroupBy(ent => ent.TechnicalUserId);
            return new CompanyDetailsDto(company)
            {
                Employees = employees,
                UserTeams = teams.Select(x => new UserTeamListDto()
                {
                    Id = x.Key,
                    Name = x.First().TechnicalUser?.CurrentVersion.Name,
                    Users = x.Select(ut => new UserDto(ut.User)).ToList(),
                }).ToList(),
            };
        }

        public async Task UpdateCompanyAsync(int id, CompanyUpdateDto companyUpdateDto)
        {
            var company = await companyRepository.GetCompanyByIdAsync(id);

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
                var teams = await userTeamRepository.GetTeamsByCompanyIdAsync(company.Id);

                var teamsToDelete = teams.Where(ent => !companyUpdateDto.UserTeams.Any(x => x.Id == ent.TechnicalUserId)).ToList();
                var teamsToAdd = companyUpdateDto.UserTeams.Where(x => !teams.Any(ent => ent.TechnicalUserId == x.Id)).ToList();

                foreach (var team in teamsToDelete)
                {
                    await userTeamRepository.DeleteAsync(team);
                    await userRepository.DeleteAsync(team.TechnicalUser);
                }

                foreach (var userTeamDto in teamsToAdd)
                {
                    var technicalUser = new User(userTeamDto.Name) { IsTechnicalAccount = true };
                    technicalUser.AddVersion(new UserData(userTeamDto.Name, "", Clock.Now, Address.GetEmptyAddress()));
                    userRepository.Insert(technicalUser);

                    foreach (int userId in userTeamDto.UserIds)
                    {
                        company.AddUserTeam(new UserTeam(id, technicalUser.Id, userId));
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

        public async Task<List<SupplierDropdownListDto>> GetSuppliersForDropdownAsync()
        {
            var suppliers = await companyRepository.GetSuppliersListAsync();
            var supplierList = new List<SupplierDropdownListDto>();

            foreach (var supplier in suppliers)
            {
                var sup = new SupplierDropdownListDto(supplier.Id, supplier.Name);
                supplierList.Add(sup);
            }

            return supplierList;
        }

        private static Expression<Func<Company, bool>> CreateFilter(CompanyFilterDto filterDto)
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
                    filter = filter.And(ent => (ent.CurrentVersion.HeadOffice.AddressValue
                    + ent.CurrentVersion.HeadOffice.City
                    + ent.CurrentVersion.HeadOffice.PostCode.ToString())
                    .ToLower().Contains(filterDto.Address.ToLower().Trim()));
                }

                if (!string.IsNullOrWhiteSpace(filterDto.ContactPersonEmail))
                {
                    filter = filter.And(ent => ent.CurrentVersion.ContactPerson
                    .Email.ToLower().Contains(filterDto.ContactPersonEmail.ToLower().Trim()));
                }
            }

            return filter;
        }

    }
}
