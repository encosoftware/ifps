using System.Collections.Generic;
using System.Threading.Tasks;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;

namespace IFPS.Factory.Application.Services
{
    public class MaterialAppService : ApplicationService, IMaterialAppService
    {
        private readonly IMaterialRepository materialRepository;

        public MaterialAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IMaterialRepository materialRepository)
            : base(aggregate)
        {
            this.materialRepository = materialRepository;
        }

        public async Task<List<MaterialListForDropdownDto>> GetMaterialsForDropdownByCategoryAsync(int categoryId)
        {
            return await materialRepository.GetAllListAsync(ent => ent.CategoryId == categoryId || ent.Category.ParentGroupId == categoryId, MaterialListForDropdownDto.Projection);
        }

        public async Task<List<MaterialListForDropdownDto>> GetMaterialsForDropdownAsync()
        {
            return await materialRepository.GetAllListAsync(ent => true, MaterialListForDropdownDto.Projection);
        }
    }
}
