using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Data;
using WMS.Data.Entities;

namespace WMS.Business.Recipe.Commands
{
    public class ModifyCategory : ICommand<ICode>
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
        /// Add an <see cref="ICode"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICode"/></param>
        /// <returns><see cref="ICode"/></returns>
        /// <inheritdoc cref="ICommand{T}.Add(ICode)"/>
        public ICode Add(ICode dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Categories>(dto);

            // add new recipe
            _dbContext.Categories.Add(entity);

            // Save changes in database
            _dbContext.SaveChanges();

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Add an <see cref="ICode"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICode"/></param>
        /// <returns><see cref="Task{ICode}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(ICode)"/>
        public async Task<ICode> AddAsync(ICode dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Categories>(dto);

            // add new recipe
            await _dbContext.Categories.AddAsync(entity).ConfigureAwait(false);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Update a <see cref="ICode"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICode"/></param>
        /// <returns><see cref="ICode"/></returns>
        /// <inheritdoc cref="ICommand{T}.Update(T)"/>
        public ICode Update(ICode dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity =  _dbContext.Categories.First(r => r.Id == dto.Id);
            entity.Description = dto.Description;
            entity.Enabled = dto.Enabled;
            entity.Category = dto.Literal;

            // Update entity in DbSet
            _dbContext.Categories.Update(entity);

            // Save changes in database
            _dbContext.SaveChanges();

            return dto;
        }

        /// <summary>
        /// Update a <see cref="ICode"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICode"/></param>
        /// <returns><see cref="ICode"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<ICode> UpdateAsync(ICode dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = await _dbContext.Categories.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
            entity.Description = dto.Description;
            entity.Enabled = dto.Enabled;
            entity.Category = dto.Literal;

            // Update entity in DbSet
            _dbContext.Categories.Update(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dto;
        }

        /// <summary>
        /// Delete a <see cref="ICode"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICode"/></param>
        /// <inheritdoc cref="ICommand{T}.Delete(T)"/>
        public void Delete(ICode dto)
        {
            var entity = _dbContext.Categories
            .FirstOrDefault(c => c.Id == dto.Id);

            if (entity != null)
            {
                // delete category 
                _dbContext.Categories.Remove(entity);

                // Save changes in database
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Delete a <see cref="ICode"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICode"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsyn(T)"/>
        public async Task DeleteAsync(ICode dto)
        {
            var entity = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == dto.Id)
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
