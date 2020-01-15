
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;
using WMS.Data;

namespace WMS.Business.Yeast.Queries
{
   /// <summary>
   /// Yeast Query Instance
   /// </summary>
   /// <inheritdoc cref="IQuery{T}"/>
   public class GetYeastPairs : IQuery<YeastPairDto>
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
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<YeastPairDto> Execute()
      {
         var dtoList = _dbContext.YeastPair
            .ProjectTo<YeastPairDto>(_mapper.ConfigurationProvider).ToList();
         
         return dtoList;
      }

      /// <summary>
      /// Query a Yeast in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="YeastPairDto"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public YeastPairDto Execute(int id)
      {
         var dto = _dbContext.YeastPair
            .ProjectTo<YeastPairDto>(_mapper.ConfigurationProvider)
            .FirstOrDefault(p => p.Id == id);

         return dto;
      }

      /// <summary>
      /// Asynchronously query all Yeasts in SQL DB
      /// </summary>
      /// <returns><see cref="Task{List{Dto.YeastPairDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<YeastPairDto>> ExecuteAsync()
      {
         var dtoList = await _dbContext.YeastPair
            .ProjectTo<YeastPairDto>(_mapper.ConfigurationProvider)
            .ToListAsync().ConfigureAwait(false);
         
         return dtoList;
      }

      /// <summary>
      /// Asynchronously query a Yeast in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="Task{YeastPair}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<YeastPairDto> ExecuteAsync(int id)
      {
         var dto = await _dbContext.YeastPair
            .ProjectTo<YeastPairDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);

         return dto;
      }


   }
}
