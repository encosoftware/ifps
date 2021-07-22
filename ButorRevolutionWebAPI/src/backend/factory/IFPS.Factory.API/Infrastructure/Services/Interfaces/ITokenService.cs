using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.API.Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        TokenDto GenerateToken(AccountDto accountDto);

        string GetNameFromExpiredToken(string token);

        TokenDto Refresh(TokenDto oldTokenDto);
    }
}