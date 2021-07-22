using IFPS.Factory.API.Common;
using IFPS.Factory.API.Infrastructure.Services.Interfaces;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : IFPSControllerBase
    {
        private const string OPNAME = "Account";

        private readonly IUserAppService userAppService;
        private readonly ITokenService tokenService;

        public AccountController(IUserAppService userAppService,
            ITokenService tokenService)
        {
            this.userAppService = userAppService;
            this.tokenService = tokenService;
        }

        [HttpPost("login")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<TokenDto> Login([FromBody]LoginDto loginDto)
        {
            var accountDto = await userAppService.LoginAsync(loginDto);

            var tokenDto = tokenService.GenerateToken(accountDto);

            await userAppService.UpdateRefreshTokenAsync(new UserWithTokenDto
            {
                RefreshToken = tokenDto.RefreshToken,
                Email = loginDto.Email
            });

            return tokenDto;
        }

        [HttpPost("refresh")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<TokenDto> Refresh([FromBody]TokenDto tokenDto)
        {
            var email = tokenService.GetNameFromExpiredToken(tokenDto.AccessToken);
            var accountDto = await userAppService.GetUserAccountByNameAndTokenAsync(new UserWithTokenDto
            {
                Email = email,
                RefreshToken = tokenDto.RefreshToken
            });

            var newTokenDto = tokenService.GenerateToken(accountDto);

            await userAppService.UpdateRefreshTokenAsync(new UserWithTokenDto
            {
                RefreshToken = newTokenDto.RefreshToken,
                Email = email
            });

            return newTokenDto;
        }

        [Authorize]
        [HttpPost("logout")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task Logout([FromBody]LogoutTokenDto tokenDto)
        {
            var email = tokenService.GetNameFromExpiredToken(tokenDto.AccessToken);
            await userAppService.LogoutAsync(email);
        }

        [HttpPost("password/reset")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task ResetPassword(string email)
        {
            await userAppService.SendResetPasswordEmailAsync(email);
        }

        [HttpPost("password/change")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task ChangePassword(int userId, [FromBody] UserPasswordUpdateDto userPasswordUpdateDto)
        {
            await userAppService.ChangePasswordAsync(userId, userPasswordUpdateDto);
        }

        [HttpGet("confirm")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task ConfirmEmail(int userId, string token)
        {
            await userAppService.ConfirmEmailAsync(userId, token);
        }
    }
}