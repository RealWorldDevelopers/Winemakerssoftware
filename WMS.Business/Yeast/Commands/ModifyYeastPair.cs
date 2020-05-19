
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Data;
using WMS.Data.Entities;

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
        /// Add a <see cref="Dto.YeastPairDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastPairDto"/></param>
        /// <returns><see cref="Dto.YeastPairDto"/></returns>
        /// <inheritdoc cref="ICommand{T}.Add(T)"/>
        public Dto.YeastPairDto Add(Dto.YeastPairDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<YeastPair>(dto);

            // add new recipe
            _dbContext.YeastPair.Add(entity);

            // Save changes in database
            _dbContext.SaveChanges();

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Add a <see cref="Dto.YeastPairDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastPairDto"/></param>
        /// <returns><see cref="Task{Dto.YeastPairDto}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
        public async Task<Dto.YeastPairDto> AddAsync(Dto.YeastPairDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<YeastPair>(dto);

            // add new recipe
            await _dbContext.YeastPair.AddAsync(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Update a <see cref="Dto.YeastPairDto"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastPairDto"/></param>
        /// <returns><see cref="Dto.YeastPairDto"/></returns>
        /// <inheritdoc cref="ICommand{T}.Update(T)"/>
        public Dto.YeastPairDto Update(Dto.YeastPairDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _dbContext.YeastPair.First(r => r.Id == dto.Id);
            entity.Category = dto.Category;
            entity.Id = dto.Id;
            entity.Note = dto.Note;
            entity.Variety = dto.Variety;
            entity.Yeast = dto.Yeast;

            // Update entity in DbSet
            _dbContext.YeastPair.Update(entity);

            // Save changes in database
            _dbContext.SaveChanges();

            return dto;
        }

        /// <summary>
        /// Update a <see cref="Dto.YeastPairDto"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastPairDto"/></param>
        /// <returns><see cref="Task{Dto.YeastPairDto}"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<Dto.YeastPairDto> UpdateAsync(Dto.YeastPairDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = await _dbContext.YeastPair.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
            entity.Category = dto.Category;
            entity.Id = dto.Id;
            entity.Note = dto.Note;
            entity.Variety = dto.Variety;
            entity.Yeast = dto.Yeast;

            // Update entity in DbSet
            _dbContext.YeastPair.Update(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dto;
        }

        /// <summary>
        /// Remove a <see cref="Dto.YeastPairDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastPairDto"/></param>
        /// <inheritdoc cref="ICommand{T}.Delete(T)"/>
        public void Delete(Dto.YeastPairDto dto)
        {
            var entity = _dbContext.YeastPair.FirstOrDefault(v => v.Id == dto.Id);
            if (entity != null)
            {
                // add new recipe
                _dbContext.YeastPair.Remove(entity);

                // Save changes in database
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Remove a <see cref="Dto.YeastPairDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastPairDto"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
        public async Task DeleteAsync(Dto.YeastPairDto dto)
        {
            var entity = await _dbContext.YeastPair.FirstOrDefaultAsync(v => v.Id == dto.Id).ConfigureAwait(false);
            if (entity != null)
            {
                // add new recipe
                _dbContext.YeastPair.Remove(entity);

                // Save changes in database
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }


    }
}
