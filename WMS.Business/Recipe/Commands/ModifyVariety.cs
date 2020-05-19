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
    public class ModifyVariety : ICommand<ICode>
    {
        private readonly IMapper _mapper;
        private readonly Data.WMSContext _dbContext;

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
        /// Add a <see cref="ICode"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICode"/></param>
        /// <returns><see cref="ICode"/></returns>
        /// <inheritdoc cref="ICommand{T}.Add(T)"/>
        public ICode Add(ICode dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Varieties>(dto);

            // add new recipe
             _dbContext.Varieties.Add(entity);

            // Save changes in database
             _dbContext.SaveChanges();

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Add a <see cref="ICode"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICode"/></param>
        /// <returns><see cref="Task{ICode}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
        public async Task<ICode> AddAsync(ICode dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Varieties>(dto);

            // add new recipe
            await _dbContext.Varieties.AddAsync(entity);

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

            var entity = _dbContext.Varieties.First(r => r.Id == dto.Id);
            entity.Description = dto.Description;
            entity.Enabled = dto.Enabled;
            entity.Variety = dto.Literal;
            entity.CategoryId = dto.ParentId;

            // Update entity in DbSet
            _dbContext.Varieties.Update(entity);

            // Save changes in database
             _dbContext.SaveChanges();

            return dto;
        }

        /// <summary>
        /// Update a <see cref="ICode"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICode"/></param>
        /// <returns><see cref="Task{ICode}"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<ICode> UpdateAsync(ICode dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity  = await _dbContext.Varieties.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
            entity.Description = dto.Description;
            entity.Enabled = dto.Enabled;
            entity.Variety = dto.Literal;
            entity.CategoryId = dto.ParentId;

            // Update entity in DbSet
            _dbContext.Varieties.Update(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dto;
        }

        /// <summary>
        /// Remove a <see cref="ICode"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICode"/></param>
        /// <inheritdoc cref="ICommand{T}.Delete(T)"/>
        public void  Delete(ICode dto)
        {
            var entity = _dbContext.Varieties.FirstOrDefault(v => v.Id == dto.Id);
            if (entity != null)
            {
                // add new recipe
                _dbContext.Varieties.Remove(entity);

                // Save changes in database
                 _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Remove a <see cref="ICode"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ICode"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
        public async Task DeleteAsync(ICode dto)
        {
            var entity = await _dbContext.Varieties.FirstOrDefaultAsync(v => v.Id == dto.Id).ConfigureAwait(false);
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
