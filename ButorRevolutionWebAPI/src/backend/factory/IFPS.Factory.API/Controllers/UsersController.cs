using ENCO.DDD.Application.Dto;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : IFPSControllerBase
    {
        private const string OPNAME = "Users";

        private readonly IUserAppService userAppService;

        public UsersController(
            IUserAppService userAppService)
        {
            this.userAppService = userAppService;
        }

        // GET user list
        [HttpGet]
        [Authorize(Policy = "GetUsers")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<UserListDto>> Get([FromQuery]UserFilterDto filter)
        {
            return userAppService.GetUsersAsync(filter);
        }

        // GET search User
        [HttpGet("search")]
        [Authorize(Policy = "GetUsers")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<UserDto>> SearchUser([FromQuery] string name, [FromQuery] DivisionTypeEnum divisionType, [FromQuery] int take = 10)
        {
            return userAppService.SearchUserAsync(name, divisionType, take);
        }

        //GET user by id
        [HttpGet("{id}")]
        [Authorize(Policy = "GetUsers")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<UserDetailsDto> GetById(int id)
        {
            return userAppService.GetUserDetailsAsync(id);
        }

        // POST: api/Users
        [HttpPost]
        [Authorize(Policy = "UpdateUsers")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> Post([FromBody] UserCreateDto userDto)
        {
            return userAppService.CreateUserAsync(userDto);
        }

        // PUT: api/users/{id}/deactivate
        [HttpPut("{id}/deactivate")]
        [Authorize(Policy = "UpdateUsers")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeactivateUser(int id)
        {
            return userAppService.DeactivateUser(id);
        }

        // PUT: api/users/{id}/deactivate
        [HttpPut("{id}/activate")]
        [Authorize(Policy = "UpdateUsers")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task ActivateUser(int id)
        {
            return userAppService.ActivateUser(id);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteUsers")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteUser(int id)
        {
            return userAppService.DeleteUser(id);
        }

        // PUT: api/users/
        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateUsers")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateUser(int id, [FromBody]UserUpdateDto updateUserDto)
        {
            return userAppService.UpdateUserAsync(id, updateUserDto);
        }

        // PUT: user profile
        [HttpPut("{id}/profile")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateUserProfile(int id, [FromBody]UserProfileUpdateDto userProfileUpdateDto)
        {
            return userAppService.UpdateUserProfileAsync(id, userProfileUpdateDto);
        }

        // PUT: user password
        [HttpPut("{id}/password")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateUserPassword(int id, [FromBody] UserPasswordUpdateDto userPasswordUpdateDto)
        {
            return userAppService.ChangeUserPasswordAsync(id, userPasswordUpdateDto);
        }

        // GET: user profile
        [HttpGet("{id}/profile")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<UserProfileDetailsDto> GetUserProfile(int id)
        {
            return userAppService.GetUserProfileAsync(id);
        }

        //GET email is valid
        [HttpGet("email/{email}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<bool> ValidateEmail(string email)
        {
            return userAppService.ValidateEmail(email);
        }

        // BookedBy list for dropdown
        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<BookedByDropdownListDto>> BookedByDropdownList()
        {
            return userAppService.GetBookedByForDropdownAsync();
        }
    }
}