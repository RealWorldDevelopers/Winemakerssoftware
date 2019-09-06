using System.Collections.Generic;
using WMS.Business.Shared;

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
        public Rating CreateNewRating(int id, int totalVotes, double totalValue, int recipeId, string originIp)
        {
            var dto = new Rating
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
        public Recipe CreateNewRecipe(int id, string submittedBy, string title, ICode variety, string description,
            string ingredients, string instructions, Rating rating, bool enabled, bool needsApproved, int hits, List<ImageFile> imageFiles)
        {
            var dto = new Recipe
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
                Hits = hits,
                ImageFiles = imageFiles
            };

            return dto;
        }

        /// <inheritdoc cref="IFactory.CreateNewImageFile"/>>
        public ImageFile CreateNewImageFile( int recipeId, string fileName, string name, byte[] data, byte[] thumb, long length, string contentType)
        {
            var dto = new ImageFile
            {                
                RecipeId = recipeId,
                FileName = fileName,
                Name=name,
                Data=data,
                Thumbnail = thumb,
                Length=length,
                ContentType=contentType
            };
            return dto;
        }


    }
}
