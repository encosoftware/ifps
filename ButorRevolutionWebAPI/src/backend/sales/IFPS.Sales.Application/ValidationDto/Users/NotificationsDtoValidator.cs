using FluentValidation;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Domain.Enums;
using System;
using System.Linq;

namespace IFPS.Sales.Application.ValidationDto
{
    public class NotificationsDtoValidator : AbstractValidator<NotificationsDto>
    {
        public NotificationsDtoValidator()
        {
            var NotificationTypeFlagValues = Enum.GetValues(typeof(NotificationTypeFlag))
                                                .Cast<NotificationTypeFlag>()
                                                .Select(x => x.ToString())
                                                .ToList();
            RuleFor(x => x.NotificationTypeFlags).NotNull();
            RuleForEach(x => x.NotificationTypeFlags).Must(x => NotificationTypeFlagValues.Contains(x))
                .WithMessage("List must contains only valid enum values!");
            RuleFor(x => x.EventTypeIds).NotNull();
        }
    }
}