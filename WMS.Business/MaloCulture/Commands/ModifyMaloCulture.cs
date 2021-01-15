using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Data;
using WMS.Data.Entities;

namespace WMS.Business.MaloCulture.Commands
{
   public class ModifyMaloCulture : ICommand<Dto.MaloCultureDto>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// MaloCulture Command Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public ModifyMaloCulture(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Add a <see cref="Dto.MaloCultureDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="Dto.MaloCultureDto"/></param>
      /// <returns><see cref="Dto.MaloCultureDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Add(T)"/>
      public Dto.MaloCultureDto Add(Dto.MaloCultureDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _mapper.Map<MaloCultures>(dto);

         // add new recipe
         _dbContext.MaloCultures.Add(entity);

         // Save changes in database
         _dbContext.SaveChanges();

         dto.Id = entity.Id;
         return dto;
      }

      /// <summary>
      /// Add a <see cref="Dto.MaloCultureDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="Dto.MaloCultureDto"/></param>
      /// <returns><see cref="Task{Dto.MaloCultureDto}"/></returns>
      /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
      public async Task<Dto.MaloCultureDto> AddAsync(Dto.MaloCultureDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _mapper.Map<MaloCultures>(dto);

         // add new recipe
         await _dbContext.MaloCultures.AddAsync(entity);

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         dto.Id = entity.Id;
         return dto;
      }

      /// <summary>
      /// Update a <see cref="Dto.MaloCultureDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="Dto.MaloCultureDto"/></param>
      /// <returns><see cref="Dto.MaloCultureDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Update(T)"/>
      public Dto.MaloCultureDto Update(Dto.MaloCultureDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _dbContext.MaloCultures.First(r => r.Id == dto.Id);
         entity.Alcohol = dto.Alcohol;
         entity.Brand = dto.Brand.Id;
         entity.Id = dto.Id;
         entity.Note = dto.Note;
         entity.Style = dto.Style.Id;
         entity.TempMax = dto.TempMax;
         entity.TempMin = dto.TempMin;
         entity.Trademark = dto.Trademark;
         entity.So2 = dto.So2;
         entity.PH = dto.pH;        

         // Update entity in DbSet
         _dbContext.MaloCultures.Update(entity);

         // Save changes in database
         _dbContext.SaveChanges();

         return dto;
      }

      /// <summary>
      /// Update a <see cref="Dto.MaloCultureDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="Dto.MaloCultureDto"/></param>
      /// <returns><see cref="Task{Dto.MaloCultureDto}"/></returns>
      /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
      public async Task<Dto.MaloCultureDto> UpdateAsync(Dto.MaloCultureDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = await _dbContext.MaloCultures.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
         entity.Alcohol = dto.Alcohol;
         entity.Brand = dto.Brand.Id;
         entity.Id = dto.Id;
         entity.Note = dto.Note;
         entity.Style = dto.Style.Id;
         entity.TempMax = dto.TempMax;
         entity.TempMin = dto.TempMin;
         entity.Trademark = dto.Trademark;
         entity.So2 = dto.So2;
         entity.PH = dto.pH;

         // Update entity in DbSet
         _dbContext.MaloCultures.Update(entity);

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         return dto;
      }

      /// <summary>
      /// Remove a <see cref="Dto.MaloCultureDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="Dto.MaloCultureDto"/></param>
      /// <inheritdoc cref="ICommand{T}.Delete(T)"/>
      public void Delete(Dto.MaloCultureDto dto)
      {
         var entity = _dbContext.MaloCultures.FirstOrDefault(v => v.Id == dto.Id);
         if (entity != null)
         {
            // add new recipe
            _dbContext.MaloCultures.Remove(entity);

            // Save changes in database
            _dbContext.SaveChanges();
         }
      }

      /// <summary>
      /// Remove a <see cref="Dto.MaloCultureDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="Dto.MaloCultureDto"/></param>
      /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
      public async Task DeleteAsync(Dto.MaloCultureDto dto)
      {
         var entity = await _dbContext.MaloCultures.FirstOrDefaultAsync(v => v.Id == dto.Id).ConfigureAwait(false);
         if (entity != null)
         {
            // add new recipe
            _dbContext.MaloCultures.Remove(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
         }
      }

   }
}
