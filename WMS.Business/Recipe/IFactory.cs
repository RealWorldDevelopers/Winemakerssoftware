using WMS.Business.Recipe.Dto;
using WMS.Business.Common;
using WMS.Business.Image.Dto;
using System.Collections.Generic;
using WMS.Business.Common.Queries;

namespace WMS.Business.Recipe
{
    /// <summary>
    /// Factory Object used to create <see cref="IQuery{T}"/> and <see cref="ICommand{T}"/> objects
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Creates a New <see cref="CodeDto"/> Instance
        /// </summary>
        /// <param name="id">Primary Key (Code) as <see cref="int"/></param>
        /// <param name="parentId">Foreign Key as <see cref="int"/></param>
        /// <param name="literal">Literal (Value) as <see cref="string"/></param>
        /// <returns><see cref="CodeDto"/></returns>
        CodeDto CreateNewCode(int id, int parentId, string literal);

        /// <summary>
        /// Creates a New <see cref="Dto.ImageDto"/> Instance
        /// </summary>
        /// <param name="recipeId">Foreign Key to a <see cref="RecipeDto"/></param>
        /// <param name="fileName">File Name of Image</param>
        /// <param name="name">Header Name of image</param>
        /// <param name="data">Image Content</param>
        /// <param name="length">Size of image in bytes</param>
        /// <param name="contentType">File Type of image</param>
        /// <returns><see cref="ImageDto"/></returns>
        ImageDto CreateNewImageFile(int recipeId, string fileName, string name, byte[] data, byte[] thumb, long length, string contentType);

        /// <summary>
        /// Creates a New <see cref="Dto.RatingDto"/> Instance
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <param name="totalVotes">Total number of votes received</param>
        /// <param name="totalValue">Grand Total of all voting submissions</param>
        /// <param name="recipeId">Foreign Key to a <see cref="RecipeDto"/></param>
        /// <param name="originIp">Delimited String with IP Origins of previous voters</param>
        /// <returns><see cref="Dto.RatingDto"/></returns>
        RatingDto CreateNewRating(int id, int totalVotes, double totalValue, int recipeId, string originIp);

        /// <summary>
        /// Creates a New <see cref="Dto.RecipeDto"/> Instance
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <param name="submittedBy">Foreign Key of associated User who submitted recipe</param>
        /// <param name="title">The name given to this recipe</param>
        /// <param name="variety">Associated Variety Code Literal <see cref="CodeDto"/></param>
        /// <param name="description">Brief description of recipe</param>
        /// <param name="ingredients">Ingredients needed</param>
        /// <param name="instructions">How to instructions</param>
        /// <param name="rating">Website users approval rating</param>
        /// <param name="enabled">States if recipe able to be view and used</param>
        /// <param name="needsApproved">States if recipe needs to go through the approval process</param>
        /// <param name="hits">Number of time recipe has been viewed online</param>
        /// <param name="imageFiles">List of Image File Data <see cref="ImageDto"/></param>
        /// <returns><see cref="Dto.RecipeDto"/></returns>
        RecipeDto CreateNewRecipe(int id, string submittedBy, string title, CodeDto variety, string description, string ingredients, string instructions, RatingDto rating, bool enabled, bool needsApproved, int hits, List<ImageDto> imageFiles);

        /// <summary>
        /// Create a Unit of Measure Query
        /// </summary>
        /// <returns><see cref="IQueryUOM"/></returns>
        IQueryUOM CreateUOMQuery();

        /// <summary>
        /// Create a Category Query
        /// </summary>
        /// <returns><see cref="IQuery{ICode}"/></returns>
        IQuery<ICodeDto> CreateCategoriesQuery();

        /// <summary>
        /// Create a Rating Query
        /// </summary>
        /// <returns><see cref="IQuery{Rating}"/></returns>
        IQuery<RatingDto> CreateRatingsQuery();

        /// <summary>
        /// Create a Recipe Query
        /// </summary>
        /// <returns><see cref="IQuery{Recipe}"/></returns>
        IQuery<RecipeDto> CreateRecipesQuery();

        /// <summary>
        /// Create a Variety Query
        /// </summary>
        /// <returns><see cref="IQuery{ICode}"/></returns>
        IQuery<ICodeDto> CreateVarietiesQuery();

        /// <summary>
        /// Create a <see cref="Common.Dto.Code"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Common.Dto.Code}"/></returns>
        ICommand<ICodeDto> CreateVarietiesCommand();

        /// <summary>
        /// Create a <see cref="Common.Dto.Code"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Common.Dto.Code}"/></returns>
        ICommand<ICodeDto> CreateCategoriesCommand();

        /// <summary>
        /// Create a <see cref="Dto.RatingDto"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Dto.RatingDto}"/></returns>
        ICommand<RatingDto> CreateRatingsCommand();

        /// <summary>
        /// Create a <see cref="Dto.RecipeDto"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Dto.RecipeDto}"/></returns>
        ICommand<RecipeDto> CreateRecipesCommand();

        /// <summary>
        /// Create a <see cref="Dto.ImageDto"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Dto.ImageDto}"/></returns>
        ICommand<ImageDto> CreateImageCommand();
   
    }
}