using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Repositories;
using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Exceptions;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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

namespace IFPS.Sales.Application.Services
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly ISalesPersonRepository salesPersonRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IVenueRepository venueRepository;
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly IUserTeamRepository userTeamRepository;
        private readonly IFileHandlerService fileHandlerService;
        private readonly IEmailService emailService;
        private readonly IEmailDataRepository emailDataRepository;
        private readonly LocalFileStorageConfiguration localFiles;
        private readonly EmailSettings emailSettings;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly string htmlContainerFolder;
        private readonly IBasketRepository basketRepository;
        private readonly IGroupingCategoryRepository groupingCategoryRepository;
        private readonly IOrderRepository orderRepository;

        public UserAppService(IApplicationServiceDependencyAggregate aggregate,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ISalesPersonRepository salesPersonRepository,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            IVenueRepository venueRepository,
            IEventTypeRepository eventTypeRepository,
            IUserTeamRepository userTeamRepository,
            IFileHandlerService fileHandlerService,
            IEmailService emailService,
            IEmailDataRepository emailDataRepository,
            IHostingEnvironment hostingEnvironment,
            SignInManager<User> signInManager,
            IOptions<LocalFileStorageConfiguration> localFiles,
            IOptions<EmailSettings> emailSettings,
            UserManager<User> userManager,
            IBasketRepository basketRepository,
            IGroupingCategoryRepository groupingCategoryRepository,
            IOrderRepository orderRepository)
            : base(aggregate)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.salesPersonRepository = salesPersonRepository;
            this.customerRepository = customerRepository;
            this.employeeRepository = employeeRepository;
            this.venueRepository = venueRepository;
            this.eventTypeRepository = eventTypeRepository;
            this.userTeamRepository = userTeamRepository;
            this.fileHandlerService = fileHandlerService;
            this.emailService = emailService;
            this.emailDataRepository = emailDataRepository;
            this.signInManager = signInManager;
            this.localFiles = localFiles.Value;
            this.emailSettings = emailSettings.Value;
            this.userManager = userManager;
            this.basketRepository = basketRepository;
            this.groupingCategoryRepository = groupingCategoryRepository;
            this.orderRepository = orderRepository;
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
            Expression<Func<User, bool>> filter = (User u) => u.IsTechnicalAccount == false;

            if (filterDto == null)
            {
                throw new IFPSAppException("FilterDto can not be null!");
            }

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

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<User>(
                UserFilterDto.GetColumnMappings(), nameof(User.Id));

            var users = await userRepository.GetUsersPagedWithDataRoleAsync<UserListDto>(
                filter, UserListDto.Projection, orderingQuery, filterDto.PageIndex, filterDto.PageSize);

            return users.ToPagedList();
        }

        public async Task<int> CreateUserAsync(UserCreateDto userDto)
        {
            var user = await CreateAndGetUserAsync(userDto);
            await SendPasswordResetEmailAsync(user);
            await unitOfWork.SaveChangesAsync();
            return user.Id;
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
            await userRepository.DeleteAsync(user);
            if (await salesPersonRepository.IsSalesPersonExistsAsync(user.Id))
            {
                var salesPerson = await salesPersonRepository.GetByUserIdAsync(user.Id);
                if (await orderRepository.IsOrderWithSalesPersonExistingAsync(salesPerson.Id))
                {
                    throw new IFPSAppException("The salesperson cannot be deleted because there is still an ongoing order assigned to him/her");
                }
                salesPerson.Close();
                await salesPersonRepository.DeleteAsync(salesPerson.Id);
            }
            if (await customerRepository.IsCustomerExistsAsync(user.Id))
            {
                var customer = await customerRepository.GetByUserIdAsync(user.Id);
                if (await orderRepository.IsOrderWithCustomerExistingAsync(customer.Id))
                {
                    throw new IFPSAppException("The customer cannot be deleted because there is still an ongoing order assigned to him/her");
                }
                customer.Close();
                await customerRepository.DeleteAsync(customer.Id);
            }
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

            if (user.Roles.Any(ent => ent.Role.Division.DivisionType == DivisionTypeEnum.Sales))
            {
                var salesPerson = await salesPersonRepository.GetByUserIdWithOfficesSupervisorsSuperviseesAsync(user.Id);
                var teamNames = await userTeamRepository.GetTeamsNamesByUserIdAsync(user.Id);
                var superviseesUsers = await userRepository.GetUsersWithDataAsync(salesPerson.Supervisees.Select(svs => svs.User.Id).ToList());

                result.WorkingInfo = new WorkingInfoDto
                {
                    Offices = salesPerson.Offices.Select(x => VenueDto.FromModel(x.Office)).ToList(),
                    Supervisor = !salesPerson.SupervisorId.HasValue ? null : new UserDto(salesPerson.Supervisor.User),
                    Supervisees = superviseesUsers.Select(x => new UserDto(x)).ToList(),
                    WorkingHours = salesPerson.DefaultTimeTable.Select(x => WorkingHourDto.FromModel(x)).ToList(),
                    MaxDiscountPercent = salesPerson.MaxDiscount,
                    MinDiscountPercent = salesPerson.MinDiscount,
                    Teams = teamNames,
                };
            }

            if (user.Roles.Any(ent => ent.Role.Division.DivisionType == DivisionTypeEnum.Customer))
            {
                var customer = await customerRepository.GetByUserIdWithNofiNotificationModeAsync(user.Id);
                result.Notifications = NotificationsDto.FromModel(customer);
            }

            if (user.Roles.Any(ent => ent.Role.Division.DivisionType == DivisionTypeEnum.Sales || ent.Role.Division.DivisionType == DivisionTypeEnum.Partner))
            {
                result.IsCompanyNeeded = true;
            }

            result.IsEmployee = user.Roles.Any(ent => Employee.EmployeesDivisions.Contains(ent.Role.Division.DivisionType));
            result.IsEmployee = result.IsEmployee ? user.Claims.Count() != 0 : false;
            return result;
        }

        public async Task UpdateUserProfileAsync(int id, UserProfileUpdateDto userProfileUpdateDto)
        {
            var user = await userRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentVersion);
            user = userProfileUpdateDto.UpdateModelObject(user);
            UserData userData = new UserData(userProfileUpdateDto.Name, userProfileUpdateDto.PhoneNumber, Clock.Now,
                    userProfileUpdateDto.Address != null ? userProfileUpdateDto.Address.CreateModelObject() : Address.GetEmptyAddress());

            user.AddVersion(userData);


            if (userProfileUpdateDto.Image != null && !userProfileUpdateDto.Image.FileName.Equals("photo-placeholder.jpg"))
            {
                if (user.ImageId.HasValue && user.ImageId != Guid.Parse("D9BD4A0D-47B9-4188-90C7-BEAE54626523"))
                    user.Image = await fileHandlerService.UpdateImage(user.ImageId.Value, userProfileUpdateDto.Image.ContainerName, userProfileUpdateDto.Image.FileName);
                else
                    user.ImageId = await fileHandlerService.InsertImage(userProfileUpdateDto.Image.ContainerName, userProfileUpdateDto.Image.FileName);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeUserPasswordAsync(int userId, UserPasswordUpdateDto updateDto)
        {
            var user = await userRepository.SingleAsync(ent => ent.Id == userId);
            IdentityResult result = await userManager.ChangePasswordAsync(user, updateDto.CurrentPassword, updateDto.NewPassword);

            if (!result.Succeeded)
                throw new IFPSValidationAppException(new Dictionary<string, List<string>>() { { nameof(IdentityResult), new List<string>() { $"Result: {result.ToString()}" } } });

            await unitOfWork.SaveChangesAsync();
        }

        private async Task<User> CreateAndGetUserAsync(UserCreateDto dto)
        {
            if (await userRepository.IsUserExistsAsync(dto.Email))
                new ValidationExceptionBuilder().AddError(nameof(dto.Email), "User name already exists!").ThrowIfHasError();
            
            // use email as User.UserName to avoid problems with special characters (after replaces)
            var newUser = new User(dto.Email) { UserName = dto.Email.Replace('.', '_').Replace('@', '_').Replace('+', '_') };

            newUser.ImageId = Guid.Parse("D9BD4A0D-47B9-4188-90C7-BEAE54626523"); //default image
            var role = await roleRepository.GetRole(dto.RoleId.GetValueOrDefault());

            if (string.IsNullOrEmpty(dto.Password))
            {
                dto.Password = "Asdf1234.";
                newUser.EmailConfirmed = true;
            }

            var result = await userManager.CreateAsync(newUser, dto.Password);
            if (!result.Succeeded)
                throw new IFPSValidationAppException(new Dictionary<string, List<string>>() { { nameof(IdentityResult), new List<string>() { $"Result: {result.ToString()}" } } });

            UserData data = new UserData(dto.Name, dto.PhoneNumber, Clock.Now.Date, Address.GetEmptyAddress()) { GaveEmailConsent = dto.GaveEmailConsent };
            newUser.AddVersion(data);

            newUser.CreationTime = DateTime.Now;
            newUser.IsActive = true;
            await SetRolesForNewUserAsync(newUser, role);

            return newUser;
        }

        private async Task SendPasswordResetEmailAsync(User user)
        {
            var builder = new BodyBuilder();
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.ConfirmNewUser);

            await GenerateEmail(user, builder, EmailTypeEnum.ConfirmNewUser);
            await emailService.SendEmailAsync(user, emailSettings.ResetSubject, builder.ToMessageBody(), emailData.Id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(int id, UserUpdateDto dto)
        {
            var user = await userRepository.GetUserWithDataRoleClaimsAsync(id);
            var company = user.Company;

            SalesPerson salesPerson = null;
            Customer customer = null;

            user.Email = dto.Email;
            UserData data = new UserData(dto.Name, dto.PhoneNumber, Clock.Now, dto.Address != null ? dto.Address.CreateModelObject() : Address.GetEmptyAddress());
            user.AddVersion(data);

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

            Tuple<SalesPerson, Customer> updateRolesResult = await UpdateUserRoles(dto.OwnedRolesIds, user);
            salesPerson = updateRolesResult.Item1;
            customer = updateRolesResult.Item2;

            UpdateUserClaims(dto.OwnedClaimsIds, user);


            if (user.Roles.Any(role => role.Role.Division.DivisionType == DivisionTypeEnum.Sales && !role.IsDeleted) && dto.WorkingInfo != null)
            {
                salesPerson = salesPerson ?? await salesPersonRepository.GetByUserIdWithOfficesSupervisorsSuperviseesAsync(user.Id);
                await UpdateWorkingInfo(dto.WorkingInfo, salesPerson);
            }

            if (user.Roles.Any(role => role.Role.Division.DivisionType == DivisionTypeEnum.Customer && !role.IsDeleted) && dto.Notifications != null)
            {
                customer = customer ?? await customerRepository.GetByUserIdWithNofiNotificationModeAsync(user.Id);
                await UpdateNotifications(dto.Notifications, customer);
            }

            await unitOfWork.SaveChangesAsync();
        }

        private async Task<Tuple<SalesPerson, Customer>> UpdateUserRoles(List<int> roleIds, User user)
        {
            SalesPerson salesPerson = null;
            Customer customer = null;
            Employee employee = null;

            var existingRoleIds = user.Roles.Select(userRole => userRole.RoleId).ToList();
            var addedRoles = await roleRepository.GetAllListIncludingAsync(ent => roleIds.Contains(ent.Id) && !existingRoleIds.Contains(ent.Id), ent => ent.Division);
            var removedRoles = await roleRepository.GetAllListIncludingAsync(ent => !roleIds.Contains(ent.Id) && existingRoleIds.Contains(ent.Id), ent => ent.Division);

            if (addedRoles.Any())
            {
                var newRolesDivisionTypes = addedRoles.Select(r => r.Division.DivisionType).ToHashSet();

                foreach (var divisionType in newRolesDivisionTypes)
                {
                    if (divisionType == DivisionTypeEnum.Sales && !await salesPersonRepository.IsSalesPersonExistsAsync(user.Id))
                    {
                        salesPerson = new SalesPerson(user.Id, DateTime.Now);
                        await salesPersonRepository.InsertAsync(salesPerson);
                    }
                    if (divisionType == DivisionTypeEnum.Customer && !await customerRepository.IsCustomerExistsAsync(user.Id))
                    {
                        customer = new Customer(user.Id, DateTime.Now);
                        await customerRepository.InsertAsync(customer);
                    }
                    if (Employee.EmployeesDivisions.Any(d => d == divisionType) && employee == null && !await employeeRepository.IsEmployeeExistsAsync(user.Id))
                    {
                        employee = new Employee(user.Id);
                        await employeeRepository.InsertAsync(employee);
                    }
                }

                user.AddRoles(addedRoles);
            }

            if (removedRoles.Any())
            {
                var removedRolesDivisionTypes = removedRoles
                    .Select(c => c.Division.DivisionType)
                    .ToHashSet();

                var leftDivisions = user.Roles
                    .Where(ur => !removedRoles.Any(rr => rr.Id == ur.RoleId))
                    .Select(ur => ur.Role.Division.DivisionType)
                    .ToHashSet();

                foreach (var divisionType in removedRolesDivisionTypes)
                {
                    if (divisionType == DivisionTypeEnum.Sales && !leftDivisions.Any(ld => ld == divisionType))
                    {
                        salesPerson = await salesPersonRepository.GetByUserIdAsync(user.Id);
                        salesPerson.Close();
                    }
                    if (divisionType == DivisionTypeEnum.Customer && !leftDivisions.Any(ld => ld == divisionType))
                    {
                        customer = await customerRepository.GetByUserIdAsync(user.Id);
                        customer.Close();
                    }
                    if (Employee.EmployeesDivisions.Any(d => d == divisionType) && !leftDivisions.Any(ld => Employee.EmployeesDivisions.Any(d => d == ld)))
                    {
                        employee = await employeeRepository.GetByUserIdAsync(user.Id);
                        employee.Close();
                    }
                }
                user.RemoveRoles(removedRoles.Select(r => r.Id).ToList());
            }
            return new Tuple<SalesPerson, Customer>(salesPerson, customer);
        }

        private void UpdateUserClaims(List<int> OwnedClaimsIds, User user)
        {
            var addedClaimsIds = OwnedClaimsIds.Where(c => !user.Claims.Any(uc => uc.ClaimId == c)).ToList();
            var removedClaimsIds = user.Claims.Where(uc => !OwnedClaimsIds.Any(c => uc.ClaimId == c)).Select(uc => uc.ClaimId).ToList();

            if (addedClaimsIds.Any())
            {
                user.AddClaims(addedClaimsIds);
            }

            if (removedClaimsIds.Any())
            {
                user.RemoveClaims(removedClaimsIds);
            }
        }

        private async Task UpdateWorkingInfo(UpdateWorkingInfoDto dto, SalesPerson salesPerson)
        {
            salesPerson.MinDiscount = dto.MinDiscountPercent;
            salesPerson.MaxDiscount = dto.MaxDiscountPercent;

            if (dto.OfficeIds != null)
            {
                var addedOfficesIds = dto.OfficeIds.Except(salesPerson.Offices.Select(sp => sp.OfficeId)).ToList();
                var removedOfficesIds = salesPerson.Offices.Select(sp => sp.OfficeId).Except(dto.OfficeIds).ToList();
                if (removedOfficesIds.Any())
                {
                    salesPerson.RemoveOffices(removedOfficesIds);
                }
                if (addedOfficesIds.Any())
                {
                    var offices = await venueRepository.GetAllListAsync();

                    var invalidOfficeIds = addedOfficesIds.Except(offices.Select(o => o.Id)).ToList();
                    if (invalidOfficeIds.Any())
                        throw new EntityNotFoundException(typeof(SalesPersonOffice), invalidOfficeIds.First());

                    salesPerson.AddOffices(addedOfficesIds);
                }
            }

            if (dto.SupervisorUserId.HasValue)
            {
                var superVisor = await salesPersonRepository.GetByUserIdAsync(dto.SupervisorUserId.Value);
                salesPerson.SupervisorId = superVisor.Id;
            }
            else
            {
                salesPerson.SupervisorId = null;
            }

            if (dto.WorkingHours != null)
            {
                var removedDefaultTimeTableEntriesIds = salesPerson.DefaultTimeTable.Select(dtt => dtt.Id)
                    .Except(dto.WorkingHours.Where(x => x.Id.HasValue).Select(x => x.Id.Value))
                    .ToList();
                if (removedDefaultTimeTableEntriesIds.Any())
                {
                    salesPerson.RemoveDefaultTimeTableEntries(removedDefaultTimeTableEntriesIds);
                }
                foreach (var dtoEntry in dto.WorkingHours)
                {
                    if (dtoEntry.Id.HasValue)
                    {
                        salesPerson.UpdateDefaultTimeTableEntry(dtoEntry.Id.Value, dtoEntry.DayTypeId, dtoEntry.From, dtoEntry.To);
                    }
                    else
                    {
                        salesPerson.AddDefaultTimeTableEntry(dtoEntry.DayTypeId, dtoEntry.From, dtoEntry.To);
                    }
                }
            }
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
                throw new IFPSAppException("User doesn't exist");
            }
            var user = await userRepository.GetUserWithClaimsAndTokenByNameAsync(loginDto.Email);

            SignInResult result = await signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, false);

            if (!result.Succeeded)
                throw new IFPSValidationAppException(new Dictionary<string, List<string>>() { { nameof(SignInResult), new List<string>() { $"Result: {result.ToString()}" } } });

            var basket = await basketRepository.FirstOrDefaultAsync(ent => ent.Customer.UserId == user.Id);
            var claims = user.Claims.Select(p => new System.Security.Claims.Claim("IFPSClaim", p.Claim.Name.ToString()));
            var accountDto = new AccountDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.CurrentVersion.Name,
                RoleName = user.Roles.First().Role.Name,
                Language = user.Language,
                Image = new ImageThumbnailDetailsDto(user.Image),
                CompanyId = user.CompanyId,
                Claims = claims,
                BasketId = basket?.Id
            };

            if (user.CurrentVersion.ContactAddress.CountryId != null)
            {
                accountDto.Address = new AddressDetailsDto(user.CurrentVersion.ContactAddress);
            }

            return accountDto;
        }

        public async Task<int> RegisterAsync(UserCreateDto userCreateDto)
        {
            User user = null;
            var role = await roleRepository.FirstOrDefaultAsync(p => p.Division.DivisionType == DivisionTypeEnum.Customer);

            user = await CreateAndGetUserAsync(new UserCreateDto
            {
                Email = userCreateDto.Email,
                Name = userCreateDto.Name,
                RoleId = role.Id,
                Password = userCreateDto.Password
            });

            await SendConfirmEmailAsync(user.Email);

            return user.Id;
        }

        public async Task ConfirmEmailAsync(int userId, string token)
        {
            var user = await userRepository.SingleAsync(ent => ent.Id == userId);
            var result = await userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                throw new IFPSAppException($"Token is invalid: {token}");
        }

        public async Task SendResetPasswordEmailAsync(string email)
        {
            var user = await userRepository.SingleIncludingAsync(ent => ent.Email == email, ent => ent.CurrentVersion);
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            token = HttpUtility.UrlEncode(token);
            var builder = new BodyBuilder();
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.ResetPassword);

            await GenerateEmail(user, builder, EmailTypeEnum.ResetPassword);
            await emailService.SendEmailAsync(user, subject: emailSettings.ResetSubject, mimeEntity: builder.ToMessageBody(), emailData.Id);
        }

        public async Task<AccountDto> GetUserAccountByNameAndTokenAsync(UserWithTokenDto userWithTokenDto)
        {
            var user = await userRepository.GetUserWithClaimsAndTokenByNameAsync(userWithTokenDto.Email); // UserName

            if (!user.Tokens.Any(p => !p.IsDeleted && p.Value == userWithTokenDto.RefreshToken))
                throw new IFPSValidationAppException(new Dictionary<string, List<string>>() { { "RefreshToken", new List<string>() { $"User RefreshToken not found!" } } });

            var basket = await basketRepository.FirstOrDefaultAsync(ent => ent.Customer.UserId == user.Id);
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
                Claims = claims,
                Address = new AddressDetailsDto(user.CurrentVersion.ContactAddress),
                BasketId = basket?.Id
            };
        }

        public async Task UpdateRefreshTokenAsync(UserWithTokenDto userWithTokenDto)
        {
            var user = await userRepository.GetUserWithClaimsAndTokenByNameAsync(userWithTokenDto.Email); //UserName

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

        public async Task LogoutAsync(string email) //name
        {
            await signInManager.SignOutAsync();

            await UpdateRefreshTokenAsync(new UserWithTokenDto
            {
                //UserName = name,
                Email = email,
                RefreshToken = ""
            });
        }

        public async Task<bool> ValidateEmail(string email)
        {
            var users = await userRepository.GetAllListAsync(ent => ent.Email.Equals(email));
            return users.Count != 0;
        }

        public async Task ChangePasswordAsync(int userId, string token, PasswordChangeDto passwordChangeDto)
        {
            var user = await userRepository.SingleAsync(ent => ent.Id == userId);
            IdentityResult result = await userManager.ResetPasswordAsync(user, token, passwordChangeDto.Password);

            if (!result.Succeeded)
                throw new IFPSValidationAppException(new Dictionary<string, List<string>>() { { nameof(IdentityResult), new List<string>() { $"Result: {result.ToString()}" } } });

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<UserNameDto>> GetUserNamesByOwnCompanyAsync(int userId)
        {
            var companyId = await userRepository.SingleAsync(ent => ent.Id == userId, ent => ent.CompanyId);
            return await userRepository.GetAllListAsync(ent => ent.CompanyId == companyId, UserNameDto.Projection);
        }

        public async Task<UserProfileDetailsDto> GetUserProfileAsync(int id)
        {
            var user = await userRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.CurrentVersion, ent => ent.Image);
            return UserProfileDetailsDto.FromModel(user);
        }

        private async Task SendConfirmEmailAsync(string email)
        {
            User user = await userRepository.SingleAsync(ent => ent.Email == email);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            token = HttpUtility.UrlEncode(token);
            string url = localFiles.BaseUrl + $"/login?userId={user.Id}&token={token}";
            var builder = new BodyBuilder();
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.ConfirmEmail);
            string contents = System.IO.File.ReadAllText(Path.Combine(htmlContainerFolder, emailData.CurrentTranslation.FileName));

            var pathImage = Path.Combine(htmlContainerFolder, emailData.CurrentTranslation.ImageFileName);
            var image = builder.LinkedResources.Add(pathImage);

            image.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = string.Format(contents, user.CurrentVersion.Name, url, url);
            await emailService.SendEmailAsync(user, subject: emailSettings.RegisterSubject, mimeEntity: builder.ToMessageBody(), emailData.Id);

            user.AddEmail(new Email(user.Id, emailData.Id, Clock.Now));
            await unitOfWork.SaveChangesAsync();

        }

        public async Task<List<DivisionTypeEnum>> GetUserDivisions(int userId)
        {
            Expression<Func<User, List<DivisionTypeEnum>>> select = x => x.Roles.Select(ent => ent.Role.Division.DivisionType).ToList();

            return (await userRepository.GetAllListAsync<List<DivisionTypeEnum>>(ent => ent.Id == userId, select)).FirstOrDefault();

        }

        private async Task GenerateEmail(User user, BodyBuilder builder, EmailTypeEnum emailType)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            token = HttpUtility.UrlEncode(token);
            string url = localFiles.BaseUrl + $"/login?dialog=true&userId={user.Id}&passwordresettoken={token}";
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == emailType);
            string contents = System.IO.File.ReadAllText(Path.Combine(htmlContainerFolder, emailData.CurrentTranslation.FileName));

            var pathImage = Path.Combine(htmlContainerFolder, emailData.CurrentTranslation.ImageFileName);
            var image = builder.LinkedResources.Add(pathImage);

            image.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = string.Format(format: contents, image.ContentId, user.CurrentVersion.Name, url);
        }

        private async Task UpdateNotifications(NotificationsDto dto, Customer customer)
        {
            var NotificationType = NotificationTypeFlag.None;
            foreach (var flag in dto.NotificationTypeFlags)
            {
                NotificationType = NotificationType | (NotificationTypeFlag)Enum.Parse(typeof(NotificationTypeFlag), flag);
            }

            customer.NotificationType = NotificationType;

            var addedNotificationsEventTypeIds = dto.EventTypeIds.Except(customer.NotificationModes.Select(nm => nm.EventTypeId)).ToList();
            var removedNotificationsEventTypeIds = customer.NotificationModes.Select(nm => nm.EventTypeId).Except(dto.EventTypeIds).ToList();
            if (removedNotificationsEventTypeIds.Any())
            {
                customer.RemoveNotificationModes(removedNotificationsEventTypeIds);
            }
            if (addedNotificationsEventTypeIds.Any())
            {
                var eventTypes = await eventTypeRepository.GetEventTypesByIdsAsync(addedNotificationsEventTypeIds);
                var invalidEventTypesIds = addedNotificationsEventTypeIds.Except(eventTypes.Select(et => et.Id)).ToList();
                if (invalidEventTypesIds.Any())
                    throw new EntityNotFoundException(typeof(EventType), invalidEventTypesIds.First());

                customer.AddNotificationModes(addedNotificationsEventTypeIds);
            }
        }

        private async Task SetRolesForNewUserAsync(User newUser, Role role)
        {
            newUser.AddRolesById(new List<int>() { role.Id });
            foreach (Claim claim in role.DefaultRoleClaims.Select(ent => ent.Claim).ToList())
            {
                newUser.AddClaims(new List<int>() { claim.Id });
            }

            if (role.Division.DivisionType == DivisionTypeEnum.Customer)
            {
                await customerRepository.InsertAsync(new Customer(newUser.Id, Clock.Now));
            }

            if (Employee.EmployeesDivisions.Any(d => d == role.Division.DivisionType))
            {
                await employeeRepository.InsertAsync(new Employee(newUser.Id));
            }

            if (role.Division.DivisionType == DivisionTypeEnum.Sales)
            {
                await salesPersonRepository.InsertAsync(new SalesPerson(newUser.Id, Clock.Now));
            }
        }
    }
}