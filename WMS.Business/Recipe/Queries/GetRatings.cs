using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Recipe.Dto;
using WMS.Business.Common;
using WMS.Data;
using AutoMapper.QueryableExtensions;

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
      /// Query all Ratings in SQL DB
      /// </summary>
      /// <returns>Ratings as <see cref="List{Rating}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<RatingDto> Execute()
      {
         var ratings = _dbContext.Ratings.ToList();
         var list = _mapper.Map<List<RatingDto>>(ratings);
         return list;
      }

      /// <summary>
      /// Query a specific Rating in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Rating as <see cref="RatingDto"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public RatingDto Execute(int id)
      {
         var rating = _dbContext.Ratings
            .FirstOrDefault(r => r.Id == id);
         var dto = _mapper.Map<RatingDto>(rating);
         return dto;
      }

      /// <summary>
      /// Asynchronously query all Ratings in SQL DB
      /// </summary>
      /// <returns>Ratings as <see cref="Task{List{RatingDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<RatingDto>> ExecuteAsync()
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
      public async Task<RatingDto> ExecuteAsync(int id)
      {
         var rating = await _dbContext.Ratings
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);
         var dto = _mapper.Map<RatingDto>(rating);
         return dto;
      }

      public List<RatingDto> ExecuteByFK(int fk)
      {
         throw new System.NotImplementedException();
      }

      public Task<List<RatingDto>> ExecuteByFKAsync(int fk)
      {
         throw new System.NotImplementedException();
      }
   }
}
