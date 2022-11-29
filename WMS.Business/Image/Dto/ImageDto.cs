
using FluentValidation;
using WMS.Business.Common;

namespace WMS.Business.Image.Dto
{
    /// <summary>
    /// Data Transfer Object representing a Image Table Entity
    /// </summary>
    public class ImageDto
    {     
                     

        /// <summary>
        /// Primary Key
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Foreign Key to a <see cref="RecipeDto"/>
        /// </summary>
        public int RecipeId { get; set; }

        /// <summary>
        /// Image File Name
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Image Header Name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Image Content
        /// </summary>
        public byte[]? Data { get; set; }      

        /// <summary>
        /// Thumbnail Content
        /// </summary>
        public byte[]? Thumbnail { get; set; }
       
        /// <summary>
        /// Size Property in Bytes
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Image Type
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

    }

    // TODO How to validate require what and test?  
    // TODO validator https://docs.fluentvalidation.net/en/latest/custom-validators.html
    public class ImageDtoValidator : AbstractValidator<ImageDto>
    {
        public ImageDtoValidator()
        {
            RuleFor(dto => dto.RecipeId).NotEmpty();
            RuleFor(dto => dto.FileName).NotEmpty();
            RuleFor(dto => dto.ContentType).NotEmpty();
        }
    }

}
