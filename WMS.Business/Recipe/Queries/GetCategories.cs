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
   /// Categories Query Instance
   /// </summary>
   /// <inheritdoc cref="IQuery{T}"/>
   public class GetCategories : IQuery<ICode>
   {

      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Category Query Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public GetCategories(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Query all Categories in SQL DB
      /// </summary>
      /// <returns>Categories as <see cref="List{ICode}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<ICode> Execute()
      {
         var categories = _dbContext.Categories.ToList();
         var list = _mapper.Map<List<ICode>>(categories);
         return list;
      }

      /// <summary>
      /// Query a specific Category in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Category as <see cref="ICode"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public ICode Execute(int id)
      {
         var category = _dbContext.Categories
            .FirstOrDefault(r => r.Id == id);
         var dto = _mapper.Map<ICode>(category);
         return dto;
      }

      /// <summary>
      /// Asynchronously query all Categories in SQL DB
      /// </summary>
      /// <returns>Categories as <see cref="Task{List{ICode}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<ICode>> ExecuteAsync()
      {
         var categories = await _dbContext.Categories.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<ICode>>(categories);
         return list;
      }

      /// <summary>
      /// Asynchronously query a specific Category in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Category as <see cref="Task{ICode}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<ICode> ExecuteAsync(int id)
      {
         var category = await _dbContext.Categories
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);
         var dto = _mapper.Map<ICode>(category);
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
