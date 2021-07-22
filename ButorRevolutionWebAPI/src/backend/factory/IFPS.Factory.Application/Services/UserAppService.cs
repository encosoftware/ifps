using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Exceptions;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace IFPS.Factory.Application.Services
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IDivisionRepository divisionRepository;
        private readonly IEmailService emailService;
        private readonly IEmailDataRepository emailDataRepository;
        private readonly IFileHandlerService fileHandlerService;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly EmailSettings emailSettings;
        private readonly string htmlContainerFolder;

        public UserAppService(IApplicationServiceDependencyAggregate aggregate,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IEmployeeRepository employeeRepository,
            IDivisionRepository divisionRepository,
            IEmailService emailService,
            IEmailDataRepository emailDataRepository,
            IFileHandlerService fileHandlerService,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IOptions<EmailSettings> emailSettings,
            IConfiguration configuration,
            IHostingEnvironment hostingEnvironment)
            : base(aggregate)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.employeeRepository = employeeRepository;
            this.divisionRepository = divisionRepository;
            this.emailService = emailService;
            this.emailDataRepository = emailDataRepository;
            this.fileHandlerService = fileHandlerService;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.emailSettings = emailSettings.Value;
            htmlContainerFolder = Path.Combine(hostingEnvironment.ContentRootPath, "AppData");
        }

        public async Task<List<UserDto>> SearchUserAsync(string name, DivisionTypeEnum divisionType, int userNumber)
        {

            Expression<Func<User, bool>> userFilter = (User u) => u.IsTechnicalAccount == false;

            if (!string.IsNullOrWhiteSpace(name))
            {
                userFilter = userFilter.And(u => u.CurrentVersion.Name.ToLower().Contains(name.ToLower().Trim()));
            }

            Expression<Func<User, bool>> userPostFilter = (User u) => true;

            if (divisionType != DivisionTypeEnum.None)
            {
                userFilter = userFilter.And(ent => ent.Roles.Select(r => r.Role.Division.DivisionType).Contains(divisionType));
            }

            var users = await userRepository.GetUsersWithDataAsync(userFilter, userNumber);

            return users.Select(x => new UserDto(x))
                .ToList();
        }

        public async Task<PagedListDto<UserListDto>> GetUsersAsync(UserFilterDto filterDto)
        {
            Expression<Func<User, bool>> filter = CreateFilter(filterDto);

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<User>(
                UserFilterDto.GetColumnMappings(), nameof(User.Id));

            var users = await userRepository.GetUsersPagedWithDataRoleAsync(
                filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);

            return users.ToPagedList(UserListDto.FromEntity);
        }

        public async Task<int> CreateUserAsync(UserCreateDto userDto)
        {
            var user = await CreateAndGetUserAsync(userDto);
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.ConfirmNewUser);

            var builder = new BodyBuilder();
            await GenerateEmail(user, builder, EmailTypeEnum.ConfirmNewUser);
            await emailService.SendEmailAsync(user, emailSettings.RegisterSubject, builder.ToMessageBody(), emailData.Id);
            await unitOfWork.SaveChangesAsync();

            return user.Id;
        }

        private async Task<User> CreateAndGetUserAsync(UserCreateDto dto)
        {
            if (await userRepository.IsUserExistsAsync(dto.Email))
                new ValidationExceptionBuilder().AddError(nameof(dto.Email), "User already exists!").ThrowIfHasError();

            // use email as User.UserName to avoid problems with special characters (after replaces)
            var newUser = new User(dto.Email) { UserName = dto.Email.Replace('.', '_').Replace('@', '_').Replace('+', '_') };

            newUser.ImageId = Guid.Parse("D9BD4A0D-47B9-4188-90C7-BEAE54626523"); //default image
            var role = await roleRepository.GetRole(dto.RoleId);

            if (string.IsNullOrEmpty(dto.Password))
            {
                dto.Password = "Asdf1234.";
                newUser.EmailConfirmed = true;
            }

            var divisions = role.DefaultRoleClaims.Select(ent => ent.Claim.Division).ToList();

            var result = await userManager.CreateAsync(newUser, dto.Password);
            if (result != IdentityResult.Success)
                throw new IFPSValidationAppException(new Dictionary<string, List<string>>() { { nameof(IdentityResult), new List<string>() { $"Result: {result.ToString()}" } } });

            UserData data = new UserData(dto.Name, dto.PhoneNumber, Clock.Now.Date, Address.GetEmptyAddress()) { GaveEmailConsent = dto.GaveEmailConsent };
            newUser.AddVersion(data);

            newUser.IsActive = true;

            newUser.AddRoles(new List<int>() { role.Id });
            foreach (Claim claim in role.DefaultRoleClaims.Select(ent => ent.Claim).ToList())
            {
                newUser.AddClaims(new List<int>() { claim.Id });
            }

            if (divisions.Any(ent => Employee.EmployeesDivisions.Any(d => d == ent.DivisionType)))
            {
                await employeeRepository.InsertAsync(new Employee(newUser.Id));
            }

            await unitOfWork.SaveChangesAsync();

            return newUser;
        }

        public async Task DeactivateUser(int id)
        {
            var user = await userRepository.GetAsync(id);
            user.IsActive = false;

            await unitOfWork.SaveChangesAsync();
        }

        public async Task ActivateUser(int id)
        {
            var user = await userRepository.GetAsync(id);
            user.IsActive = true;

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await userRepository.GetAsync(id);
            user.IsDeleted = true;

            if (await employeeRepository.IsEmployeeExistsAsync(user.Id))
            {
                var employee = await employeeRepository.GetByUserIdAsync(user.Id);
                employee.Close();
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<UserDetailsDto> GetUserDetailsAsync(int id)
        {
            var user = await userRepository.GetUserWithDataRoleClaimsAsync(id);

            var result = UserDetailsDto.FromModel(user);

            result.IsEmployee = user.Claims.Any(ent => Employee.EmployeesDivisions.Contains(ent.Claim.Division.DivisionType));
            result.IsEmployee = result.IsEmployee ? user.Claims.Count() != 0 : false;
            return result;
        }

        public async Task UpdateUserAsync(int id, UserUpdateDto dto)
        {
            var user = await userRepository.GetUserWithDataRoleClaimsAsync(id);
            var company = user.Company;

            List<Claim> userClaims = null;

            user.Email = dto.Email;
            user.AddVersion(new UserData(dto.Name, dto.PhoneNumber, Clock.Now, dto.Address != null ? dto.Address.CreateModelObject() : Address.GetEmptyAddress()));
            user.Language = dto.Language;

            if (dto.CompanyId != company?.Id)
            {
                user.CompanyId = dto.CompanyId;
            }

            if (dto.ImageUpdateDto != null && !dto.ImageUpdateDto.FileName.Equals("photo-placeholder.jpg"))
            {
                if (user.ImageId.HasValue && user.ImageId != Guid.Parse("D9BD4A0D-47B9-4188-90C7-BEAE54626523"))
                    user.Image = await fileHandlerService.UpdateImage(user.ImageId.Value, dto.ImageUpdateDto.ContainerName, dto.ImageUpdateDto.FileName);
                else
                    user.ImageId = await fileHandlerService.InsertImage(dto.ImageUpdateDto.ContainerName, dto.ImageUpdateDto.FileName);
            }

            UpdateUserRoles(dto.OwnedRolesIds, user);

            Tuple<List<Claim>> updateClaimsResult = await UpdateUserClaims(dto.OwnedClaimsIds, user);
            userClaims = updateClaimsResult.Item1;

            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateUserProfileAsync(int id, UserProfileUpdateDto userProfileUpdateDto)
        {
            var user = await userRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentVersion);
            user = userProfileUpdateDto.UpdateModelObject(user);

            UserData data = new UserData(userProfileUpdateDto.Name, userProfileUpdateDto.PhoneNumber, Clock.Now,
                    userProfileUpdateDto.Address != null ? userProfileUpdateDto.Address.CreateModelObject() : Address.GetEmptyAddress());
            user.AddVersion(data);


            if (userProfileUpdateDto.Image != null && !userProfileUpdateDto.Image.FileName.Equals("photo-placeholder.jpg"))
            {
                if (user.ImageId.HasValue && user.ImageId != Guid.Parse("D9BD4A0D-47B9-4188-90C7-BEAE54626523"))
                    user.Image = await fileHandlerService.UpdateImage(user.ImageId.Value, userProfileUpdateDto.Image.ContainerName, userProfileUpdateDto.Image.FileName);
                else
                    user.ImageId = await fileHandlerService.InsertImage(userProfileUpdateDto.Image.ContainerName, userProfileUpdateDto.Image.FileName);
            }
            if (!string.IsNullOrWhiteSpace(userProfileUpdateDto.NewPassword))
            {
                await userManager.ChangePasswordAsync(user, userProfileUpdateDto.CurrentPassword, userProfileUpdateDto.NewPassword);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeUserPasswordAsync(int userId, UserPasswordUpdateDto userPasswordUpdateDto)
        {
            var user = await userRepository.SingleAsync(ent => ent.Id == userId);
            IdentityResult result = await userManager.ChangePasswordAsync(user, userPasswordUpdateDto.CurrentPassword, userPasswordUpdateDto.NewPassword);

            if (!result.Succeeded)
                throw new IFPSValidationAppException(new Dictionary<string, List<string>>() { { nameof(IdentityResult), new List<string>() { $"Result: {result.ToString()}" } } });

            await unitOfWork.SaveChangesAsync();
        }

        private async void UpdateUserRoles(List<int> roleIds, User user)
        {
            var existingRoleIds = user.Roles.Select(userRole => userRole.RoleId).ToList();
            var roles = await roleRepository.GetAllListIncludingAsync(ent => roleIds.Contains(ent.Id) && !existingRoleIds.Contains(ent.Id), ent => ent.Division);
            user.AddRoles(roles);
            user.RemoveRoles(user.Roles.Where(ur => !roleIds.Any(r => ur.RoleId == r)).Select(ur => ur.RoleId).ToList());
        }

        private async Task<Tuple<List<Claim>>> UpdateUserClaims(List<int> OwnedClaimsIds, User user)
        {
            Employee employee = null;
            List<Division> divisions = null;

            var addedClaimsIds = OwnedClaimsIds.Where(c => !user.Claims.Any(uc => uc.ClaimId == c)).ToList();
            var removedClaimsIds = user.Claims.Where(uc => !OwnedClaimsIds.Any(c => uc.ClaimId == c)).Select(uc => uc.ClaimId).ToList();

            if (addedClaimsIds.Any())
            {
                divisions = divisions ?? await divisionRepository.GetAllWithClaimsTranslationAsync();
                var newClaimsDivisionTypes = divisions
                    .SelectMany(d => d.Claims)
                    .Where(c => addedClaimsIds.Any(ac => ac == c.Id))
                    .Select(c => c.Division.DivisionType)
                    .ToHashSet();

                foreach (var divisionType in newClaimsDivisionTypes)
                {
                    if (Employee.EmployeesDivisions.Any(d => d == divisionType) && employee == null && !await employeeRepository.IsEmployeeExistsAsync(user.Id))
                    {
                        employee = new Employee(user.Id);
                        await employeeRepository.InsertAsync(employee);
                    }
                }

                user.AddClaims(addedClaimsIds);
            }

            if (removedClaimsIds.Any())
            {
                divisions = divisions ?? await divisionRepository.GetAllWithClaimsTranslationAsync();
                var removedClaimsDivisionTypes = divisions.SelectMany(d => d.Claims)
                    .Where(c => removedClaimsIds.Any(rc => rc == c.Id))
                    .Select(c => c.Division.DivisionType)
                    .ToHashSet();

                var userLeftClaims = divisions
                    .SelectMany(d => d.Claims)
                    .Where(c => user.Claims.Any(uc => uc.ClaimId == c.Id))
                    .Where(c => !removedClaimsIds.Any(rc => rc == c.Id))
                    .ToList();

                foreach (var divisionType in removedClaimsDivisionTypes)
                {
                    if (Employee.EmployeesDivisions.Any(d => d == divisionType) && !userLeftClaims.Any(c => Employee.EmployeesDivisions.Any(d => d == c.Division.DivisionType)))
                    {
                        employee = await employeeRepository.GetByUserIdAsync(user.Id);
                        employee.Close();
                    }
                }
                user.RemoveClaims(removedClaimsIds);
            }

            List<Claim> userClaims;
            if (addedClaimsIds.Any() || removedClaimsIds.Any())
            {
                userClaims = divisions
                    .SelectMany(d => d.Claims)
                    .Where(c => user.Claims.Any(uc => uc.ClaimId == c.Id && !uc.IsDeleted))
                    .ToList();
            }
            else
            {
                userClaims = user.Claims
                    .Where(uc => !uc.IsDeleted)
                    .Select(uc => uc.Claim)
                    .ToList();
            }

            return new Tuple<List<Claim>>(userClaims);
        }

        public async Task<LanguageTypeEnum> GetUserPreferredLanguageCode(int id)
        {
            var user = await userRepository.GetAsync(id);
            return user == null ? LanguageTypeEnum.HU : user.Language;
        }
        public async Task<AccountDto> LoginAsync(LoginDto loginDto)
        {
            if (!await userRepository.IsUserExistsAsync(loginDto.Email))
            {
                throw new IFPSAppException("User doesn't exists");
            }
            var user = await userRepository.GetUserWithClaimsAndTokenByNameAsync(loginDto.Email);

            SignInResult result = await signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, false);

            if (!result.Succeeded)
                throw new IFPSValidationAppException(new Dictionary<string, List<string>>() { { nameof(SignInResult), new List<string>() { $"Result: {result.ToString()}" } } });

            var claims = user.Claims.Select(p => new System.Security.Claims.Claim("IFPSClaim", p.Claim.Name.ToString())).ToList();
            return new AccountDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.CurrentVersion.Name,
                RoleName = user.Roles.First().Role.Name,
                Language = user.Language,
                Image = new ImageThumbnailDetailsDto(user.Image),
                CompanyId = user.CompanyId,
                Claims = claims
            };
        }

        public async Task ConfirmEmailAsync(int userId, string token)
        {
            var user = await userRepository.SingleAsync(ent => ent.Id == userId);
            var result = await userManager.ConfirmEmailAsync(user, token);
        }

        public async Task SendResetPasswordEmailAsync(string email)
        {
            var user = await userRepository.SingleIncludingAsync(ent => ent.Email == email, ent => ent.CurrentVersion);
            var builder = new BodyBuilder();
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.ConfirmNewUser);

            await GenerateEmail(user, builder, EmailTypeEnum.ResetPassword);
            await emailService.SendEmailAsync(user, subject: emailSettings.ResetSubject, builder.ToMessageBody(), emailData.Id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<AccountDto> GetUserAccountByNameAndTokenAsync(UserWithTokenDto userWithTokenDto)
        {
            var user = await userRepository.GetUserWithClaimsAndTokenByNameAsync(userWithTokenDto.Email); // UserName

            if (!user.Tokens.Any(p => !p.IsDeleted && p.Value == userWithTokenDto.RefreshToken))
                throw new IFPSValidationAppException(new Dictionary<string, List<string>>() { { "RefreshToken", new List<string>() { $"User RefreshToken not found!" } } });

            var claims = user.Claims.Select(p => new System.Security.Claims.Claim("IFPSClaim", p.Claim.Name.ToString()));

            return new AccountDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.CurrentVersion.Name,
                RoleName = user.Roles.First().Role.Name,
                Language = user.Language,
                Image = new ImageThumbnailDetailsDto(user.Image),
                CompanyId = user.CompanyId,
                Claims = claims
            };
        }

        public async Task UpdateRefreshTokenAsync(UserWithTokenDto userWithTokenDto)
        {
            var user = await userRepository.GetUserWithClaimsAndTokenByNameAsync(userWithTokenDto.Email); // UserName

            user.RemoveTokens();

            if (!string.IsNullOrEmpty(userWithTokenDto.RefreshToken))
            {
                var newToken = new UserToken
                {
                    UserId = user.Id,
                    User = user,
                    Name = "RefreshToken",
                    Value = userWithTokenDto.RefreshToken
                };

                user.AddToken(newToken);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task LogoutAsync(string email)
        {
            await signInManager.SignOutAsync();

            await UpdateRefreshTokenAsync(new UserWithTokenDto
            {
                Email = email,
                RefreshToken = ""
            });
        }

        public async Task<bool> ValidateEmail(string email)
        {
            return (await userRepository.GetAllListAsync(ent => ent.Email.Equals(email))).Count != 0;
        }

        public async Task ChangePasswordAsync(int userId, UserPasswordUpdateDto passwordUpdateDto)
        {
            var user = await userRepository.SingleAsync(ent => ent.Id == userId);
            IdentityResult result = await userManager.ChangePasswordAsync(user, passwordUpdateDto.CurrentPassword, passwordUpdateDto.NewPassword);

            if (!result.Succeeded)
                throw new IFPSValidationAppException(new Dictionary<string, List<string>>() { { nameof(IdentityResult), new List<string>() { $"Result: {result.Errors.ToArray()[0].Code.ToString()}\n {result.Errors.ToArray()[0].Description.ToString()}" } } });

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<BookedByDropdownListDto>> GetBookedByForDropdownAsync()
        {
            var users = await userRepository.GetBookedByForDropdownListAsync();
            var bookedByList = new List<BookedByDropdownListDto>();

            foreach (var user in users)
            {
                var bookedBy = new BookedByDropdownListDto(user.Id, user.CurrentVersion.Name);
                bookedByList.Add(bookedBy);
            }

            return bookedByList;
        }

        public async Task<UserProfileDetailsDto> GetUserProfileAsync(int id)
        {
            var user = await userRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentVersion, ent => ent.Image);
            return UserProfileDetailsDto.FromModel(user);
        }

        private async Task GenerateEmail(User user, BodyBuilder builder, EmailTypeEnum emailType)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            token = HttpUtility.UrlEncode(token);
            string url = configuration["Site:BaseUrl"] + $"/login?dialog=true&userId={user.Id}&passwordresettoken={token}";
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == emailType);
            string contents = System.IO.File.ReadAllText(Path.Combine(htmlContainerFolder, emailData.CurrentTranslation.FileName));

            var pathImage = Path.Combine(htmlContainerFolder, emailData.CurrentTranslation.ImageFileName);
            var image = builder.LinkedResources.Add(pathImage);

            image.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = string.Format(format: contents, image.ContentId, user.CurrentVersion.Name, url);
        }

        private static Expression<Func<User, bool>> CreateFilter(UserFilterDto filterDto)
        {
            Expression<Func<User, bool>> filter = (User u) => u.IsTechnicalAccount == false;

            if (filterDto == null)
                throw new IFPSAppException("FilterDto can not be null!");

            if (!string.IsNullOrWhiteSpace(filterDto.CompanyName))
            {
                filter = filter.And(ent => ent.Company.Name.ToLower().Contains(filterDto.CompanyName.ToLower().Trim()));
            }
            if (!string.IsNullOrWhiteSpace(filterDto.Name))
            {
                filter = filter.And(ent => ent.CurrentVersion.Name.ToLower().Contains(filterDto.Name.ToLower().Trim()));
            }
            if (!string.IsNullOrWhiteSpace(filterDto.PhoneNumber))
            {
                filter = filter.And(ent => ent.CurrentVersion.Phone.Contains(filterDto.PhoneNumber.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(filterDto.Email))
            {
                filter = filter.And(ent => ent.Email.ToLower().Contains(filterDto.Email.ToLower().Trim()));
            }
            if (filterDto.RoleId.HasValue)
            {
                filter = filter.And(ent => ent.Roles.Any(r => r.Role.Id == filterDto.RoleId.Value));
            }
            if (filterDto.IsActive.HasValue)
            {
                filter = filter.And(ent => ent.IsActive == filterDto.IsActive.Value);
            }
            if (filterDto.AddedOnFrom.HasValue)
            {
                filter = filter.And(ent => ent.CreationTime.Date >= filterDto.AddedOnFrom);
            }
            if (filterDto.AddedOnTo.HasValue)
            {
                filter = filter.And(ent => ent.CreationTime.Date <= filterDto.AddedOnTo);
            }

            return filter;
        }
    }
}