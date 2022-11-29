using FluentValidation;

namespace WMS.Business.Common
{
    /// <inheritdoc cref="ICodeDto"/>
    public class CodeDto : ICodeDto
    {
        /// <inheritdoc cref="ICodeDto.Id"/>
        public int? Id { get; set; }

        /// <inheritdoc cref="ICodeDto.ParentId"/>
        public int? ParentId { get; set; }

        /// <inheritdoc cref="ICodeDto.Literal"/>
        public string Literal { get; set; } = string.Empty;

        /// <inheritdoc cref="ICodeDto.Enabled"/>
        public bool Enabled { get; set; } = true;

        /// <inheritdoc cref="ICodeDto.Description"/>
        public string Description { get; set; } = string.Empty;

    }

    // TODO How to validate require what and test?  
    // TODO validator https://docs.fluentvalidation.net/en/latest/custom-validators.html
    public class CodeDtoValidator : AbstractValidator<CodeDto>
    {
        public CodeDtoValidator()
        {
            RuleFor(dto => dto.Description).NotEmpty();
            RuleFor(dto => dto.Enabled).NotEmpty();
            RuleFor(dto => dto.Literal).NotEmpty();
        }
    }

}
