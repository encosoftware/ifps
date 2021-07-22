using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class VenueUpdateValidatorDto : AbstractValidator<VenueUpdateDto>
    {
        public VenueUpdateValidatorDto()
        {
            RuleFor(ent => ent.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(ent => ent.Name).NotNull().NotEmpty();
            RuleFor(ent => ent.PhoneNumber).NotNull().NotEmpty();
            RuleFor(ent => ent.OfficeAddress).SetValidator(new AddressUpdateDtoValidator());

            RuleForEach(ent => ent.MeetingRooms).SetValidator(new MeetingRoomUpdateValidatorDto());
            RuleForEach(ent => ent.OpeningHours).SetValidator(new VenueDateRangeUpdateValidatorDto());
        }
    }
}
