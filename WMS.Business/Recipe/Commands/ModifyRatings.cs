
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Recipe.Dto;
using WMS.Business.Common;
using WMS.Data;
using Microsoft.EntityFrameworkCore;

namespace WMS.Business.Recipe.Commands
{
   /// <summary>
   /// Rating Command Instance
   /// </summary>
   public class ModifyRatings : ICommand<RatingDto>
   {
      private readonly IMapper _mapper;
      private readonly Data.WMSContext _dbContext;

      /// <summary>
      /// Ratings Command Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public ModifyRatings(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Add a <see cref="RatingDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="RatingDto"/></param>
      /// <returns><see cref="RatingDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Add(T)"/>
      public RatingDto Add(RatingDto dto)
      {
         var entity = _mapper.Map<Data.Entities.Ratings>(dto);

         // Update entity in DbSet
         _dbContext.Ratings.Add(entity);

         // Save changes in database
         _dbContext.SaveChanges();

         if (dto != null)
            dto.Id = entity.Id;

         return dto;
      }

      /// <summary>
      /// Add a <see cref="RatingDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="RatingDto"/></param>
      /// <returns><see cref="Task{Rating}"/></returns>
      /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
      public async Task<RatingDto> AddAsync(RatingDto dto)
      {
         var entity = _mapper.Map<Data.Entities.Ratings>(dto);

         // Update entity in DbSet
         await _dbContext.Ratings.AddAsync(entity).ConfigureAwait(false);

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         if (dto != null)
            dto.Id = entity.Id;

         return dto;
      }

      /// <summary>
      /// Update a <see cref="RatingDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="RatingDto"/></param>
      /// <returns><see cref="RatingDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
      public RatingDto Update(RatingDto dto)
      {
         var entity = _dbContext.Ratings.First(r => r.Id == dto.Id);
         entity.OriginIp = dto?.OriginIp;
         entity.TotalValue = dto.TotalValue;
         entity.TotalVotes = dto.TotalVotes;

         // Update entity in DbSet
         _dbContext.Ratings.Update(entity);

         // Save changes in database
         _dbContext.SaveChanges();

         return dto;
      }

      /// <summary>
      /// Update a <see cref="RatingDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="RatingDto"/></param>
      /// <returns><see cref="Task{Rating}"/></returns>
      /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
      public async Task<RatingDto> UpdateAsync(RatingDto dto)
      {
         var entity = await _dbContext.Ratings.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
         entity.OriginIp = dto?.OriginIp;
         entity.TotalValue = dto.TotalValue;
         entity.TotalVotes = dto.TotalVotes;

         // Update entity in DbSet
         _dbContext.Ratings.Update(entity);

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         return dto;
      }

      /// <summary>
      /// Delete a <see cref="RatingDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="RatingDto"/></param>
      /// <inheritdoc cref="ICommand{T}.Delete(T)"/> 
      public void Delete(RatingDto dto)
      {
         var entity = _dbContext.Ratings.FirstOrDefault(r => r.Id == dto.Id);
         if (entity != null)
         {
            // Update entity in DbSet
            _dbContext.Ratings.Remove(entity);

            // Save changes in database
            _dbContext.SaveChanges();
         }
      }

      /// <summary>
      /// Delete a <see cref="RatingDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="RatingDto"/></param>
      /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/> 
      public async Task DeleteAsync(RatingDto dto)
      {
         var entity = await _dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == dto.Id).ConfigureAwait(false);
         if (entity != null)
         {
            // Update entity in DbSet
            _dbContext.Ratings.Remove(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
         }
      }

   }
}
