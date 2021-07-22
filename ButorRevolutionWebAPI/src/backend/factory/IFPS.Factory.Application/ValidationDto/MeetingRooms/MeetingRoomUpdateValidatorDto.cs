using FluentValidation;
using IFPS.Factory.Application.Dto;

namespace IFPS.Factory.Application.ValidationDto
{
    public class MeetingRoomUpdateValidatorDto : AbstractValidator<MeetingRoomUpdateDto>
    {
        public MeetingRoomUpdateValidatorDto()
        {
            RuleForEach(ent => ent.Translations).SetValidator(new MeetingRoomTranslationUpdateValidatorDto());
        }
    }
}
