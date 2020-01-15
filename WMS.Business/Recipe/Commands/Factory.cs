using AutoMapper;
using WMS.Business.Recipe.Dto;
using WMS.Business.Common;
using WMS.Data;

namespace WMS.Business.Recipe.Commands
{
    /// <summary>
    /// Instance of <see cref="Commands.ICommand{T}"/> Factory
    /// </summary>
    /// <inheritdoc cref="IFactory"/>>
    public class Factory : IFactory
    {
        private readonly WMSContext _recipeContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Command Factory Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public Factory(WMSContext dbContext, IMapper mapper)
        {
            _recipeContext = dbContext;
            _mapper = mapper;
        }

        /// <inheritdoc cref="IFactory.CreateVarietiesCommand"/>>
        public ICommand<ICode> CreateVarietiesCommand()
        {
            return new ModifyVariety(_recipeContext, _mapper);
        }

        /// <inheritdoc cref="IFactory.CreateCategoriesCommand"/>>
        public ICommand<ICode> CreateCategoriesCommand()
        {
            return new ModifyCategory(_recipeContext, _mapper);
        }

        /// <inheritdoc cref="IFactory.CreateRecipesCommand"/>>
        public ICommand<Dto.RecipeDto> CreateRecipesCommand()
        {
            return new ModifyRecipes(_recipeContext, _mapper);
        }

        /// <inheritdoc cref="IFactory.CreateRatingsCommand"/>>
        public ICommand<RatingDto> CreateRatingsCommand()
        {
            return new ModifyRatings(_recipeContext, _mapper);
        }


        /// <inheritdoc cref="IFactory.CreateImageCommand"/>>
        public ICommand<ImageFileDto> CreateImageCommand()
        {
            return new ModifyImages(_recipeContext);
        }


    }
}
