using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using WMS.Business.Common;
using WMS.Business.MaloCulture.Commands;
using WMS.Business.MaloCulture.Dto;
using WMS.Business.MaloCulture.Queries;
using WMS.Data.SQL;

namespace WMS.Business.MaloCulture
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

        /// <inheritdoc cref="IFactory.CreateMaloCulturesCommand"/>>
        public ICommand<MaloCultureDto> CreateMaloCulturesCommand()
        {
            return ActivatorUtilities.CreateInstance<ModifyMaloCulture>(_serviceProvider, _dbContext, _mapper);
        }

        /// <summary>
        /// Instance of Create MaloCulture Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateVarietiesQuery"/>>
        public IQuery<MaloCultureDto> CreateMaloCulturesQuery()
        {
            return ActivatorUtilities.CreateInstance<GetMaloCultures>(_serviceProvider, _dbContext, _mapper);
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
