using AutoMapper;
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
      /// Styles Query Constructor
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
         var styles = _dbContext.YeastStyle.ToList();
         var list = _mapper.Map<List<ICode>>(styles);
         return list;
      }

      /// <summary>
      /// Query a Style in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="ICode"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public ICode Execute(int id)
      {
         var style = _dbContext.YeastStyle.FirstOrDefault(p => p.Id == id);
         var dto = _mapper.Map<ICode>(style);
         return dto;
      }

      /// <summary>
      /// Asynchronously query all Styles in SQL DB
      /// </summary>
      /// <returns><see cref="Task{List{ICode}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<ICode>> ExecuteAsync()
      {
         var styles = await _dbContext.YeastStyle.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<ICode>>(styles);
         return list;
      }

      /// <summary>
      /// Asynchronously query a Style in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="Task{ICode}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<ICode> ExecuteAsync(int id)
      {
         var style = await _dbContext.YeastStyle
            .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
         var dto = _mapper.Map<ICode>(style);
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
