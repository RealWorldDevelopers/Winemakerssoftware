using FluentValidation;
using WMS.Business.Common;

namespace WMS.Business.Yeast.Dto
{
    /// <summary>
    /// Data Transfer Object representing a Code Literal Type Table with an added optional foreign key property
    /// </summary>
    public class YeastDto
    {
        public int? Id { get; set; }
        public CodeDto? Brand { get; set; }
        public CodeDto? Style { get; set; }
        public string? Trademark { get; set; }
        public int? TempMin { get; set; }
        public int? TempMax { get; set; }
        public double? Alcohol { get; set; }
        public string? Note { get; set; }
    }

    // TODO how to validate require and test ?
    // TODO validator https://docs.fluentvalidation.net/en/latest/custom-validators.html
    public class YeastDtoValidator : AbstractValidator<YeastDto>
    {
        public YeastDtoValidator()
        {
            RuleFor(dto => dto.Trademark).NotEmpty();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            RuleFor(dto => dto.Brand).SetValidator(new CodeDtoValidator());
            RuleFor(dto => dto.Style).SetValidator(new CodeDtoValidator());
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        }
    }

}
