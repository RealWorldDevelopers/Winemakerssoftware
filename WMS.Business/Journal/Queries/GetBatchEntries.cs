using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Data;

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
      /// Query all Entries in SQL DB
      /// </summary>
      /// <returns>Entries as <see cref="List{BatchEntryDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<BatchEntryDto> Execute()
      {
         var entries = _dbContext.BatchEntries.ToList();
         var list = _mapper.Map<List<BatchEntryDto>>(entries);

         var sugarUoms = _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Sugar.Batch).ToList();
         var sugarUomDtoList = _mapper.Map<List<IUnitOfMeasure>>(sugarUoms);

         var tempUoms = _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Temperature.Batch).ToList();
         var tempUomDtoList = _mapper.Map<List<IUnitOfMeasure>>(tempUoms);

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
      /// Query a specific Entry in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Entry as <see cref="BatchEntryDto"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public BatchEntryDto Execute(int id)
      {
         var entry = _dbContext.BatchEntries
            .FirstOrDefault(r => r.Id == id);
         var dto = _mapper.Map<BatchEntryDto>(entry);

         if (dto != null)
         {
            var sugarUoms = _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Sugar.Batch).ToList();
            var sugarUomDtoList = _mapper.Map<List<IUnitOfMeasure>>(sugarUoms);

            var tempUoms = _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Temperature.Batch).ToList();
            var tempUomDtoList = _mapper.Map<List<IUnitOfMeasure>>(tempUoms);

            if (dto.SugarUom != null)
               dto.SugarUom = sugarUomDtoList.FirstOrDefault(s => s.Id == dto.SugarUom.Id);

            if (dto.TempUom != null)
               dto.TempUom = tempUomDtoList.FirstOrDefault(s => s.Id == dto.TempUom.Id);
         }
         return dto;
      }

      /// <summary>
      /// Query a specific Entry in SQL DB by a foreign key
      /// </summary>
      /// <param name="fk">Foreign Key as <see cref="int"/></param>
      /// <returns>Entry as <see cref="BatchEntryDto"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public List<BatchEntryDto> ExecuteByFK(int fk)
      {
         var entries = _dbContext.BatchEntries.Where(e => e.BatchId == fk).ToList();
         var list = _mapper.Map<List<BatchEntryDto>>(entries);

         var sugarUoms = _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Sugar.Batch).ToList();
         var sugarUomDtoList = _mapper.Map<List<IUnitOfMeasure>>(sugarUoms);

         var tempUoms = _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Temperature.Batch).ToList();
         var tempUomDtoList = _mapper.Map<List<IUnitOfMeasure>>(tempUoms);

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
      /// Asynchronously query all Entries in SQL DB
      /// </summary>
      /// <returns>Entries as <see cref="Task{List{BatchEntryDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<BatchEntryDto>> ExecuteAsync()
      {
         var entries = await _dbContext.BatchEntries.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<BatchEntryDto>>(entries);

         var sugarUoms = await _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Sugar.Batch).ToListAsync().ConfigureAwait(false);
         var sugarUomDtoList = _mapper.Map<List<IUnitOfMeasure>>(sugarUoms);

         var tempUoms = await _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Temperature.Batch).ToListAsync().ConfigureAwait(false);
         var tempUomDtoList = _mapper.Map<List<IUnitOfMeasure>>(tempUoms);

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
      public async Task<BatchEntryDto> ExecuteAsync(int id)
      {
         var entry = await _dbContext.BatchEntries
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);

         var dto = _mapper.Map<BatchEntryDto>(entry);

         if (dto != null)
         {
            var sugarUoms = await _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Sugar.Batch).ToListAsync().ConfigureAwait(false);
            var sugarUomDtoList = _mapper.Map<List<IUnitOfMeasure>>(sugarUoms);

            var tempUoms = await _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Temperature.Batch).ToListAsync().ConfigureAwait(false);
            var tempUomDtoList = _mapper.Map<List<IUnitOfMeasure>>(tempUoms);

            if (dto.SugarUom != null)
               dto.SugarUom = sugarUomDtoList.FirstOrDefault(s => s.Id == dto.SugarUom.Id);

            if (dto.TempUom != null)
               dto.TempUom = tempUomDtoList.FirstOrDefault(s => s.Id == dto.TempUom.Id);
         }

         return dto;
      }

      /// <summary>
      /// Asynchronously query a specific Entry in SQL DB by foreign key
      /// </summary>
      /// <param name="fk">Foreign Key as <see cref="int"/></param>
      /// <returns>Entry as <see cref="Task{BatchEntryDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<List<BatchEntryDto>> ExecuteByFKAsync(int fk)
      {
         var entries = await _dbContext.BatchEntries.Where(e => e.BatchId == fk).ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<BatchEntryDto>>(entries);

         var sugarUoms = await _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Sugar.Batch).ToListAsync().ConfigureAwait(false);
         var sugarUomDtoList = _mapper.Map<List<IUnitOfMeasure>>(sugarUoms);

         var tempUoms = await _dbContext.UnitsOfMeasure.Where(uom => uom.Subset == Common.Subsets.Temperature.Batch).ToListAsync().ConfigureAwait(false);
         var tempUomDtoList = _mapper.Map<List<IUnitOfMeasure>>(tempUoms);

         foreach (var e in list)
         {
            if (e.SugarUom != null)
               e.SugarUom = sugarUomDtoList.FirstOrDefault(s => s.Id == e.SugarUom.Id);

            if (e.TempUom != null)
               e.TempUom = tempUomDtoList.FirstOrDefault(s => s.Id == e.TempUom.Id);
         }

         return list;
      }

   }

}
