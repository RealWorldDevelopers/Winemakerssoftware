using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Business.Recipe.Dto;
using WMS.Business.Common;
using WMS.Data.SQL;

namespace WMS.Business.Recipe.Queries
{
   /// <summary>
   /// Rating Query Instance
   /// </summary>
   /// <inheritdoc cref="IQuery{T}"/>
   public class GetRatings : IQuery<RatingDto>
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
      /// Asynchronously query all Ratings in SQL DB
      /// </summary>
      /// <returns>Ratings as <see cref="Task{List{RatingDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<RatingDto>> Execute()
      {
         var ratings = await _dbContext.Ratings.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<RatingDto>>(ratings);
         return list;
      }

      /// <summary>
      /// Asynchronously query a specific Rating in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Rating as <see cref="Task{Rating}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<RatingDto> Execute(int id)
      {
         var rating = await _dbContext.Ratings
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);
         var dto = _mapper.Map<RatingDto>(rating);
         return dto;
      }

        public Task<List<RatingDto>> Execute(int start, int length)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<RatingDto>> ExecuteByFK(int fk)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<RatingDto>> ExecuteByUser(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
