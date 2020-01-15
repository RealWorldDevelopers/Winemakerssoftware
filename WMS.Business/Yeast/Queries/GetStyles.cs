
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Data;

namespace WMS.Business.Yeast.Queries
{
   /// <inheritdoc cref="IQuery{T}"/>
   public class GetStyles : IQuery<ICode>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Yeasts Query Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public GetStyles(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Query all Styles in SQL DB
      /// </summary>
      /// <returns><see cref="List{ICode}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<ICode> Execute()
      {
         var dtoList = _dbContext.YeastStyle
            .ProjectTo<ICode>(_mapper.ConfigurationProvider).ToList();
         
         return dtoList;
      }

      /// <summary>
      /// Query a Style in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="ICode"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public ICode Execute(int id)
      {
         var dto = _dbContext.YeastStyle
            .ProjectTo<ICode>(_mapper.ConfigurationProvider)
            .FirstOrDefault(p => p.Id == id);

         return dto;
      }

      /// <summary>
      /// Asynchronously query all Styles in SQL DB
      /// </summary>
      /// <returns><see cref="Task{List{ICode}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<ICode>> ExecuteAsync()
      {
         var dtoList = await _dbContext.YeastStyle
            .ProjectTo<ICode>(_mapper.ConfigurationProvider)
            .ToListAsync().ConfigureAwait(false);

         return dtoList;
      }

      /// <summary>
      /// Asynchronously query a Yeast in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="Task{ICode}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<ICode> ExecuteAsync(int id)
      {
         var dto = await _dbContext.YeastStyle
            .ProjectTo<ICode>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);

         return dto;
      }

   }
}
