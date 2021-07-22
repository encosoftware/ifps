using ENCO.DDD.Application.Dto;
using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IUserAppService
    {
        Task<List<UserDto>> SearchUserAsync(string name, DivisionTypeEnum divisionType, int take);
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
        Task ConfirmEmailAsync(int userId, string code);
        Task ChangePasswordAsync(int userId, UserPasswordUpdateDto passwordUpdateDto);
        Task SendResetPasswordEmailAsync(string email);
        Task<AccountDto> GetUserAccountByNameAndTokenAsync(UserWithTokenDto userWithTokenDto);
        Task UpdateRefreshTokenAsync(UserWithTokenDto userWithTokenDto);
        Task LogoutAsync(string name);
        Task<bool> ValidateEmail(string email);
        Task<List<BookedByDropdownListDto>> GetBookedByForDropdownAsync();
    }
}
