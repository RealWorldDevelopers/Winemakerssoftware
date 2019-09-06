using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Recipe.Dto;
using WMS.Business.Shared;
using WMS.Data;

namespace WMS.Business.Recipe.Queries
{
    /// <summary>
    /// Rating Query Instance
    /// </summary>
    /// <inheritdoc cref="IQuery{T}"/>
    public class GetRatings : IQuery<Rating>
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        /// <summary>
        /// Rating Query Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public GetRatings(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Query all Ratings in SQL DB
        /// </summary>
        /// <returns>Ratings as <see cref="List{Rating}"/></returns>
        /// <inheritdoc cref="IQuery{T}.Execute"/>
        public List<Rating> Execute()
        {
            var ratings = _dbContext.Ratings.ToList();
            var list = _mapper.Map<List<Rating>>(ratings);
            return list;
        }

        /// <summary>
        /// Query a specific Rating in SQL DB by primary key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns>Rating as <see cref="Rating"/></returns>
        /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
        public Rating Execute(int id)
        {
            var ratings =  _dbContext.Ratings
               .Where(r => r.Id == id)
               .FirstOrDefault();

            var dto = _mapper.Map<Rating>(ratings);

            return dto;
        }

        /// <summary>
        /// Asynchronously query all Ratings in SQL DB
        /// </summary>
        /// <returns>Ratings as <see cref="Task{List{Rating}}"/></returns>
        /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
        public async Task<List<Rating>> ExecuteAsync()
        {
            var ratings = await _dbContext.Ratings.ToListAsync();
            var list = _mapper.Map<List<Rating>>(ratings);
            return list;
        }

        /// <summary>
        /// Asynchronously query a specific Rating in SQL DB by primary key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns>Rating as <see cref="Task{Rating}"/></returns>
        /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
        public async Task<Rating> ExecuteAsync(int id)
        {
            var ratings = await _dbContext.Ratings
               .Where(r => r.Id == id)
               .FirstOrDefaultAsync();

            var dto = _mapper.Map<Rating>(ratings);

            return dto;
        }

    }
}
