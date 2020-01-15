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
         var dtoList = _dbContext.Ratings
            .ProjectTo<RatingDto>(_mapper.ConfigurationProvider).ToList();

         return dtoList;
      }

      /// <summary>
      /// Query a specific Rating in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Rating as <see cref="RatingDto"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public RatingDto Execute(int id)
      {
         var dto = _dbContext.Ratings
            .ProjectTo<RatingDto>(_mapper.ConfigurationProvider)
            .FirstOrDefault(r => r.Id == id);

         return dto;
      }

      /// <summary>
      /// Asynchronously query all Ratings in SQL DB
      /// </summary>
      /// <returns>Ratings as <see cref="Task{List{RatingDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<RatingDto>> ExecuteAsync()
      {
         var dtoList = await _dbContext.Ratings
            .ProjectTo<RatingDto>(_mapper.ConfigurationProvider)
            .ToListAsync().ConfigureAwait(false);

         return dtoList;
      }

      /// <summary>
      /// Asynchronously query a specific Rating in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Rating as <see cref="Task{Rating}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<RatingDto> ExecuteAsync(int id)
      {
         var dto = await _dbContext.Ratings
            .ProjectTo<RatingDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);

         return dto;
      }

   }
}
