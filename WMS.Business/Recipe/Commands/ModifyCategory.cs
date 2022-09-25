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
    public class ModifyCategory : ICommand<ICodeDto>
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        /// <summary>
        /// Category Command Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public ModifyCategory(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Add an <see cref="ICodeDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICodeDto"/></param>
        /// <returns><see cref="Task{ICode}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(ICodeDto)"/>
        public async Task<ICodeDto> Add(ICodeDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Category>(dto);

            // add new category
            await _dbContext.Categories.AddAsync(entity).ConfigureAwait(false);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Update a <see cref="ICodeDto"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICodeDto"/></param>
        /// <returns><see cref="ICodeDto"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<ICodeDto> Update(ICodeDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = await _dbContext.Categories.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
            entity.Description = dto.Description;
            entity.Enabled = dto.Enabled;
            entity.Category1 = dto.Literal;

            // Update entity in DbSet
            _dbContext.Categories.Update(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dto;
        }

        /// <summary>
        /// Delete a <see cref="ICodeDto"/> in the Database
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsyn(T)"/>
        public async Task Delete(int id)
        {
            var entity = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == id)
            .ConfigureAwait(false);

            if (entity != null)
            {
                // delete category 
                _dbContext.Categories.Remove(entity);

                // Save changes in database
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

    }
}
