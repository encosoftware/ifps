using ENCO.DDD.Application.Dto;
using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using IFPS.Sales.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
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
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<UserDto>> SearchUser([FromQuery] string name, [FromQuery] DivisionTypeEnum divisionType, [FromQuery] int userNumber = 10)
        {
            return userAppService.SearchUserAsync(name, divisionType, userNumber);
        }

        //GET any user by id
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
        [HttpPut("profile")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateUserProfile([FromBody]UserProfileUpdateDto userProfileUpdateDto)
        {
            return userAppService.UpdateUserProfileAsync(GetCallerId(), userProfileUpdateDto);
        }

        // PUT: change user's password
        [HttpPut("password")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateUserPassword([FromBody] UserPasswordUpdateDto userPasswordUpdateDto)
        {
            return userAppService.ChangeUserPasswordAsync(GetCallerId(), userPasswordUpdateDto);
        }

        // GET: user profile
        [HttpGet("profile")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<UserProfileDetailsDto> GetUserProfile()
        {
            return userAppService.GetUserProfileAsync(GetCallerId());
        }

        //GET email is valid
        [HttpGet("email/{email}")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<bool> ValidateEmail(string email)
        {
            return userAppService.ValidateEmail(email);
        }

        [HttpGet("company")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<UserNameDto>> GetUserNamesByCompany()
        {
            return userAppService.GetUserNamesByOwnCompanyAsync(GetCallerId());
        }

        [HttpGet("{id}/divisions")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<DivisionTypeEnum>> GetUserDivisions(int id)
        {
            return userAppService.GetUserDivisions(id);
        }
    }
}