using System.Collections.Generic;
using WMS.Business.Shared;

namespace WMS.Business.Recipe.Dto
{
    /// <summary>
    /// Factory Object used to create <see cref="Dto"/> objects
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Creates a New <see cref="ICode"/> Instance
        /// </summary>
        /// <param name="id">Primary Key (Code) as <see cref="int"/></param>
        /// <param name="parentId">Foreign Key as <see cref="int"/></param>
        /// <param name="literal">Literal (Value) as <see cref="string"/></param>
        /// <returns><see cref="ICode"/></returns>
        ICode CreateNewCode(int id, int parentId, string literal);

        /// <summary>
        /// Creates a New <see cref="Dto.ImageFile"/> Instance
        /// </summary>
        /// <param name="recipeId">Foreign Key to a <see cref="Recipe"/></param>
        /// <param name="fileName">File Name of Image</param>
        /// <param name="name">Header Name of image</param>
        /// <param name="data">Image Content</param>
        /// <param name="length">Size of image in bytes</param>
        /// <param name="contentType">File Type of image</param>
        /// <returns><see cref="ImageFile"/></returns>
        ImageFile CreateNewImageFile(int recipeId, string fileName, string name, byte[] data, byte[] thumb, long length, string contentType);

        /// <summary>
        /// Creates a New <see cref="Dto.Rating"/> Instance
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <param name="totalVotes">Total number of votes received</param>
        /// <param name="totalValue">Grand Total of all voting submissions</param>
        /// <param name="recipeId">Foreign Key to a <see cref="Recipe"/></param>
        /// <param name="originIp">Delimited String with IP Origins of previous voters</param>
        /// <returns><see cref="Dto.Rating"/></returns>
        Rating CreateNewRating(int id, int totalVotes, double totalValue, int recipeId, string originIp);

        /// <summary>
        /// Creates a New <see cref="Dto.Recipe"/> Instance
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <param name="submittedBy">Foreign Key of associated User who submitted recipe</param>
        /// <param name="title">The name given to this recipe</param>
        /// <param name="variety">Associated Variety Code Literal <see cref="ICode"/></param>
        /// <param name="description">Brief description of recipe</param>
        /// <param name="ingredients">Ingredients needed</param>
        /// <param name="instructions">How to instructions</param>
        /// <param name="rating">Website users approval rating</param>
        /// <param name="enabled">States if recipe able to be view and used</param>
        /// <param name="needsApproved">States if recipe needs to go through the approval process</param>
        /// <param name="hits">Number of time recipe has been viewed online</param>
        /// <param name="imageFiles">List of Image File Data <see cref="ImageFile"/></param>
        /// <returns><see cref="Dto.Recipe"/></returns>
        Recipe CreateNewRecipe(int id, string submittedBy, string title, ICode variety, string description, string ingredients, string instructions, Rating rating, bool enabled, bool needsApproved, int hits, List<ImageFile> imageFiles);
        
    }
}