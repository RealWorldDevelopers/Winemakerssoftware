using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Data.SQL;
using WMS.Data.SQL.Entities;

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
        /// <returns><see cref="Task{BatchEntryDto}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(BatchEntryDto)"/>
        public async Task<BatchEntryDto> Add(BatchEntryDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<BatchEntry>(dto);

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
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<BatchEntryDto> Update(BatchEntryDto dto)
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
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsyn(T)"/>
        public async Task Delete(int id)
        {
            var entity = await _dbContext.BatchEntries
            .FirstOrDefaultAsync(c => c.Id == id)
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
