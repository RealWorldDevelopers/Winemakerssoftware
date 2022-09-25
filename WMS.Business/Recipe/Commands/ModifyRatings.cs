
using AutoMapper;
using System.Threading.Tasks;
using WMS.Business.Recipe.Dto;
using WMS.Business.Common;
using WMS.Data.SQL;
using Microsoft.EntityFrameworkCore;
using System;

namespace WMS.Business.Recipe.Commands
{
    /// <summary>
    /// Rating Command Instance
    /// </summary>
    public class ModifyRatings : ICommand<RatingDto>
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

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
        /// <returns><see cref="Task{Rating}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
        public async Task<RatingDto> Add(RatingDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Data.SQL.Entities.Rating>(dto);

            // Update entity in DbSet
            await _dbContext.Ratings.AddAsync(entity).ConfigureAwait(false);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            dto.Id = entity.Id;

            return dto;
        }

        /// <summary>
        /// Update a <see cref="RatingDto"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="RatingDto"/></param>
        /// <returns><see cref="Task{Rating}"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<RatingDto> Update(RatingDto dto)
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
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/> 
        public async Task Delete(int id)
        {
            var entity = await _dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == id).ConfigureAwait(false);
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
