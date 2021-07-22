using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto
{
    public class ImageUpdateDtoValidator : AbstractValidator<ImageUpdateDto>
    {
        public ImageUpdateDtoValidator()
        {
            RuleFor(x => x.ContainerName).NotNull().NotEmpty();
            RuleFor(x => x.FileName).NotNull().NotEmpty();
        }
    }
}