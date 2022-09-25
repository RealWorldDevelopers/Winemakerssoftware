
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;
using WMS.Data.SQL;

namespace WMS.Business.Yeast.Commands
{
    public class ModifyYeast : ICommand<YeastDto>
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
        /// Add a <see cref="YeastDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="YeastDto"/></param>
        /// <returns><see cref="Task{YeastDto}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
        public async Task<YeastDto> Add(YeastDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Data.SQL.Entities.Yeast>(dto);

            // add new recipe
            await _dbContext.Yeasts.AddAsync(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Update a <see cref="YeastDto"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="YeastDto"/></param>
        /// <returns><see cref="Task{YeastDto}"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<YeastDto> Update(YeastDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = await _dbContext.Yeasts.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
            entity.Alcohol = dto.Alcohol;
            entity.Brand = dto.Brand?.Id;
            entity.Id = dto.Id.Value;
            entity.Note = dto.Note;
            entity.Style = dto.Style?.Id;
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
        /// Remove a <see cref="YeastDto"/> to Database
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
        public async Task Delete(int id)
        {
            var entity = await _dbContext.Yeasts.FirstOrDefaultAsync(v => v.Id == id).ConfigureAwait(false);
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
