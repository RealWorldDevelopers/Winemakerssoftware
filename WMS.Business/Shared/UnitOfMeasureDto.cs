

using FluentValidation;

namespace WMS.Business.Common
{

    /// <inheritdoc cref="IUnitOfMeasureDto"/>
    public class UnitOfMeasureDto : IUnitOfMeasureDto
    {
        /// <inheritdoc cref="IUnitOfMeasureDto.Id"/>
        public int? Id { get; set; }

        /// <inheritdoc cref="IUnitOfMeasureDto.Abbreviation"/>
        public string? Abbreviation { get; set; }

        /// <inheritdoc cref="IUnitOfMeasureDto.Name"/>
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc cref="IUnitOfMeasureDto.Description"/>
        public string Description { get; set; } = string.Empty;

        /// <inheritdoc cref="IUnitOfMeasureDto.Enabled"/>
        public bool Enabled { get; set; } = true;
               

    }

    // TODO How to validate require what and test?  
    // TODO validator https://docs.fluentvalidation.net/en/latest/custom-validators.html
    public class UnitOfMeasureDtoValidator : AbstractValidator<UnitOfMeasureDto>
    {
        public UnitOfMeasureDtoValidator()
        {
            RuleFor(dto => dto.Description).NotEmpty();
            RuleFor(dto => dto.Enabled).NotEmpty();
            RuleFor(dto => dto.Name).NotEmpty();
        }
    }

}
