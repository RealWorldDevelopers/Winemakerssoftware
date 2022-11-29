
using FluentValidation;

namespace WMS.Business.Recipe.Dto
{
    /// <summary>
    /// Data Transfer Object representing a Rating Table Entity
    /// </summary>
    public class RatingDto
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Total number of votes received
        /// </summary>
        public int TotalVotes { get; set; }

        /// <summary>
        /// Grand Total of all voting submissions
        /// </summary>
        public double TotalValue { get; set; }

        /// <summary>
        /// Foreign Key to a <see cref="RecipeDto"/>
        /// </summary>
        public int RecipeId { get; set; }

        /// <summary>
        /// Delimited String with IP Origins of previous voters
        /// </summary>
        public string? OriginIp { get; set; }

    }

    // TODO How to validate require what and test?  
    // TODO validator https://docs.fluentvalidation.net/en/latest/custom-validators.html
    public class RatingDtoValidator : AbstractValidator<RatingDto>
    {
        public RatingDtoValidator()
        {
            RuleFor(dto => dto.RecipeId).NotEmpty();
            RuleFor(dto => dto.TotalValue).NotEmpty();
            RuleFor(dto => dto.TotalVotes).NotEmpty();
        }
    }
}
