using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Data;

namespace WMS.Business.Journal.Queries
{
   public class GetBatchSugarUOM : IQuery<IUnitOfMeasure>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Units of Measure Query Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public GetBatchSugarUOM(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Query all Units of Measure in SQL DB
      /// </summary>
      /// <returns>Units of Measure as <see cref="List{IUnitOfMeasure}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<IUnitOfMeasure> Execute()
      {
         var UnitOfMeasure = _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Sugar.Batch).ToList();
         var list = _mapper.Map<List<IUnitOfMeasure>>(UnitOfMeasure);
         return list;
      }

      /// <summary>
      /// Query a specific Unit of Measure in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Unit of Measure as <see cref="IUnitOfMeasure"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public IUnitOfMeasure Execute(int id)
      {
         var category = _dbContext.UnitsOfMeasure
            .Where(uom => uom.Subset == Common.Subsets.Sugar.Batch)
            .FirstOrDefault(r => r.Id == id);
         var dto = _mapper.Map<IUnitOfMeasure>(category);
         return dto;
      }

      /// <summary>
      /// Asynchronously query all Units of Measure in SQL DB
      /// </summary>
      /// <returns>Units of Measure as <see cref="Task{List{IUnitOfMeasure}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<IUnitOfMeasure>> ExecuteAsync()
      {
         var UnitOfMeasure = await _dbContext.UnitsOfMeasure
            .Where(uom => uom.Subset == Common.Subsets.Sugar.Batch)
            .ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<IUnitOfMeasure>>(UnitOfMeasure);
         return list;
      }

      /// <summary>
      /// Asynchronously query a specific Unit of Measure in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Unit of Measure as <see cref="Task{IUnitOfMeasure}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<IUnitOfMeasure> ExecuteAsync(int id)
      {
         var category = await _dbContext.UnitsOfMeasure
            .Where(uom => uom.Subset == Common.Subsets.Sugar.Batch)
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);
         var dto = _mapper.Map<IUnitOfMeasure>(category);
         return dto;
      }

      public List<IUnitOfMeasure> ExecuteByFK(int fk)
      {
         throw new System.NotImplementedException();
      }

      public Task<List<IUnitOfMeasure>> ExecuteByFKAsync(int fk)
      {
         throw new System.NotImplementedException();
      }

      public List<IUnitOfMeasure> ExecuteByUser(string userId)
      {
         throw new System.NotImplementedException();
      }

      public Task<List<IUnitOfMeasure>> ExecuteByUserAsync(string userId)
      {
         throw new System.NotImplementedException();
      }
   }

}
