using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.MaloCulture.Dto;
using WMS.Data.SQL;

namespace WMS.Business.MaloCulture.Queries
{
   /// <summary>
   /// MaloCulture Query Instance
   /// </summary>
   /// <inheritdoc cref="IQuery{T}"/>
   public class GetMaloCultures : IQuery<Dto.MaloCultureDto>
   {

      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// MaloCulturess Query Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public GetMaloCultures(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }  

      /// <summary>
      /// Asynchronously query all MaloCulturess in SQL DB
      /// </summary>
      /// <returns><see cref="Task{List{MaloCultureDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<MaloCultureDto>> Execute()
      {
         // using TPL to parallel call gets
         List<Task> tasks = new List<Task>();
         var t1 = Task.Run(async () => await _dbContext.MaloCultures.ToListAsync().ConfigureAwait(false));
         tasks.Add(t1);
         var list = _mapper.Map<List<MaloCultureDto>>(await t1.ConfigureAwait(false));

         var t2 = Task.Run(async () => await _dbContext.MaloCultureBrands.ToListAsync().ConfigureAwait(false));
         tasks.Add(t2);
         var brands = await t2.ConfigureAwait(false);

         var t3 = Task.Run(async () => await _dbContext.MaloCultureStyles.ToListAsync().ConfigureAwait(false));
         tasks.Add(t3);
         var styles = await t3.ConfigureAwait(false);

         Task.WaitAll(tasks.ToArray());

         foreach (var item in list)
         {
            if (item.Brand != null)
            {
               var code = brands.SingleOrDefault(a => a.Id == item.Brand.Id);
               item.Brand.Literal = code.Brand;
            }
            if (item.Style != null)
            {
               var code = styles.SingleOrDefault(a => a.Id == item.Style.Id);
               item.Style.Literal = code.Style;
            }
         }

         return list;


      }

      /// <summary>
      /// Asynchronously query a MaloCultures in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="Task{MaloCultureDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<MaloCultureDto> Execute(int id)
      {
         var MaloCultures = await _dbContext.MaloCultures
            .FirstOrDefaultAsync(y => y.Id == id).ConfigureAwait(false);
         var dto = _mapper.Map<MaloCultureDto>(MaloCultures);
         return dto;
      }

        public Task<List<MaloCultureDto>> Execute(int start, int length)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<MaloCultureDto>> ExecuteByFK(int fk)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<MaloCultureDto>> ExecuteByUser(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
