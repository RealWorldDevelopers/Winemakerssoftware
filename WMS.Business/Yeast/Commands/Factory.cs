
using AutoMapper;
using WMS.Business.Shared;
using WMS.Data;

namespace WMS.Business.Yeast.Commands
{
    public class Factory : IFactory
    {
        private readonly WMSContext _yeastContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Yeast Factory Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public Factory(WMSContext dbContext, IMapper mapper)
        {
            _yeastContext = dbContext;
            _mapper = mapper;
        }


        /// <summary>
        /// Instance of Create Yeast Command
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateYeastsCommand"/>>
        public ICommand<Dto.Yeast> CreateYeastsCommand()
        {
            return new ModifyYeast(_yeastContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Yeast Pair Command
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateYeastPairCommand"/>>
        public ICommand<Dto.YeastPair> CreateYeastPairCommand()
        {
            return new ModifyYeastPair(_yeastContext, _mapper);
        }

    }
}
