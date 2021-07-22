using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : IFPSControllerBase
    {
        private const string OPNAME = "Roles";

        private readonly IRoleAppService roleAppService;

        public RolesController(
            IRoleAppService roleAppService)
        {
            this.roleAppService = roleAppService;
        }

        // GET roles list
        [HttpGet]
        [Authorize(Policy = "GetRoles")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<RoleListDto>> GetRoles()
        {
            return await roleAppService.GetRolesAsync();
        }

        //GET role by id
        [HttpGet("{id}")]
        [Authorize(Policy = "GetRoles")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<RoleDetailsDto> GetRoleById(int id)
        {
            return await roleAppService.GetRoleDetailsAsync(id);
        }

        // POST: api/roles
        [HttpPost]
        [Authorize(Policy = "UpdateRoles")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<int> PostRole([FromBody] RoleCreateDto roleCreateDto)
        {
            return await roleAppService.CreateRoleAsync(roleCreateDto);
        }

        // PUT: api/roles/
        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateRoles")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task UpdateRole(int id, [FromBody]RoleUpdateDto roleUpdateDto)
        {
            await roleAppService.UpdateRoleAsync(id, roleUpdateDto);
        }

        // GET detailed role list
        [HttpGet("detailedlist")]
        [Authorize(Policy = "GetRoles")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<RoleClaimsDto>> GetDetailedRolesList()
        {
            return roleAppService.GetRolesWithClaimsAsync();
        }
    }
}