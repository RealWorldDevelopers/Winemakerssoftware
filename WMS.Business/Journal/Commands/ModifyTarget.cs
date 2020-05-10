
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

   public class ModifyTarget : ICommand<TargetDto>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Target Command Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public ModifyTarget(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Add an <see cref="TargetDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="TargetDto"/></param>
      /// <returns><see cref="TargetDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Add(TargetDto)"/>
      public TargetDto Add(TargetDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _mapper.Map<Targets>(dto);

         // add new recipe
         _dbContext.Targets.Add(entity);

         // Save changes in database
         _dbContext.SaveChanges();

         //dto.Id = entity.Id;
         return dto;
      }

      /// <summary>
      /// Add an <see cref="TargetDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="TargetDto"/></param>
      /// <returns><see cref="Task{TargetDto}"/></returns>
      /// <inheritdoc cref="ICommand{T}.AddAsync(TargetDto)"/>
      public async Task<TargetDto> AddAsync(TargetDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _mapper.Map<Targets>(dto);

         // add new recipe
         await _dbContext.Targets.AddAsync(entity).ConfigureAwait(false);

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         //dto.Id = entity.Id;
         return dto;
      }

      /// <summary>
      /// Update a <see cref="TargetDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="TargetDto"/></param>
      /// <returns><see cref="TargetDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Update(T)"/>
      public TargetDto Update(TargetDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _dbContext.Targets.First(r => r.Id == dto.Id);
         entity.EndSugar = dto.EndSugar;
         entity.EndSugarUomId = dto.EndSugarUomId;
         entity.PH = dto.pH;
         entity.StartSugar = dto.StartSugar;
         entity.StartSugarUomId = dto.StartSugarUomId;
         entity.Ta = dto.TA;
         entity.Temp = dto.Temp;
         entity.TempUomId = dto.TempUomId;

         // Update entity in DbSet
         _dbContext.Targets.Update(entity);

         // Save changes in database
         _dbContext.SaveChanges();

         return dto;
      }

      /// <summary>
      /// Update a <see cref="TargetDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="TargetDto"/></param>
      /// <returns><see cref="TargetDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
      public async Task<TargetDto> UpdateAsync(TargetDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = await _dbContext.Targets.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
         entity.EndSugar = dto.EndSugar;
         entity.EndSugarUomId = dto.EndSugarUomId;
         entity.PH = dto.pH;
         entity.StartSugar = dto.StartSugar;
         entity.StartSugarUomId = dto.StartSugarUomId;
         entity.Ta = dto.TA;
         entity.Temp = dto.Temp;
         entity.TempUomId = dto.TempUomId;

         // Update entity in DbSet
         _dbContext.Targets.Update(entity);

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         return dto;
      }

      /// <summary>
      /// Delete a <see cref="TargetDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="TargetDto"/></param>
      /// <inheritdoc cref="ICommand{T}.Delete(T)"/>
      public void Delete(TargetDto dto)
      {
         var entity = _dbContext.Targets
         .FirstOrDefault(c => c.Id == dto.Id);

         if (entity != null)
         {
            // delete category 
            _dbContext.Targets.Remove(entity);

            // Save changes in database
            _dbContext.SaveChanges();
         }
      }

      /// <summary>
      /// Delete a <see cref="TargetDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="TargetDto"/></param>
      /// <inheritdoc cref="ICommand{T}.DeleteAsyn(T)"/>
      public async Task DeleteAsync(TargetDto dto)
      {
         var entity = await _dbContext.Targets
         .FirstOrDefaultAsync(c => c.Id == dto.Id)
         .ConfigureAwait(false);

         if (entity != null)
         {
            // delete category 
            _dbContext.Targets.Remove(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
         }
      }


   }
}
