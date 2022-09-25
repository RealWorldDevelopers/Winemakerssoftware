
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;
using WMS.Data.SQL;
using WMS.Data.SQL.Entities;

namespace WMS.Business.Yeast.Commands
{
    public class ModifyYeastPair : ICommand<Dto.YeastPairDto>
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        public ModifyYeastPair(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a <see cref="YeastPairDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="YeastPairDto"/></param>
        /// <returns><see cref="Task{YeastPairDto}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
        public async Task<YeastPairDto> Add(YeastPairDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<YeastPair>(dto);

            // add new entity
            await _dbContext.YeastPairs.AddAsync(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            dto.Id = entity.Id;


            return dto;
        }

        /// <summary>
        /// Update a <see cref="YeastPairDto"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="YeastPairDto"/></param>
        /// <returns><see cref="Task{YeastPairDto}"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<YeastPairDto> Update(YeastPairDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = await _dbContext.YeastPairs.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
            entity.Category = dto.Category;
            entity.Id = dto.Id.Value;
            entity.Note = dto.Note;
            entity.Variety = dto.Variety;
            entity.Yeast = dto.Yeast;

            // Update entity in DbSet
            _dbContext.YeastPairs.Update(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dto;
        }

       /// <summary>
        /// Remove a <see cref="YeastPairDto"/> to Database
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
        public async Task Delete(int id)
        {
            var entity = await _dbContext.YeastPairs.FirstOrDefaultAsync(v => v.Id == id).ConfigureAwait(false);
            if (entity != null)
            {
                // add new recipe
                _dbContext.YeastPairs.Remove(entity);

                // Save changes in database
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }


    }
}
