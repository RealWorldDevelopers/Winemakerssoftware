using WMS.Business.Recipe.Dto;
using WMS.Business.Common;

namespace WMS.Business.Recipe.Commands
{
    /// <summary>
    /// Factory Object used to create <see cref="ICommand{T}"/> objects
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Create a <see cref="Common.Dto.Code"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Common.Dto.Code}"/></returns>
        ICommand<ICode> CreateVarietiesCommand();

        /// <summary>
        /// Create a <see cref="Common.Dto.Code"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Common.Dto.Code}"/></returns>
        ICommand<ICode> CreateCategoriesCommand();

        /// <summary>
        /// Create a <see cref="Dto.RatingDto"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Dto.RatingDto}"/></returns>
        ICommand<RatingDto> CreateRatingsCommand();

        /// <summary>
        /// Create a <see cref="Dto.RecipeDto"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Dto.RecipeDto}"/></returns>
        ICommand<Dto.RecipeDto> CreateRecipesCommand();

        /// <summary>
        /// Create a <see cref="Dto.ImageFileDto"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Dto.ImageFileDto}"/></returns>
        ICommand<ImageFileDto> CreateImageCommand();
    }
}