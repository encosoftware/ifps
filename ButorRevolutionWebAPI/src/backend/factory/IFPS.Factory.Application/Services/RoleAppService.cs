using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly IRoleRepository roleRepository;

        public RoleAppService(IApplicationServiceDependencyAggregate aggregate,
            IRoleRepository roleRepository)
            : base(aggregate)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<int> CreateRoleAsync(RoleCreateDto roleCreateDto)
        {
            var newRole = roleCreateDto.CreateModelObject();

            foreach (int claimId in roleCreateDto.ClaimIdList)
            {
                newRole.AddDefaultRoleClaims(new DefaultRoleClaim(newRole.Id, claimId));
            }

            await roleRepository.InsertAsync(newRole);
            await unitOfWork.SaveChangesAsync();
            return newRole.Id;
        }

        public async Task<RoleDetailsDto> GetRoleDetailsAsync(int id)
        {
            var role = await roleRepository.GetRole(id);
            return new RoleDetailsDto(role);
        }

        public async Task<List<RoleListDto>> GetRolesAsync()
        {
            var roles = await roleRepository.GetAllListIncludingAsync(ent=> true, ent=> ent.Division.Translations, ent => ent.Translations);
            return roles.Select(ent => new RoleListDto(ent)).ToList();
        }

        public async Task UpdateRoleAsync(int id, RoleUpdateDto roleUpdateDto)
        {
            var role = await roleRepository.GetRole(id);
            foreach (DefaultRoleClaim defaultRoleClaim in role.DefaultRoleClaims)
            {
                if (!roleUpdateDto.ClaimIdList.Contains(defaultRoleClaim.ClaimId))
                {
                    defaultRoleClaim.IsDeleted = true;
                }
            }
            foreach (int claimId in roleUpdateDto.ClaimIdList)
            {
                if (!role.DefaultRoleClaims.Select(ent=>ent.ClaimId).Contains(claimId))
                {
                    role.AddDefaultRoleClaims(new DefaultRoleClaim(id, claimId));
                }
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<RoleClaimsDto>> GetRolesWithClaimsAsync()
        {
            var roles = await roleRepository.GetRoles();
            return roles.Select(ent => new RoleClaimsDto(ent)).ToList();
        }
    }
}
