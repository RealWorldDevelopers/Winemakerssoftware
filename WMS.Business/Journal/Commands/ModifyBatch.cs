
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Data;
using WMS.Data.Entities;

namespace WMS.Business.Journal.Commands
{
   public class ModifyBatch : ICommand<BatchDto>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Batch Command Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public ModifyBatch(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Add an <see cref="BatchDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="BatchDto"/></param>
      /// <returns><see cref="BatchDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Add(BatchDto)"/>
      public BatchDto Add(BatchDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _mapper.Map<Batches>(dto);

         // add new batch
         _dbContext.Batches.Add(entity);

         // if entity has recipe id update recipe's target id
         if (entity.RecipeId.HasValue && entity.TargetId.HasValue)
         {
            var recipeEntity = _dbContext.Recipes.FirstOrDefault(r => r.Id == entity.RecipeId);
            if (!recipeEntity.TargetId.HasValue)
            {
               recipeEntity.TargetId = entity.TargetId;
               _dbContext.Recipes.Update(recipeEntity);
            }
         }

         // Save changes in database
         _dbContext.SaveChanges();

         dto.Id = entity.Id;
         return dto;
      }

      /// <summary>
      /// Add an <see cref="BatchDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="BatchDto"/></param>
      /// <returns><see cref="Task{BatchDto}"/></returns>
      /// <inheritdoc cref="ICommand{T}.AddAsync(BatchDto)"/>
      public async Task<BatchDto> AddAsync(BatchDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _mapper.Map<Batches>(dto);

         // add new batch
         await _dbContext.Batches.AddAsync(entity).ConfigureAwait(false);

         // if entity has recipe id update recipe's target id
         if (entity.RecipeId.HasValue && entity.TargetId.HasValue)
         {
            var recipeEntity = await _dbContext.Recipes.FirstOrDefaultAsync(r => r.Id == entity.RecipeId).ConfigureAwait(false);
            if (!recipeEntity.TargetId.HasValue)
            {
               recipeEntity.TargetId = entity.TargetId;
               _dbContext.Recipes.Update(recipeEntity);
            }
         }

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         dto.Id = entity.Id;
         return dto;
      }

      /// <summary>
      /// Update a <see cref="BatchDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="BatchDto"/></param>
      /// <returns><see cref="BatchDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Update(T)"/>
      public BatchDto Update(BatchDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _dbContext.Batches.First(r => r.Id == dto.Id);
         entity.Complete = dto.Complete;
         entity.RecipeId = dto.RecipeId;
         entity.SubmittedBy = dto.SubmittedBy;
         entity.TargetId = dto.Target.Id;
         entity.Title = dto.Title;
         entity.Description = dto.Description;
         entity.VarietyId = dto.Variety.Id;
         entity.Vintage = dto.Vintage;
         entity.Volume = dto.Volume;
         entity.VolumeUomId = dto.VolumeUom.Id;

         // Update entity in DbSet
         _dbContext.Batches.Update(entity);

         // if entity has recipe id update recipe's target id
         if (entity.RecipeId.HasValue && entity.TargetId.HasValue)
         {
            var recipeEntity = _dbContext.Recipes.FirstOrDefault(r => r.Id == entity.RecipeId);
            if (!recipeEntity.TargetId.HasValue)
            {
               recipeEntity.TargetId = entity.TargetId;
               _dbContext.Recipes.Update(recipeEntity);
            }
         }

         // Save changes in database
         _dbContext.SaveChanges();

         return dto;
      }

      /// <summary>
      /// Update a <see cref="BatchDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="BatchDto"/></param>
      /// <returns><see cref="BatchDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
      public async Task<BatchDto> UpdateAsync(BatchDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = await _dbContext.Batches.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
         entity.Complete = dto.Complete;
         entity.RecipeId = dto.RecipeId;
         entity.SubmittedBy = dto.SubmittedBy;
         entity.TargetId = dto.Target.Id;
         entity.Title = dto.Title;
         entity.Description = dto.Description;
         entity.VarietyId = dto.Variety.Id;
         entity.Vintage = dto.Vintage;
         entity.Volume = dto.Volume;
         entity.VolumeUomId = dto.VolumeUom.Id;

         // Update entity in DbSet
         _dbContext.Batches.Update(entity);

         // if entity has recipe id update recipe's target id
         if (entity.RecipeId.HasValue && entity.TargetId.HasValue)
         {
            var recipeEntity = await _dbContext.Recipes.FirstOrDefaultAsync(r => r.Id == entity.RecipeId).ConfigureAwait(false);
            if (!recipeEntity.TargetId.HasValue)
            {
               recipeEntity.TargetId = entity.TargetId;
               _dbContext.Recipes.Update(recipeEntity);
            }
         }

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         return dto;
      }

      /// <summary>
      /// Delete a <see cref="BatchDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="BatchDto"/></param>
      /// <inheritdoc cref="ICommand{T}.Delete(T)"/>
      public void Delete(BatchDto dto)
      {
         var entity = _dbContext.Batches
         .FirstOrDefault(c => c.Id == dto.Id);

         if (entity != null)
         {
            // delete category 
            _dbContext.Batches.Remove(entity);

            // Save changes in database
            _dbContext.SaveChanges();
         }
      }

      /// <summary>
      /// Delete a <see cref="BatchDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="BatchDto"/></param>
      /// <inheritdoc cref="ICommand{T}.DeleteAsyn(T)"/>
      public async Task DeleteAsync(BatchDto dto)
      {
         var entity = await _dbContext.Batches
         .FirstOrDefaultAsync(c => c.Id == dto.Id)
         .ConfigureAwait(false);

         if (entity != null)
         {
            // delete category 
            _dbContext.Batches.Remove(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
         }
      }


   }
}
