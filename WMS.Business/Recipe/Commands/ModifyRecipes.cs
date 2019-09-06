﻿
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Shared;
using WMS.Data;
using WMS.Data.Entities;

namespace WMS.Business.Recipe.Commands
{
    /// <summary>
    /// Recipe Command Instance
    /// </summary>
    public class ModifyRecipes : ICommand<Dto.Recipe>
    {

        private readonly IMapper _mapper;
        private readonly Data.WMSContext _dbContext;

        /// <summary>
        /// Recipe Command Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public ModifyRecipes(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a <see cref="Dto.Recipe"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.Recipe"/></param>
        /// <returns><see cref="Dto.Recipe"/></returns>
        /// <inheritdoc cref="ICommand{T}.Add(T)"/>
        public Dto.Recipe Add(Dto.Recipe dto)
        {
            var entity = _mapper.Map<Recipes>(dto);

            // add new recipe
            _dbContext.Recipes.Add(entity);

            // Save changes in database
            _dbContext.SaveChanges();

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Add a <see cref="Dto.Recipe"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.Recipe"/></param>
        /// <returns><see cref="Task{Dto.Recipe}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
        public async Task<Dto.Recipe> AddAsync(Dto.Recipe dto)
        {
            var entity = _mapper.Map<Recipes>(dto);

            // add new recipe
            await _dbContext.Recipes.AddAsync(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Delete a <see cref="Dto.Recipe"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.Recipe"/></param>
        /// <inheritdoc cref="ICommand{T}.Delete(T)"/>
        public void Delete(Dto.Recipe dto)
        {
            var entity = _dbContext.Recipes.FirstOrDefault(r => r.Id == dto.Id);
            if (entity != null)
            {
                // add new recipe
                _dbContext.Recipes.Remove(entity);

                // Save changes in database
                 _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Delete a <see cref="Dto.Recipe"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.Recipe"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
        public async Task DeleteAsync(Dto.Recipe dto)
        {
            var entity = _dbContext.Recipes.FirstOrDefault(r => r.Id == dto.Id);
            if (entity != null)
            {
                // add new recipe
                _dbContext.Recipes.Remove(entity);

                // Save changes in database
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update a <see cref="Dto.Recipe"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.Recipe"/></param>
        /// <returns><see cref="Dto.Recipe"/></returns>
        /// <inheritdoc cref="ICommand{T}.Update(T)"/>
        public Dto.Recipe Update(Dto.Recipe dto)
        {
            var entity = _dbContext.Recipes.Where(r => r.Id == dto.Id).First();
            entity.Description = dto.Description;
            entity.Enabled = dto.Enabled;
            entity.Hits = dto.Hits;
            entity.Ingredients = dto.Ingredients;
            entity.Instructions = dto.Instructions;
            entity.Title = dto.Title;
            entity.VarietyId = dto.Variety?.Id;

            // Update entity in DbSet
            _dbContext.Recipes.Update(entity);

            // Save changes in database
             _dbContext.SaveChanges();

            return dto;
        }

        /// <summary>
        /// Update a <see cref="Dto.Recipe"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.Recipe"/></param>
        /// <returns><see cref="Task{Dto.Recipe}"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<Dto.Recipe> UpdateAsync(Dto.Recipe dto)
        {
            var entity = _dbContext.Recipes.Where(r => r.Id == dto.Id).First();
            entity.Description = dto.Description;
            entity.Enabled = dto.Enabled;
            entity.NeedsApproved = dto.NeedsApproved;
            entity.Hits = dto.Hits;
            entity.Ingredients = dto.Ingredients;
            entity.Instructions = dto.Instructions;
            entity.Title = dto.Title;
            entity.VarietyId = dto.Variety?.Id;

            // Update entity in DbSet
            _dbContext.Recipes.Update(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync();

            return dto;
        }

    }
}