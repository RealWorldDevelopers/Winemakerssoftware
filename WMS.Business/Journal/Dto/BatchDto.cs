
using FluentValidation;
using System.Collections.Generic;
using WMS.Business.Common;
using WMS.Business.Image.Dto;
using WMS.Business.Recipe.Dto;
using WMS.Business.Yeast.Dto;

namespace WMS.Business.Journal.Dto
{
    /// <summary>
    /// Data Transfer Object representing a Batch Table Entity
    /// </summary>
    public class BatchDto
    {
        public BatchDto()
        {
            Entries = new List<BatchEntryDto>();
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The name given to this Batch
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Brief description of Batch
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Size of Batch
        /// </summary>
        public double? Volume { get; set; }

        /// <summary>
        /// Unit of Measure for Volume
        /// </summary>
        public UnitOfMeasureDto? VolumeUom { get; set; }

        /// <summary>
        /// Foreign Key of Associated User who Submitted Batch
        /// </summary>
        public string? SubmittedBy { get; set; }

        /// <summary>
        /// Year of Batch
        /// </summary>
        public int? Vintage { get; set; }

        /// <summary>
        /// Foreign Key of Variety for Batch
        /// </summary>
        public CodeDto? Variety { get; set; }

        /// <summary>
        /// Foreign Key of a Set of Targets for Batch
        /// </summary>
        public TargetDto? Target { get; set; }

        /// <summary>
        /// Foreign Key of Related Recipe for Batch
        /// </summary>
        public int? RecipeId { get; set; }

        /// <summary>
        /// Foreign Key of Related Yeast for Batch
        /// </summary>
        public YeastDto? Yeast { get; set; }

        /// <summary>
        /// Foreign Key of Related MLF Culture for Batch
        /// </summary>
        public int? MaloCultureId { get; set; }

        /// <summary>
        /// Is Batch Completed
        /// </summary>
        public bool Complete { get; set; } = false;

        public List<BatchEntryDto>? Entries { get; }

    }

    // TODO How to Validate and Test
    // TODO add fluent validation https://docs.fluentvalidation.net/en/latest/custom-validators.html
    public class BatchDtoValidator : AbstractValidator<BatchDto>
    {
        public BatchDtoValidator()
        {
            RuleFor(dto => dto.Title).NotEmpty();
            RuleFor(dto => dto.Description).NotEmpty();
            RuleFor(dto => dto.Complete).NotEmpty();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            RuleFor(dto => dto.VolumeUom).SetValidator(new UnitOfMeasureDtoValidator());
            RuleFor(dto => dto.Variety).SetValidator(new CodeDtoValidator());
            RuleFor(dto => dto.Target).SetValidator(new TargetDtoValidator());
            RuleFor(dto => dto.Yeast).SetValidator(new YeastDtoValidator());
            RuleForEach(dto => dto.Entries).SetValidator(new BatchEntryDtoValidator());
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        }
    }

}
