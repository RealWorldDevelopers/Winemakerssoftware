using FluentValidation;
using System;
using WMS.Business.Common;

namespace WMS.Business.Journal.Dto
{
    public class BatchEntryDto
    {
        public int? Id { get; set; }
        public int? BatchId { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public DateTime? ActionDateTime { get; set; }
        public double? Temp { get; set; }
        public UnitOfMeasureDto? TempUom { get; set; }
        public double? pH { get; set; }
        public double? Sugar { get; set; }
        public UnitOfMeasureDto? SugarUom { get; set; }
        public double? Ta { get; set; }
        public double? So2 { get; set; }
        public string? Additions { get; set; }
        public string? Comments { get; set; }
        public bool Racked { get; set; } = false;
        public bool Filtered { get; set; } = false;
        public bool Bottled { get; set; } = false;

    }

    // TODO How to validate require what and test?  
    // TODO validator https://docs.fluentvalidation.net/en/latest/custom-validators.html
    public class BatchEntryDtoValidator : AbstractValidator<BatchEntryDto>
    {
        public BatchEntryDtoValidator()
        {
            RuleFor(dto => dto.BatchId).NotEmpty();
            RuleFor(dto => dto.EntryDateTime).NotEmpty();
            RuleFor(dto => dto.Racked).NotEmpty();
            RuleFor(dto => dto.Filtered).NotEmpty();
            RuleFor(dto => dto.Bottled).NotEmpty();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            RuleFor(dto => dto.SugarUom).SetValidator(new UnitOfMeasureDtoValidator());
            RuleFor(dto => dto.TempUom).SetValidator(new UnitOfMeasureDtoValidator());
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        }
    }
}
