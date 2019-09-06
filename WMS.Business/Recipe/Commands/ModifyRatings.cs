
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Recipe.Dto;
using WMS.Business.Shared;
using WMS.Data;

namespace WMS.Business.Recipe.Commands
{
    /// <summary>
    /// Rating Command Instance
    /// </summary>
    public class ModifyRatings : ICommand<Rating>
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
        /// Add a <see cref="Rating"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Rating"/></param>
        /// <returns><see cref="Rating"/></returns>
        /// <inheritdoc cref="ICommand{T}.Add(T)"/>
        public Rating Add(Rating dto)
        {
            var entity = _mapper.Map<Data.Entities.Ratings>(dto);

            // Update entity in DbSet
            _dbContext.Ratings.Add(entity);

            // Save changes in database
            _dbContext.SaveChanges();

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Add a <see cref="Rating"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Rating"/></param>
        /// <returns><see cref="Task{Rating}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
        public async Task<Rating> AddAsync(Rating dto)
        {
            var entity = _mapper.Map<Data.Entities.Ratings>(dto);

            // Update entity in DbSet
            _dbContext.Ratings.Add(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Update a <see cref="Rating"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Rating"/></param>
        /// <returns><see cref="Rating"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public Rating Update(Rating dto)
        {
            var entity = _dbContext.Ratings.Where(r => r.Id == dto.Id).First();
            entity.OriginIp = dto.OriginIp;
            entity.TotalValue = dto.TotalValue;
            entity.TotalVotes = dto.TotalVotes;

            // Update entity in DbSet
            _dbContext.Ratings.Update(entity);

            // Save changes in database
            _dbContext.SaveChanges();

            return dto;
        }

        /// <summary>
        /// Update a <see cref="Rating"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Rating"/></param>
        /// <returns><see cref="Task{Rating}"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<Rating> UpdateAsync(Rating dto)
        {
            var entity = _dbContext.Ratings.Where(r => r.Id == dto.Id).First();
            entity.OriginIp = dto.OriginIp;
            entity.TotalValue = dto.TotalValue;
            entity.TotalVotes = dto.TotalVotes;

            // Update entity in DbSet
            _dbContext.Ratings.Update(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync();

            return dto;
        }

        /// <summary>
        /// Delete a <see cref="Rating"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Rating"/></param>
        /// <inheritdoc cref="ICommand{T}.Delete(T)"/> 
        public void Delete(Rating dto)
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
        /// Delete a <see cref="Rating"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Rating"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/> 
        public async Task DeleteAsync(Rating dto)
        {
            var entity = _dbContext.Ratings.FirstOrDefault(r => r.Id == dto.Id);
            if (entity != null)
            {
                // Update entity in DbSet
                _dbContext.Ratings.Remove(entity);

                // Save changes in database
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
