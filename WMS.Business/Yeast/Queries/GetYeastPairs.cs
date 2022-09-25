using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;
using WMS.Data.SQL;

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
      /// Asynchronously query all Yeasts in SQL DB
      /// </summary>
      /// <returns><see cref="Task{List{YeastPairDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<YeastPairDto>> Execute()
      {
         var pairs = await _dbContext.YeastPairs.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<YeastPairDto>>(pairs);
         return list;
      }

      /// <summary>
      /// Asynchronously query a Yeast in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="Task{YeastPair}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<YeastPairDto> Execute(int id)
      {
         var yeast = await _dbContext.YeastPairs
            .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
         var dto = _mapper.Map<YeastPairDto>(yeast);
         return dto;
      }

        public Task<List<YeastPairDto>> Execute(int start, int length)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<YeastPairDto>> ExecuteByFK(int fk)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<YeastPairDto>> ExecuteByUser(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
