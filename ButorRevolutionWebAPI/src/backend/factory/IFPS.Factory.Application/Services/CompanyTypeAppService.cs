using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class CompanyTypeAppService : ApplicationService, ICompanyTypeAppService
    {
        private readonly ICompanyTypeRepository companyTypeRepository;


        public CompanyTypeAppService(
            IApplicationServiceDependencyAggregate aggregate,
            ICompanyTypeRepository companyTypeRepository)
            : base(aggregate)
        {
            this.companyTypeRepository = companyTypeRepository;
        }

        public async Task<List<CompanyTypeListDto>> GetCompanyTypesAsync()
        {
            var companyTypes = await companyTypeRepository.GetAllListAsync();
            return companyTypes.Select(ent => new CompanyTypeListDto(ent)).ToList();
        }
    }
}
