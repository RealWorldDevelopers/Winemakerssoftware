using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using WMS.Business.Common;
using WMS.Business.Common.Queries;
using WMS.Business.Image.Commands;
using WMS.Business.Image.Dto;
using WMS.Business.Recipe.Commands;
using WMS.Business.Recipe.Dto;
using WMS.Business.Recipe.Queries;
using WMS.Data.SQL;

namespace WMS.Business.Recipe
{
    /// <summary>
    /// Instance of <see cref="IFactory"/> 
    /// </summary>
    /// <inheritdoc cref="IFactory"/>>
    public class Factory : IFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WMSContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Query Factory Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public Factory(IServiceProvider serviceProvider, WMSContext dbContext, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <inheritdoc cref="IFactory.CreateNewCode"/>>
        public CodeDto CreateNewCode(int id, int parentId, string literal)
        {
            var dto = new CodeDto
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
        public RecipeDto CreateNewRecipe(int id, string submittedBy, string title, CodeDto variety, string description,
            string ingredients, string instructions, RatingDto rating, bool enabled, bool needsApproved, int hits, List<ImageDto> imageFiles)
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

        /// <inheritdoc cref="IFactory.CreateUOMQuery"/>>
        public IQueryUOM CreateUOMQuery()
        {
            return ActivatorUtilities.CreateInstance<GetUOMs>(_serviceProvider, _dbContext, _mapper);
        }

        /// <inheritdoc cref="IFactory.CreateNewImageFile"/>>
        public ImageDto CreateNewImageFile(int recipeId, string fileName, string name, byte[] data, byte[] thumb, long length, string contentType)
        {
            var dto = new ImageDto()
            {
                RecipeId = recipeId,
                FileName = fileName,
                Name = name,
                Length = length,
                ContentType = contentType, 
                Data = data, 
                Thumbnail = thumb
            };
            return dto;
        }


        /// <inheritdoc cref="IFactory.CreateVarietiesQuery"/>>
        public IQuery<ICodeDto> CreateVarietiesQuery()
        {
            return ActivatorUtilities.CreateInstance<GetVarieties>(_serviceProvider, _dbContext, _mapper);
        }

        
        /// <inheritdoc cref="IFactory.CreateCategoriesQuery"/>>
        public IQuery<ICodeDto> CreateCategoriesQuery()
        {
            return ActivatorUtilities.CreateInstance<GetCategories>(_serviceProvider, _dbContext, _mapper);
        }

       
        /// <inheritdoc cref="IFactory.CreateRecipesCommand"/>>
        public IQuery<Dto.RecipeDto> CreateRecipesQuery()
        {
            return ActivatorUtilities.CreateInstance<GetRecipes>(_serviceProvider, _dbContext, _mapper);
        }

        
        /// <inheritdoc cref="IFactory.CreateRatingsQuery"/>>
        public IQuery<Dto.RatingDto> CreateRatingsQuery()
        {
            return ActivatorUtilities.CreateInstance<GetRatings>(_serviceProvider, _dbContext, _mapper);
        }



        /// <inheritdoc cref="IFactory.CreateVarietiesCommand"/>>
        public ICommand<ICodeDto> CreateVarietiesCommand()
        {
             return ActivatorUtilities.CreateInstance<ModifyVariety>(_serviceProvider, _dbContext, _mapper);
        }

        /// <inheritdoc cref="IFactory.CreateCategoriesCommand"/>>
        public ICommand<ICodeDto> CreateCategoriesCommand()
        {
             return ActivatorUtilities.CreateInstance<ModifyCategory>(_serviceProvider, _dbContext, _mapper);
        }

        /// <inheritdoc cref="IFactory.CreateRecipesCommand"/>>
        public ICommand<Dto.RecipeDto> CreateRecipesCommand()
        {
            return ActivatorUtilities.CreateInstance<ModifyRecipes>(_serviceProvider, _dbContext, _mapper);
        }
           

        ICommand<Dto.RatingDto> IFactory.CreateRatingsCommand()
        {
           return ActivatorUtilities.CreateInstance<ModifyRatings>(_serviceProvider, _dbContext, _mapper);
        }

        ICommand<ImageDto> IFactory.CreateImageCommand()
        {
            return ActivatorUtilities.CreateInstance<ModifyImage>(_serviceProvider, _dbContext, _mapper);
        }

    }
}

