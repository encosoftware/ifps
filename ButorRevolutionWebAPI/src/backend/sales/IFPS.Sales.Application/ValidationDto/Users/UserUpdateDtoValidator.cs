using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(x => x.Language).NotNull().NotEmpty();
            RuleFor(x => x.OwnedClaimsIds).NotNull();
            RuleFor(x => x.OwnedRolesIds).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.PhoneNumber).NotNull();

            //RuleFor(x => x.Address).SetValidator(new AddressCreateDtoValidator());
            RuleFor(x => x.WorkingInfo).SetValidator(new UpdateWorkingInfoDtoValidator());
            RuleFor(x => x.Notifications).SetValidator(new NotificationsDtoValidator());
        }
    }
}