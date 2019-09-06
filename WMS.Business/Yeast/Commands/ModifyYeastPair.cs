
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Shared;
using WMS.Data;
using WMS.Data.Entities;

namespace WMS.Business.Yeast.Commands
{
    public class ModifyYeastPair : ICommand<Dto.YeastPair>
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        public ModifyYeastPair(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a <see cref="Dto.YeastPair"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastPair"/></param>
        /// <returns><see cref="Dto.YeastPair"/></returns>
        /// <inheritdoc cref="ICommand{T}.Add(T)"/>
        public Dto.YeastPair Add(Dto.YeastPair dto)
        {
            var entity = _mapper.Map<YeastPair>(dto);

            // add new recipe
            _dbContext.YeastPair.Add(entity);

            // Save changes in database
            _dbContext.SaveChanges();

            //dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Add a <see cref="Dto.YeastPair"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastPair"/></param>
        /// <returns><see cref="Task{Dto.YeastPair}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
        public async Task<Dto.YeastPair> AddAsync(Dto.YeastPair dto)
        {
            var entity = _mapper.Map<YeastPair>(dto);

            // add new recipe
            await _dbContext.YeastPair.AddAsync(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync();

            //dto.Id = entity.Id;
            return dto;
        }

        /// <summary>
        /// Update a <see cref="Dto.YeastPair"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastPair"/></param>
        /// <returns><see cref="Dto.YeastPair"/></returns>
        /// <inheritdoc cref="ICommand{T}.Update(T)"/>
        public Dto.YeastPair Update(Dto.YeastPair dto)
        {
            var entity = _dbContext.YeastPair.Where(r => r.Id == dto.Id).First();
            entity.Category = dto.Category;
            entity.Id = dto.Id;
            entity.Note = dto.Note;
            entity.Variety = dto.Variety;
            entity.Yeast = dto.Yeast;

            // Update entity in DbSet
            _dbContext.YeastPair.Update(entity);

            // Save changes in database
            _dbContext.SaveChanges();

            return dto;
        }

        /// <summary>
        /// Update a <see cref="Dto.YeastPair"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastPair"/></param>
        /// <returns><see cref="Task{Dto.YeastPair}"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<Dto.YeastPair> UpdateAsync(Dto.YeastPair dto)
        {
            var entity = _dbContext.YeastPair.Where(r => r.Id == dto.Id).First();
            entity.Category = dto.Category;
            entity.Id = dto.Id;
            entity.Note = dto.Note;
            entity.Variety = dto.Variety;
            entity.Yeast = dto.Yeast;

            // Update entity in DbSet
            _dbContext.YeastPair.Update(entity);

            // Save changes in database
            await _dbContext.SaveChangesAsync();

            return dto;
        }

        /// <summary>
        /// Remove a <see cref="Dto.YeastPair"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastPair"/></param>
        /// <inheritdoc cref="ICommand{T}.Delete(T)"/>
        public void Delete(Dto.YeastPair dto)
        {
            var entity = _dbContext.YeastPair.FirstOrDefault(v => v.Id == dto.Id);
            if (entity != null)
            {
                // add new recipe
                _dbContext.YeastPair.Remove(entity);

                // Save changes in database
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Remove a <see cref="Dto.YeastPair"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="Dto.YeastPair"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
        public async Task DeleteAsync(Dto.YeastPair dto)
        {
            var entity = _dbContext.YeastPair.FirstOrDefault(v => v.Id == dto.Id);
            if (entity != null)
            {
                // add new recipe
                _dbContext.YeastPair.Remove(entity);

                // Save changes in database
                await _dbContext.SaveChangesAsync();
            }
        }


    }
}
