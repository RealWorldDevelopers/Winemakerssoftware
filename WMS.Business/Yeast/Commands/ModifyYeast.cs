
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
    public class ModifyYeast : ICommand<Dto.YeastDto>
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        /// <summary>
        /// Yeast Command Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public ModifyYeast(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a <see cref="Dto.YeastDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastDto"/></param>
        /// <returns><see cref="Dto.YeastDto"/></returns>
        /// <inheritdoc cref="ICommand{T}.Add(T)"/>
        public Dto.YeastDto Add(Dto.YeastDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Yeasts>(dto);

            // add new recipe
            _dbContext.Yeasts.Add(entity);

            // Save changes in database
            _dbContext.SaveChanges();

            //dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Add a <see cref="Dto.YeastDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastDto"/></param>
        /// <returns><see cref="Task{Dto.YeastDto}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
        public async Task<Dto.YeastDto> AddAsync(Dto.YeastDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Yeasts>(dto);

            // add new recipe
            await _dbContext.Yeasts.AddAsync(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            //dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Update a <see cref="Dto.YeastDto"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastDto"/></param>
        /// <returns><see cref="Dto.YeastDto"/></returns>
        /// <inheritdoc cref="ICommand{T}.Update(T)"/>
        public Dto.YeastDto Update(Dto.YeastDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _dbContext.Yeasts.First(r => r.Id == dto.Id);
            entity.Alcohol = dto.Alcohol;
            entity.Brand = dto.Brand.Id;
            entity.Id = dto.Id;
            entity.Note = dto.Note;
            entity.Style = dto.Style.Id;
            entity.TempMax = dto.TempMax;
            entity.TempMin = dto.TempMin;
            entity.Trademark = dto.Trademark;    

            // Update entity in DbSet
            _dbContext.Yeasts.Update(entity);

            // Save changes in database
            _dbContext.SaveChanges();

            return dto;
        }

        /// <summary>
        /// Update a <see cref="Dto.YeastDto"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastDto"/></param>
        /// <returns><see cref="Task{Dto.YeastDto}"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<Dto.YeastDto> UpdateAsync(Dto.YeastDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = await _dbContext.Yeasts.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
            entity.Alcohol = dto.Alcohol;
            entity.Brand = dto.Brand.Id;
            entity.Id = dto.Id;
            entity.Note = dto.Note;
            entity.Style = dto.Style.Id;
            entity.TempMax = dto.TempMax;
            entity.TempMin = dto.TempMin;
            entity.Trademark = dto.Trademark;

            // Update entity in DbSet
            _dbContext.Yeasts.Update(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dto;
        }

        /// <summary>
        /// Remove a <see cref="Dto.YeastDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastDto"/></param>
        /// <inheritdoc cref="ICommand{T}.Delete(T)"/>
        public void Delete(Dto.YeastDto dto)
        {
            var entity = _dbContext.Yeasts.FirstOrDefault(v => v.Id == dto.Id);
            if (entity != null)
            {
                // add new recipe
                _dbContext.Yeasts.Remove(entity);

                // Save changes in database
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Remove a <see cref="Dto.YeastDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastDto"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
        public async Task DeleteAsync(Dto.YeastDto dto)
        {
            var entity = await _dbContext.Yeasts.FirstOrDefaultAsync(v => v.Id == dto.Id).ConfigureAwait(false);
            if (entity != null)
            {
                // add new recipe
                _dbContext.Yeasts.Remove(entity);

                // Save changes in database
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

    }
}
