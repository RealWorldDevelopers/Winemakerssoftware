using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Data;

namespace WMS.Business.MaloCulture.Queries
{
   /// <inheritdoc cref = "IQuery{T}" />
   public class GetBrands : IQuery<ICode>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Brands Query Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>       
      public GetBrands(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Query all Brands in SQL DB
      /// </summary>
      /// <returns><see cref="List{ICode}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<ICode> Execute()
      {
         var brands = _dbContext.MaloCultureBrand.ToList();
         var list = _mapper.Map<List<ICode>>(brands);
         return list;
      }

      /// <summary>
      /// Query a Brand in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="ICode"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public ICode Execute(int id)
      {
         var brand = _dbContext.MaloCultureBrand.FirstOrDefault(p => p.Id == id);
         var dto = _mapper.Map<ICode>(brand);
         return dto;
      }

      /// <summary>
      /// Asynchronously query all Brands in SQL DB
      /// </summary>
      /// <returns><see cref="Task{List{ICode}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<ICode>> ExecuteAsync()
      {
         var brands = await _dbContext.MaloCultureBrand.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<ICode>>(brands);
         return list;
      }

      /// <summary>
      /// Asynchronously query a Brand in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="Task{ICode}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<ICode> ExecuteAsync(int id)
      {
         var brand = await _dbContext.MaloCultureBrand
            .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
         var dto = _mapper.Map<ICode>(brand);
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

      public List<ICode> ExecuteByUser(string userId)
      {
         throw new System.NotImplementedException();
      }

      public Task<List<ICode>> ExecuteByUserAsync(string userId)
      {
         throw new System.NotImplementedException();
      }
   }
}
