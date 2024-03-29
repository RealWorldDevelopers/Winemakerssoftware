﻿using FluentValidation;
using System.Collections.Generic;
using WMS.Business.Common;
using WMS.Business.Image.Dto;
using WMS.Business.Journal.Dto;
using WMS.Business.Yeast.Dto;

namespace WMS.Business.Recipe.Dto
{
    /// <summary>
    /// Data Transfer Object representing a Recipe Table Entity
    /// </summary>
    public class RecipeDto
    {
        public RecipeDto()
        {
            ImageFiles = new List<ImageDto>();
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Foreign Key of associated User who submitted recipe
        /// </summary>
        public string? SubmittedBy { get; set; }

        /// <summary>
        /// The name given to this recipe
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Associated Category Code Literal <see cref="ICodeDto"/>
        /// </summary>
        public CodeDto? Category { get; set; }

        /// <summary>
        /// Associated Variety Code Literal <see cref="ICodeDto"/>
        /// </summary>
        public CodeDto? Variety { get; set; }

        /// <summary>
        /// Yeast to Use
        /// </summary>
        public YeastDto? Yeast { get; set; }

        /// <summary>
        /// Target Values
        /// </summary>
        public TargetDto? Target { get; set; }

        /// <summary>
        /// Brief description of recipe
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Ingredients needed
        /// </summary>
        public string? Ingredients { get; set; }

        /// <summary>
        /// How to instructions
        /// </summary>
        public string? Instructions { get; set; }

        /// <summary>
        /// Website users approval rating
        /// </summary>
        public RatingDto? Rating { get; set; }

        /// <summary>
        /// States if recipe able to be view and used
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// States if recipe needs to go through the approval process
        /// </summary>
        public bool NeedsApproved { get; set; } = true;

        /// <summary>
        /// Number of time recipe has been viewed online
        /// </summary>
        public int? Hits { get; set; }

        /// <summary>
        /// List of Image File Data <see cref="ImageFileDto"/>
        /// </summary>
        public List<ImageDto> ImageFiles { get; }

    }

    // TODO How to Validate and Test
    // TODO add fluent validation https://docs.fluentvalidation.net/en/latest/custom-validators.html
    public class RecipeDtoValidator : AbstractValidator<RecipeDto>
    {
        public RecipeDtoValidator()
        {
            RuleFor(dto => dto.Title).NotEmpty();
            RuleFor(dto => dto.Description).NotEmpty();
            RuleFor(dto => dto.NeedsApproved).NotEmpty();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            RuleFor(dto => dto.Category).SetValidator(new CodeDtoValidator());
            RuleFor(dto => dto.Variety).SetValidator(new CodeDtoValidator()); 
            RuleFor(dto => dto.Yeast).SetValidator(new YeastDtoValidator());
            RuleFor(dto => dto.Target).SetValidator(new TargetDtoValidator());
            RuleFor(dto => dto.Rating).SetValidator(new RatingDtoValidator());
            RuleForEach(dto => dto.ImageFiles).SetValidator(new ImageDtoValidator());
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        }
    }

}
