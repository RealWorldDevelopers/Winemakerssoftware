using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Data.SQL;
using WMS.Data.SQL.Entities;

namespace WMS.Business.Recipe.Commands
{
    public class ModifyVariety : ICommand<ICodeDto>
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        /// <summary>
        /// Category Command Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public ModifyVariety(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a <see cref="ICodeDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICodeDto"/></param>
        /// <returns><see cref="Task{ICode}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
        public async Task<ICodeDto> Add(ICodeDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Variety>(dto);

            // add new recipe
            await _dbContext.Varieties.AddAsync(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Update a <see cref="ICodeDto"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICodeDto"/></param>
        /// <returns><see cref="Task{ICode}"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<ICodeDto> Update(ICodeDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity  = await _dbContext.Varieties.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
            entity.Description = dto.Description;
            entity.Enabled = dto.Enabled;
            entity.Variety1 = dto.Literal;
            entity.CategoryId = dto.ParentId;

            // Update entity in DbSet
            _dbContext.Varieties.Update(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dto;
        }

        /// <summary>
        /// Remove a <see cref="ICodeDto"/> to Database
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
        public async Task Delete(int id)
        {
            var entity = await _dbContext.Varieties.FirstOrDefaultAsync(v => v.Id == id).ConfigureAwait(false);
            if (entity != null)
            {
                // add new recipe
                _dbContext.Varieties.Remove(entity);

                // Save changes in database
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

    }
}
