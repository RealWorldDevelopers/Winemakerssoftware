using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using WMS.Business.Common;
using WMS.Business.Image.Commands;
using WMS.Business.Image.Dto;
using WMS.Business.Image.Queries;
using WMS.Data.SQL;

namespace WMS.Business.Image
{
    /// <summary>
    /// Instance of <see cref="IFactory"/> Factory
    /// </summary>
    /// <inheritdoc cref="IFactory"/>>
    public class Factory : IFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WMSContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Image Factory Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public Factory(IServiceProvider serviceProvider, WMSContext dbContext, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Instance of Create Image Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateRatingsQuery"/>>
        public IQuery<ImageDto> CreateImagesQuery()
        {            
            return ActivatorUtilities.CreateInstance<GetImages>(_serviceProvider, _dbContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Image Command 
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateRatingsQuery"/>>
        public ICommand<ImageDto> CreateImagesCommand()
        {
            return ActivatorUtilities.CreateInstance<ModifyImage>(_serviceProvider, _dbContext, _mapper);
        }


    }
}

