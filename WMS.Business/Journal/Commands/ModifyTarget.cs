﻿
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Data.SQL;
using WMS.Data.SQL.Entities;

namespace WMS.Business.Journal.Commands
{

    public class ModifyTarget : ICommand<TargetDto>
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        /// <summary>
        /// Target Command Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public ModifyTarget(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Add an <see cref="TargetDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="TargetDto"/></param>
        /// <returns><see cref="Task{TargetDto}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(TargetDto)"/>
        public async Task<TargetDto> Add(TargetDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = new Data.SQL.Entities.Target
            {
                Temp = dto.Temp,
                TempUomId = dto.TempUom?.Id,
                PH = dto.pH,
                Ta = dto.TA,
                StartSugar = dto.StartSugar,
                StartSugarUomId = dto.StartSugarUom?.Id,
                EndSugar = dto.EndSugar,
                EndSugarUomId = dto.EndSugarUom?.Id
            };

            // add new recipe
            await _dbContext.Targets.AddAsync(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Update a <see cref="TargetDto"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="TargetDto"/></param>
        /// <returns><see cref="TargetDto"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<TargetDto> Update(TargetDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = await _dbContext.Targets.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
            entity.EndSugar = dto.EndSugar;
            entity.EndSugarUomId = dto.EndSugarUom?.Id;
            entity.PH = dto.pH;
            entity.StartSugar = dto.StartSugar;
            entity.StartSugarUomId = dto.StartSugarUom?.Id;
            entity.Ta = dto.TA;
            entity.Temp = dto.Temp;
            entity.TempUomId = dto.TempUom?.Id;

            // Update entity in DbSet
            _dbContext.Targets.Update(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dto;
        }

        /// <summary>
        /// Delete a <see cref="TargetDto"/> in the Database
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsyn(T)"/>
        public async Task Delete(int id)
        {
            var entity = await _dbContext.Targets
            .FirstOrDefaultAsync(c => c.Id == id)
            .ConfigureAwait(false);

            if (entity != null)
            {
                // delete category 
                _dbContext.Targets.Remove(entity);

                // Save changes in database
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }


    }
}
