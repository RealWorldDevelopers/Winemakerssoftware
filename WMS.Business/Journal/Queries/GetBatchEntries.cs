using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Data.SQL;

namespace WMS.Business.Journal.Queries
{
   public class GetBatchEntries : IQuery<BatchEntryDto>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Batch Entry Query Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public GetBatchEntries(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }


      /// <summary>
      /// Asynchronously query all Entries in SQL DB
      /// </summary>
      /// <returns>Entries as <see cref="Task{List{BatchEntryDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<BatchEntryDto>> Execute()
      {
         var entries = await _dbContext.BatchEntries.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<BatchEntryDto>>(entries);

         var sugarUoms = await _dbContext.UnitsOfMeasures.Where(uom => uom.Subset == Common.Subsets.Sugar.Standard).ToListAsync().ConfigureAwait(false);
         var sugarUomDtoList = _mapper.Map<List<UnitOfMeasureDto>>(sugarUoms);

         var tempUoms = await _dbContext.UnitsOfMeasures.Where(uom => uom.Subset == Common.Subsets.Temperature.Standard).ToListAsync().ConfigureAwait(false);
         var tempUomDtoList = _mapper.Map<List<UnitOfMeasureDto>>(tempUoms);

         foreach (var e in list)
         {
            if (e.SugarUom != null)
               e.SugarUom = sugarUomDtoList.FirstOrDefault(s => s.Id == e.SugarUom.Id);

            if (e.TempUom != null)
               e.TempUom = tempUomDtoList.FirstOrDefault(s => s.Id == e.TempUom.Id);
         }

         return list;
      }

      /// <summary>
      /// Asynchronously query a specific Entry in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Entry as <see cref="Task{BatchEntryDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<BatchEntryDto> Execute(int id)
      {
         var entry = await _dbContext.BatchEntries
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);

         var dto = _mapper.Map<BatchEntryDto>(entry);

         if (dto != null)
         {
            var sugarUoms = await _dbContext.UnitsOfMeasures.Where(uom => uom.Subset == Common.Subsets.Sugar.Standard).ToListAsync().ConfigureAwait(false);
            var sugarUomDtoList = _mapper.Map<List<UnitOfMeasureDto>>(sugarUoms);

            var tempUoms = await _dbContext.UnitsOfMeasures.Where(uom => uom.Subset == Common.Subsets.Temperature.Standard).ToListAsync().ConfigureAwait(false);
            var tempUomDtoList = _mapper.Map<List<UnitOfMeasureDto>>(tempUoms);

            if (dto.SugarUom != null)
               dto.SugarUom = sugarUomDtoList.FirstOrDefault(s => s.Id == dto.SugarUom.Id);

            if (dto.TempUom != null)
               dto.TempUom = tempUomDtoList.FirstOrDefault(s => s.Id == dto.TempUom.Id);
         }

         return dto ?? new BatchEntryDto();
      }

      public Task<List<BatchEntryDto>> Execute(int start, int length)
      {
         throw new System.NotImplementedException();
      }

      public async Task<List<BatchEntryDto>> ExecuteByFK(int fk)
      {
         var entries = await _dbContext.BatchEntries.Where(e => e.BatchId == fk).ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<BatchEntryDto>>(entries);

         var sugarUoms = await _dbContext.UnitsOfMeasures.Where(uom => uom.Subset == Common.Subsets.Sugar.Standard).ToListAsync().ConfigureAwait(false);
         var sugarUomDtoList = _mapper.Map<List<UnitOfMeasureDto>>(sugarUoms);

         var tempUoms = await _dbContext.UnitsOfMeasures.Where(uom => uom.Subset == Common.Subsets.Temperature.Standard).ToListAsync().ConfigureAwait(false);
         var tempUomDtoList = _mapper.Map<List<UnitOfMeasureDto>>(tempUoms);

         foreach (var e in list)
         {
            if (e.SugarUom != null)
               e.SugarUom = sugarUomDtoList.FirstOrDefault(s => s.Id == e.SugarUom.Id);

            if (e.TempUom != null)
               e.TempUom = tempUomDtoList.FirstOrDefault(s => s.Id == e.TempUom.Id);
         }

         return list;
      }

      public Task<List<BatchEntryDto>> ExecuteByUser(string userId)
      {
         throw new System.NotImplementedException();
      }
   }

}
