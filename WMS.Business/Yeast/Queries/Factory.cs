
using AutoMapper;
using WMS.Business.Common;
using WMS.Data;

namespace WMS.Business.Yeast.Queries
{
    public class Factory : IFactory
    {
        private readonly WMSContext _yeastContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Query Factory Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">Auto Mapper Instance as <see cref="IMapper"/></param>
        public Factory(WMSContext dbContext, IMapper mapper)
        {
            _yeastContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Instance of Create Yeast Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateVarietiesQuery"/>>
        public IQuery<Dto.YeastDto> CreateYeastsQuery()
        {
            return new GetYeasts(_yeastContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Yeast Pair Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateYeastPairQuery"/>>
        public IQuery<Dto.YeastPairDto> CreateYeastPairQuery()
        {
            return new GetYeastPairs(_yeastContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Brands Query
        /// </summary>
        /// <returns></returns>
        public IQuery<ICode> CreateBrandsQuery()
        {
            return new GetBrands(_yeastContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Styles Query
        /// </summary>
        /// <returns></returns>
        public IQuery<ICode> CreateStylesQuery()
        {
            return new GetStyles(_yeastContext, _mapper);
        }
    }
}
