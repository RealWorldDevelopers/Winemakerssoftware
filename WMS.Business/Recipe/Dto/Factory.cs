using System.Collections.Generic;
using WMS.Business.Common;

namespace WMS.Business.Recipe.Dto
{
    /// <summary>
    /// Instance of <see cref="Dto"/> Factory
    /// </summary>
    /// <inheritdoc cref="IFactory"/>>
    public class Factory : IFactory
    {
        /// <inheritdoc cref="IFactory.CreateNewCode"/>>
        public ICode CreateNewCode(int id, int parentId, string literal)
        {
            var dto = new Code
            {
                Id = id,
                Literal = literal,
                ParentId = parentId
            };
            return dto;
        }

        /// <inheritdoc cref="IFactory.CreateNewRating"/>>
        public RatingDto CreateNewRating(int id, int totalVotes, double totalValue, int recipeId, string originIp)
        {
            var dto = new RatingDto
            {
                Id = id,
                TotalValue = totalValue,
                TotalVotes = totalVotes,
                RecipeId = recipeId,
                OriginIp = originIp
            };
            return dto;
        }

        /// <inheritdoc cref="IFactory.CreateNewRecipe"/>>
        public RecipeDto CreateNewRecipe(int id, string submittedBy, string title, ICode variety, string description,
            string ingredients, string instructions, RatingDto rating, bool enabled, bool needsApproved, int hits, List<ImageFileDto> imageFiles)
        {
            var dto = new RecipeDto
            {
                Id = 0,
                SubmittedBy = submittedBy,
                Title = title,
                Variety = variety,
                Description = description,
                Ingredients = ingredients,
                Instructions = instructions,
                Rating = rating,
                Enabled = enabled,
                NeedsApproved = needsApproved,
                Hits = hits
            };
            dto.ImageFiles.AddRange(imageFiles);

            return dto;
        }

        /// <inheritdoc cref="IFactory.CreateNewImageFile"/>>
        public ImageFileDto CreateNewImageFile(int recipeId, string fileName, string name, byte[] data, byte[] thumb, long length, string contentType)
        {
            var dto = new ImageFileDto(thumb, data)
            {
                RecipeId = recipeId,
                FileName = fileName,
                Name = name,
                Length = length,
                ContentType = contentType
            };
            return dto;
        }


    }
}
