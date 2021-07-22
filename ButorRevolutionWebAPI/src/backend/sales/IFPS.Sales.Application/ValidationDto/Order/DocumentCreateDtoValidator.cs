using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto.Order
{
    public class DocumentCreateDtoValidator : AbstractValidator<DocumentCreateDto>
    {
        public DocumentCreateDtoValidator()
        {
            RuleFor(x => x.ContainerName).NotNull().NotEmpty();
            RuleFor(x => x.FileName).NotNull().NotEmpty();
        }
    }
}
