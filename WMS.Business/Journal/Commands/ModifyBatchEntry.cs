using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Data;
using WMS.Data.Entities;

namespace WMS.Business.Journal.Commands
{
   public class ModifyBatchEntry : ICommand<BatchEntryDto>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Batch Entry Command Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public ModifyBatchEntry(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Add an <see cref="BatchEntryDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="BatchEntryDto"/></param>
      /// <returns><see cref="BatchEntryDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Add(BatchEntryDto)"/>
      public BatchEntryDto Add(BatchEntryDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _mapper.Map<BatchEntries>(dto);

         // add new batch entry
         _dbContext.BatchEntries.Add(entity);

         // Save changes in database
         _dbContext.SaveChanges();

         dto.Id = entity.Id;
         return dto;
      }

      /// <summary>
      /// Add an <see cref="BatchEntryDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="BatchEntryDto"/></param>
      /// <returns><see cref="Task{BatchEntryDto}"/></returns>
      /// <inheritdoc cref="ICommand{T}.AddAsync(BatchEntryDto)"/>
      public async Task<BatchEntryDto> AddAsync(BatchEntryDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _mapper.Map<BatchEntries>(dto);

         // add new batch
         await _dbContext.BatchEntries.AddAsync(entity).ConfigureAwait(false);

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         dto.Id = entity.Id;
         return dto;
      }

      /// <summary>
      /// Update a <see cref="BatchEntryDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="BatchEntryDto"/></param>
      /// <returns><see cref="BatchEntryDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Update(T)"/>
      public BatchEntryDto Update(BatchEntryDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _dbContext.BatchEntries.First(r => r.Id == dto.Id);

         entity.BatchId = dto.BatchId;
         entity.EntryDateTime = dto.EntryDateTime;
         entity.ActionDateTime = dto.ActionDateTime;
         entity.Temp = dto.Temp;
         entity.TempUomId = dto.TempUom.Id;
         entity.PH = dto.pH;
         entity.Sugar = dto.Sugar;
         entity.SugarUomId = dto.SugarUom.Id;
         entity.Ta = dto.Ta;
         entity.So2 = dto.So2;
         entity.Additions = dto.Additions;
         entity.Comments = dto.Comments;
         entity.Racked = dto.Racked;
         entity.Filtered = dto.Filtered;
         entity.Bottled = dto.Bottled;

         // Update entity in DbSet
         _dbContext.BatchEntries.Update(entity);

         // Save changes in database
         _dbContext.SaveChanges();

         return dto;
      }

      /// <summary>
      /// Update a <see cref="BatchEntryDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="BatchEntryDto"/></param>
      /// <returns><see cref="BatchEntryDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
      public async Task<BatchEntryDto> UpdateAsync(BatchEntryDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = await _dbContext.BatchEntries.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);

         entity.BatchId = dto.BatchId;
         entity.EntryDateTime = dto.EntryDateTime;
         entity.ActionDateTime = dto.ActionDateTime;
         entity.Temp = dto.Temp;
         entity.TempUomId = dto.TempUom.Id;
         entity.PH = dto.pH;
         entity.Sugar = dto.Sugar;
         entity.SugarUomId = dto.SugarUom.Id;
         entity.Ta = dto.Ta;
         entity.So2 = dto.So2;
         entity.Additions = dto.Additions;
         entity.Comments = dto.Comments;
         entity.Racked = dto.Racked;
         entity.Filtered = dto.Filtered;
         entity.Bottled = dto.Bottled;

         // Update entity in DbSet
         _dbContext.BatchEntries.Update(entity);

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         return dto;
      }

      /// <summary>
      /// Delete a <see cref="BatchEntryDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="BatchEntryDto"/></param>
      /// <inheritdoc cref="ICommand{T}.Delete(T)"/>
      public void Delete(BatchEntryDto dto)
      {
         var entity = _dbContext.BatchEntries
         .FirstOrDefault(c => c.Id == dto.Id);

         if (entity != null)
         {
            // delete category 
            _dbContext.BatchEntries.Remove(entity);

            // Save changes in database
            _dbContext.SaveChanges();
         }
      }

      /// <summary>
      /// Delete a <see cref="BatchEntryDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="BatchEntryDto"/></param>
      /// <inheritdoc cref="ICommand{T}.DeleteAsyn(T)"/>
      public async Task DeleteAsync(BatchEntryDto dto)
      {
         var entity = await _dbContext.BatchEntries
         .FirstOrDefaultAsync(c => c.Id == dto.Id)
         .ConfigureAwait(false);

         if (entity != null)
         {
            // delete category 
            _dbContext.BatchEntries.Remove(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
         }
      }

   }

}
