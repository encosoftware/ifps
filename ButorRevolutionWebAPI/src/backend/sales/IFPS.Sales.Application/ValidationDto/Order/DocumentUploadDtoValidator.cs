using FluentValidation;
using IFPS.Sales.Application.Dto;

namespace IFPS.Sales.Application.ValidationDto.Order
{
    public class DocumentUploadDtoValidator : AbstractValidator<DocumentUploadDto>
    {
        public DocumentUploadDtoValidator()
        {
            RuleFor(x => x.DocumentGroupId).GreaterThan(0);
            RuleFor(x => x.DocumentGroupVersionId).Must(x => !x.HasValue || (x.HasValue && x.Value > 0));
            RuleFor(x => x.Documents).NotNull().NotEmpty();
            RuleForEach(x => x.Documents).SetValidator(new DocumentCreateDtoValidator());
            RuleFor(x => x.DocumentTypeId).GreaterThan(0);
            RuleFor(x => x.UploaderUserId).GreaterThan(0);
        }
    }
}
