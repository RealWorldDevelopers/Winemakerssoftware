using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using WMS.Business.Common;
using WMS.Business.Yeast.Commands;
using WMS.Business.Yeast.Dto;
using WMS.Business.Yeast.Queries;
using WMS.Data.SQL;

namespace WMS.Business.Yeast
{
    public class Factory : IFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WMSContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Query Factory Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">Auto Mapper Instance as <see cref="IMapper"/></param>
        public Factory(IServiceProvider serviceProvider, WMSContext dbContext, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Instance of Create Yeast Command
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateYeastsCommand"/>>
        public ICommand<YeastDto> CreateYeastsCommand()
        {
            return ActivatorUtilities.CreateInstance<ModifyYeast>(_serviceProvider, _dbContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Yeast Pair Command
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateYeastPairCommand"/>>
        public ICommand<YeastPairDto> CreateYeastPairCommand()
        {
           return ActivatorUtilities.CreateInstance<ModifyYeastPair>(_serviceProvider, _dbContext, _mapper);
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

        /// <inheritdoc cref="IFactory.CreateNewYeast"/>>
        public YeastDto CreateNewYeast(int id, CodeDto brand, CodeDto style, string trademark, int? tempMin, int? tempMax, double? alcohol, string note)
        {
            var dto = new YeastDto
            {
                Id = id,
                Brand = brand,
                Style = style,
                Trademark = trademark,
                TempMin = tempMin,
                TempMax = tempMax,
                Alcohol = alcohol,
                Note = note
            };
            return dto;
        }


        /// <summary>
        /// Instance of Create Yeast Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateVarietiesQuery"/>>
        public IQuery<YeastDto> CreateYeastsQuery()
        {
            return ActivatorUtilities.CreateInstance<GetYeasts>(_serviceProvider, _dbContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Yeast Pair Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateYeastPairQuery"/>>
        public IQuery<Dto.YeastPairDto> CreateYeastPairQuery()
        {
            return ActivatorUtilities.CreateInstance<GetYeastPairs>(_serviceProvider, _dbContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Brands Query
        /// </summary>
        /// <returns></returns>
        public IQuery<ICodeDto> CreateBrandsQuery()
        {
            return ActivatorUtilities.CreateInstance<GetBrands>(_serviceProvider, _dbContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Styles Query
        /// </summary>
        /// <returns></returns>
        public IQuery<ICodeDto> CreateStylesQuery()
        {
            return ActivatorUtilities.CreateInstance<GetStyles>(_serviceProvider, _dbContext, _mapper);
        }
    }
}
