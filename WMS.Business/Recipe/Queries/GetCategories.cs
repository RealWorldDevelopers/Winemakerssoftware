using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Data.SQL;

namespace WMS.Business.Recipe.Queries
{
   /// <summary>
   /// Categories Query Instance
   /// </summary>
   /// <inheritdoc cref="IQuery{T}"/>
   public class GetCategories : IQuery<ICodeDto>
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
      /// Asynchronously query all Categories in SQL DB
      /// </summary>
      /// <returns>Categories as <see cref="Task{List{ICodeDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<ICodeDto>> Execute()
      {
         var categories = await _dbContext.Categories.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<ICodeDto>>(categories);
         return list;
      }

      /// <summary>
      /// Asynchronously query a specific Category in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Category as <see cref="Task{ICode}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<ICodeDto> Execute(int id)
      {
         var category = await _dbContext.Categories
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);
         var dto = _mapper.Map<ICodeDto>(category);
         return dto;
      }

        public Task<List<ICodeDto>> Execute(int start, int length)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ICodeDto>> ExecuteByFK(int fk)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ICodeDto>> ExecuteByUser(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
