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
         var dtoList = _dbContext.Categories
            .ProjectTo<ICode>(_mapper.ConfigurationProvider).ToList();
         
         return dtoList;
      }

      /// <summary>
      /// Query a specific Category in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Category as <see cref="ICode"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public ICode Execute(int id)
      {
         var dto = _dbContext.Categories
            .ProjectTo<ICode>(_mapper.ConfigurationProvider)
            .FirstOrDefault(r => r.Id == id);

         return dto;
      }

      /// <summary>
      /// Asynchronously query all Categories in SQL DB
      /// </summary>
      /// <returns>Categories as <see cref="Task{List{ICode}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<ICode>> ExecuteAsync()
      {
         var dtoList = await _dbContext.Categories
            .ProjectTo<ICode>(_mapper.ConfigurationProvider)
            .ToListAsync().ConfigureAwait(false);

         return dtoList;
      }

      /// <summary>
      /// Asynchronously query a specific Category in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Category as <see cref="Task{ICode}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<ICode> ExecuteAsync(int id)
      {
         var dto = await _dbContext.Categories
            .ProjectTo<ICode>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);

         return dto;
      }

   }
}
