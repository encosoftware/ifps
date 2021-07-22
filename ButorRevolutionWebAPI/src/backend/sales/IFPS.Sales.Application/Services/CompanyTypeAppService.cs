using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
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
            return (await companyTypeRepository.GetAllListAsync()).Select(ent => new CompanyTypeListDto(ent)).ToList();
        }
    }
}
