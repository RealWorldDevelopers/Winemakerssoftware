using WMS.Business.Recipe.Dto;
using WMS.Business.Common;

namespace WMS.Business.Recipe.Queries
{
    /// <summary>
    /// Factory Object used to create <see cref="IQuery{T}"/> objects
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Create a Category Query
        /// </summary>
        /// <returns><see cref="IQuery{ICode}"/></returns>
        IQuery<ICode> CreateCategoriesQuery();

        /// <summary>
        /// Create a Rating Query
        /// </summary>
        /// <returns><see cref="IQuery{Rating}"/></returns>
        IQuery<RatingDto> CreateRatingsQuery();

        /// <summary>
        /// Create a Recipe Query
        /// </summary>
        /// <returns><see cref="IQuery{Recipe}"/></returns>
        IQuery<Dto.RecipeDto> CreateRecipesQuery();

        /// <summary>
        /// Create a Variety Query
        /// </summary>
        /// <returns><see cref="IQuery{ICode}"/></returns>
        IQuery<ICode> CreateVarietiesQuery();
    }
}