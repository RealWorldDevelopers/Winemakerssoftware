
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Recipe.Dto;
using WMS.Data.SQL;
using WMS.Data.SQL.Entities;

namespace WMS.Business.Recipe.Commands
{
   /// <summary>
   /// Recipe Command Instance
   /// </summary>
   public class ModifyRecipes : ICommand<RecipeDto>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

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
      /// Add a <see cref="RecipeDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="RecipeDto"/></param>
      /// <returns><see cref="Task{RecipeDto}"/></returns>
      /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
      public async Task<RecipeDto> Add(RecipeDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _mapper.Map<Data.SQL.Entities.Recipe>(dto);

         // add new target
         if (dto.Target != null)
         {
            var target = _mapper.Map<Target>(dto.Target);
            await _dbContext.Targets.AddAsync(target);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            entity.TargetId = target.Id;
         }

         // add new recipe   
         await _dbContext.Recipes.AddAsync(entity);

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         dto.Id = entity.Id;
         if (entity.TargetId.HasValue && dto.Target != null)
            dto.Target.Id = entity.TargetId;
         return dto;
      }

      /// <summary>
      /// Delete a <see cref="RecipeDto"/> to Database
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
      public async Task Delete(int id)
      {
         // get recipe to delete
         var entity = await _dbContext.Recipes.FirstOrDefaultAsync(r => r.Id == id).ConfigureAwait(false);
         if (entity != null)
         {
            // see if any batches related to recipe id
            if (_dbContext.Batches.Any(b => b.RecipeId.HasValue && b.RecipeId.Value == entity.Id))
            {
               var batchEntities = _dbContext.Batches.Where(b => b.RecipeId == entity.Id);
               if (batchEntities != null)
                  foreach (var e in batchEntities)
                  {
                     e.RecipeId = null;
                     _dbContext.Batches.Update(e);
                  }
            }

            // see if any batches related to target id
            if (entity.TargetId.HasValue)
            {
               // if target is not in any batches, delete target entity too
               if (!_dbContext.Batches.Any(b => b.TargetId.HasValue && b.TargetId.Value == entity.TargetId))
               {
                  var targetEntity = await _dbContext.Targets.FirstOrDefaultAsync(t => t.Id == entity.TargetId).ConfigureAwait(false);
                  if (targetEntity != null) _dbContext.Targets.Remove(targetEntity);
               }
            }

            // delete recipe
            _dbContext.Recipes.Remove(entity);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
         }
      }

      /// <summary>
      /// Update a <see cref="RecipeDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="RecipeDto"/></param>
      /// <returns><see cref="Task{RecipeDto}"/></returns>
      /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
      public async Task<RecipeDto> Update(RecipeDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = await _dbContext.Recipes.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
         entity.Description = dto.Description;
         entity.Enabled = dto.Enabled;
         entity.NeedsApproved = dto.NeedsApproved;
         entity.Hits = dto.Hits;
         entity.Ingredients = dto.Ingredients;
         entity.Instructions = dto.Instructions;
         entity.VarietyId = dto.Variety?.Id;
         entity.YeastId = dto.Yeast?.Id;
         entity.TargetId = dto.Target?.Id;

         if (dto.Title != null)
            entity.Title = dto.Title;

         // Update entity in DbSet
         _dbContext.Recipes.Update(entity);

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         return dto;
      }

   }
}
