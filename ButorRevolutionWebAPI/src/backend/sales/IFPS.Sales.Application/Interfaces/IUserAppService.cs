using ENCO.DDD.Application.Dto;
using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IUserAppService
    {
        Task<List<UserDto>> SearchUserAsync(string name, DivisionTypeEnum divisionType, int userNumber);
        Task<int> CreateUserAsync(UserCreateDto userDto);
        Task<PagedListDto<UserListDto>> GetUsersAsync(UserFilterDto filter);
        Task<UserDetailsDto> GetUserDetailsAsync(int id);
        Task UpdateUserAsync(int id, UserUpdateDto userDto);
        Task UpdateUserProfileAsync(int id, UserProfileUpdateDto userProfileUpdateDto);
        Task ChangeUserPasswordAsync(int id, UserPasswordUpdateDto userPasswordUpdateDto);
        Task<UserProfileDetailsDto> GetUserProfileAsync(int id);
        Task DeactivateUser(int id);
        Task ActivateUser(int id);
        Task DeleteUser(int id);
        Task<LanguageTypeEnum> GetUserPreferredLanguageCode(int id);
        Task<AccountDto> LoginAsync(LoginDto loginDto);
        Task<int> RegisterAsync(UserCreateDto userCreateDto);
        Task ConfirmEmailAsync(int userId, string code);
        Task ChangePasswordAsync(int userId, string token, PasswordChangeDto passwordChangeDto);
        Task SendResetPasswordEmailAsync(string email);
        Task<AccountDto> GetUserAccountByNameAndTokenAsync(UserWithTokenDto userWithTokenDto);
        Task UpdateRefreshTokenAsync(UserWithTokenDto userWithTokenDto);
        Task LogoutAsync(string name);
        Task<bool> ValidateEmail(string email);
        Task<List<UserNameDto>> GetUserNamesByOwnCompanyAsync(int userId);
        Task<List<DivisionTypeEnum>> GetUserDivisions(int userId);
    }
}
