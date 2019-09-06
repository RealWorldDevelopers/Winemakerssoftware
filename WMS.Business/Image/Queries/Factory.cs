using AutoMapper;
using WMS.Business.Shared;
using WMS.Data;

namespace WMS.Business.Image.Queries
{
    /// <summary>
    /// Instance of <see cref="Commands.ICommand{T}"/> Factory
    /// </summary>
    /// <inheritdoc cref="IFactory"/>>
    public class Factory : IFactory
    {
        private readonly WMSContext _imageContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Image Factory Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public Factory(WMSContext dbContext, IMapper mapper)
        {
            _imageContext = dbContext;
            _mapper = mapper;
        }    
        
        /// <summary>
        /// Instance of Create Rating Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateRatingsQuery"/>>
        public IQuery<Dto.Image> CreateImageQuery()
        {
            return new GetImages(_imageContext, _mapper);
        }

    }
}

