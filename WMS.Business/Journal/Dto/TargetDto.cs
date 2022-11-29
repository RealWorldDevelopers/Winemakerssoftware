using FluentValidation;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;

namespace WMS.Business.Journal.Dto
{
    /// <summary>
    /// Data Transfer Object representing a Target Table Entity
    /// </summary>
    public class TargetDto
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Target Fermentation Temperature
        /// </summary>
        public double? Temp { get; set; }

        /// <summary>
        /// Unit of Measure for Temp
        /// </summary>
        public UnitOfMeasureDto? TempUom { get; set; }


        /// <summary>
        /// Target pH
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Proper Spelling")]
        public double? pH { get; set; }

        /// <summary>
        /// Target Total Acidity
        /// </summary>
        public double? TA { get; set; }

        /// <summary>
        /// Target Starting Sugar
        /// </summary>
        public double? StartSugar { get; set; }

        /// <summary>
        /// Unit of Measure for Starting Sugar
        /// </summary>
        public UnitOfMeasureDto? StartSugarUom { get; set; }

        /// <summary>
        /// Target Ending Sugar
        /// </summary>
        public double? EndSugar { get; set; }

        /// <summary>
        /// Unit of Measure for Ending Sugar
        /// </summary>
        public UnitOfMeasureDto? EndSugarUom { get; set; }


    }

    // TODO how to validate require or test ?
    // TODO validator https://docs.fluentvalidation.net/en/latest/custom-validators.html
    public class TargetDtoValidator : AbstractValidator<TargetDto>
    {
        public TargetDtoValidator()
        {            
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            RuleFor(dto => dto.StartSugarUom).SetValidator(new UnitOfMeasureDtoValidator());
            RuleFor(dto => dto.TempUom).SetValidator(new UnitOfMeasureDtoValidator());
            RuleFor(dto => dto.EndSugarUom).SetValidator(new UnitOfMeasureDtoValidator());
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        }
    }

}
