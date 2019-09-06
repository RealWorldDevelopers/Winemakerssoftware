using AutoMapper;
using WMS.Business.Shared;
using WMS.Data;

namespace WMS.Business.Recipe.Queries
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
        /// Query Factory Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public Factory(WMSContext dbContext, IMapper mapper)
        {
            _recipeContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Instance of Create Variety Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateVarietiesQuery"/>>
        public IQuery<ICode> CreateVarietiesQuery()
        {
            return new GetVarieties(_recipeContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Category Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateCategoriesQuery"/>>
        public IQuery<ICode> CreateCategoriesQuery()
        {
            return new GetCategories(_recipeContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Recipe Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateRecipesCommand"/>>
        public IQuery<Dto.Recipe> CreateRecipesQuery()
        {
            return new GetRecipes(_recipeContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Rating Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateRatingsQuery"/>>
        public IQuery<Dto.Rating> CreateRatingsQuery()
        {
            return new GetRatings(_recipeContext, _mapper);
        }

    }
}

