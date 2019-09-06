using WMS.Business.Recipe.Dto;
using WMS.Business.Shared;

namespace WMS.Business.Recipe.Commands
{
    /// <summary>
    /// Factory Object used to create <see cref="ICommand{T}"/> objects
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Create a <see cref="Shared.Dto.Code"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Shared.Dto.Code}"/></returns>
        ICommand<ICode> CreateVarietiesCommand();

        /// <summary>
        /// Create a <see cref="Shared.Dto.Code"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Shared.Dto.Code}"/></returns>
        ICommand<ICode> CreateCategoriesCommand();

        /// <summary>
        /// Create a <see cref="Dto.Rating"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Dto.Rating}"/></returns>
        ICommand<Rating> CreateRatingsCommand();

        /// <summary>
        /// Create a <see cref="Dto.Recipe"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Dto.Recipe}"/></returns>
        ICommand<Dto.Recipe> CreateRecipesCommand();

        /// <summary>
        /// Create a <see cref="Dto.ImageFile"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{Dto.ImageFile}"/></returns>
        ICommand<ImageFile> CreateImageCommand();
    }
}