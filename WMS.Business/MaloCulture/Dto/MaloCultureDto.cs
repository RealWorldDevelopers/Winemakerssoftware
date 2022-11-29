using FluentValidation;
using WMS.Business.Common;
using WMS.Business.Image.Dto;
using WMS.Business.Journal.Dto;
using WMS.Business.Recipe.Dto;
using WMS.Business.Yeast.Dto;

namespace WMS.Business.MaloCulture.Dto
{
    public class MaloCultureDto
    {
        public int? Id { get; set; }
        public CodeDto? Brand { get; set; }
        public CodeDto? Style { get; set; }
        public string Trademark { get; set; } = string.Empty;
        public int? TempMin { get; set; }
        public int? TempMax { get; set; }
        public double? Alcohol { get; set; }
        public double? So2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Correct Name")]
        public double? pH { get; set; }
        public string? Note { get; set; }

    }

    // TODO How to Validate and Test
    // TODO add fluent validation https://docs.fluentvalidation.net/en/latest/custom-validators.html
    public class MaloCultureDtoValidator : AbstractValidator<MaloCultureDto>
    {
        public MaloCultureDtoValidator()
        {
            RuleFor(dto => dto.Trademark).NotEmpty();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            RuleFor(dto => dto.Brand).SetValidator(new CodeDtoValidator());
            RuleFor(dto => dto.Style).SetValidator(new CodeDtoValidator());
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        }
    }

}
