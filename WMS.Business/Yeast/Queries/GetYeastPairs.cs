
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Shared;
using WMS.Business.Yeast.Dto;
using WMS.Data;

namespace WMS.Business.Yeast.Queries
{
    /// <summary>
    /// Yeast Query Instance
    /// </summary>
    /// <inheritdoc cref="IQuery{T}"/>
    public class GetYeastPairs : IQuery<YeastPair>
    {

        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        /// <summary>
        /// Yeasts Query Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public GetYeastPairs(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Query all Yeasts in SQL DB
        /// </summary>
        /// <returns><see cref="List{YeastPair}"/></returns>
        /// <inheritdoc cref="IQuery{T}.Execute"/>
        public List<YeastPair> Execute()
        {
            var pairs = _dbContext.YeastPair.ToList();
            var list = _mapper.Map<List<Dto.YeastPair>>(pairs);
            return list;
        }

        /// <summary>
        /// Query a Yeast in SQL DB by primary key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns><see cref="YeastPair"/></returns>
        /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
        public YeastPair Execute(int id)
        {
            var yeast =  _dbContext.YeastPair
               .Where(p => p.Id == id)
               .FirstOrDefault();

            var dto = _mapper.Map<YeastPair>(yeast);

            return dto;
        }

        /// <summary>
        /// Asynchronously query all Yeasts in SQL DB
        /// </summary>
        /// <returns><see cref="Task{List{Dto.YeastPair}}"/></returns>
        /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
        public async Task<List<YeastPair>> ExecuteAsync()
        {
            var pairs =  await _dbContext.YeastPair.ToListAsync();  
            var list = _mapper.Map<List<Dto.YeastPair>>(pairs);
            return list;
        }

        /// <summary>
        /// Asynchronously query a Yeast in SQL DB by primary key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns><see cref="Task{YeastPair}"/></returns>
        /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
        public async Task<YeastPair> ExecuteAsync(int id)
        {
            var yeast = await _dbContext.YeastPair
               .Where(p => p.Id == id)
               .FirstOrDefaultAsync();

            var dto = _mapper.Map<Dto.YeastPair>(yeast);

            return dto;
        }             

        
    }
}
