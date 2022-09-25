using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.MaloCulture.Dto;
using WMS.Data.SQL;

namespace WMS.Business.MaloCulture.Commands
{
   public class ModifyMaloCulture : ICommand<MaloCultureDto>
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
      /// Add a <see cref="MaloCultureDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="MaloCultureDto"/></param>
      /// <returns><see cref="Task{MaloCultureDto}"/></returns>
      /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
      public async Task<Dto.MaloCultureDto> Add(MaloCultureDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = _mapper.Map<Data.SQL.Entities.MaloCulture>(dto);

         // add new recipe
         await _dbContext.MaloCultures.AddAsync(entity);

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         dto.Id = entity.Id;
         return dto;
      }

      /// <summary>
      /// Update a <see cref="MaloCultureDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="MaloCultureDto"/></param>
      /// <returns><see cref="Task{MaloCultureDto}"/></returns>
      /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
      public async Task<MaloCultureDto> Update(MaloCultureDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var entity = await _dbContext.MaloCultures.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
         entity.Alcohol = dto.Alcohol;
         entity.Brand = dto.Brand?.Id;
         entity.Id = dto.Id.Value;
         entity.Note = dto.Note;
         entity.Style = dto.Style?.Id;
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
        /// Remove a <see cref="MaloCultureDto"/> to Database
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
        public async Task Delete(int id)
      {
         var entity = await _dbContext.MaloCultures.FirstOrDefaultAsync(v => v.Id == id).ConfigureAwait(false);
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
