using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Data;

namespace WMS.Business.Recipe.Queries
{
   /// <summary>
   /// Variety Query Instance
   /// </summary>
   /// <inheritdoc cref="IQuery{T}"/>
   public class GetVarieties : IQuery<ICode>
   {

      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Variety Query Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public GetVarieties(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Query all Varieties in SQL DB
      /// </summary>
      /// <returns><see cref="List{ICode}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<ICode> Execute()
      {
         var varieties = _dbContext.Varieties.ToList();
         var list = _mapper.Map<List<ICode>>(varieties);
         return list;
      }

      /// <summary>
      /// Query a specific Variety in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="ICode"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public ICode Execute(int id)
      {
         var variety = _dbContext.Varieties.FirstOrDefault(r => r.Id == id);
         var dto = _mapper.Map<ICode>(variety);
         return dto;
      }

      /// <summary>
      /// Asynchronously query all Varieties in SQL DB
      /// </summary>
      /// <returns><see cref="Task{List{ICode}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<ICode>> ExecuteAsync()
      {
         var varieties = await _dbContext.Varieties.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<ICode>>(varieties);
         return list;
      }

      /// <summary>
      /// Asynchronously query a specific Variety in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="Task{ICode}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<ICode> ExecuteAsync(int id)
      {
         var variety = await _dbContext.Varieties
            .FirstOrDefaultAsync(r => r.Id == id).ConfigureAwait(false);
         var dto = _mapper.Map<ICode>(variety);
         return dto;
      }

      public List<ICode> ExecuteByFK(int fk)
      {
         throw new System.NotImplementedException();
      }

      public Task<List<ICode>> ExecuteByFKAsync(int fk)
      {
         throw new System.NotImplementedException();
      }
   }
}
