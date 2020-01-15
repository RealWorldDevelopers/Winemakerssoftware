
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
   public class GetYeasts : IQuery<Dto.YeastDto>
   {

      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Yeasts Query Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public GetYeasts(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Query all Yeasts in SQL DB
      /// </summary>
      /// <returns><see cref="List{YeastDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<YeastDto> Execute()
      {
         var dtoList = _dbContext.Yeasts
            .ProjectTo<YeastDto>(_mapper.ConfigurationProvider).ToList();
         var brands = _dbContext.YeastBrand.ToList();
         var styles = _dbContext.YeastStyle.ToList();

         foreach (var item in dtoList)
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

         return dtoList;
      }

      /// <summary>
      /// Query a Yeast in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="YeastDto"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public YeastDto Execute(int id)
      {
         var dto = _dbContext.Yeasts
            .ProjectTo<YeastDto>(_mapper.ConfigurationProvider)
            .FirstOrDefault(y => y.Id == id);

         return dto;
      }

      /// <summary>
      /// Asynchronously query all Yeasts in SQL DB
      /// </summary>
      /// <returns><see cref="Task{List{YeastDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<YeastDto>> ExecuteAsync()
      {
         // using TPL to parallel call gets
         List<Task> tasks = new List<Task>();
         var t1 = Task.Run(async () =>
            await _dbContext.Yeasts
               .ProjectTo<YeastDto>(_mapper.ConfigurationProvider)
               .ToListAsync().ConfigureAwait(false));
         tasks.Add(t1);
         var list = await t1.ConfigureAwait(false);

         var t2 = Task.Run(async () => await _dbContext.YeastBrand.ToListAsync().ConfigureAwait(false));
         tasks.Add(t2);
         var brands = await t2.ConfigureAwait(false);

         var t3 = Task.Run(async () => await _dbContext.YeastStyle.ToListAsync().ConfigureAwait(false));
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
      /// Asynchronously query a Yeast in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="Task{YeastDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<YeastDto> ExecuteAsync(int id)
      {
         var dto = await _dbContext.Yeasts
            .ProjectTo<YeastDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(y => y.Id == id).ConfigureAwait(false);

         return dto;
      }


   }
}
