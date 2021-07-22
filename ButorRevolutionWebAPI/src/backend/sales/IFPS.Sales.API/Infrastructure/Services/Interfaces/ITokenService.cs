using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.API.Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        TokenDto GenerateToken(AccountDto accountDto);

        string GetNameFromExpiredToken(string token);

        TokenDto Refresh(TokenDto oldTokenDto);
    }
}