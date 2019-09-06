using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Recipe.Dto;
using WMS.Business.Shared;
using WMS.Data;
using WMS.Data.Entities;

namespace WMS.Business.Recipe.Commands
{
    /// <summary>
    /// Image Command Instance
    /// </summary>
    public class ModifyImages : ICommand<ImageFile>
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        /// <summary>
        /// Image Command Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public ModifyImages(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Add an <see cref="ImageFile"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ImageFile"/></param>
        /// <returns><see cref="ImageFile"/></returns>
        /// <inheritdoc cref="ICommand{T}.Add(T)"/>
        public ImageFile Add(ImageFile dto)
        {
            Images image = new Images
            {
                ContentType = dto.ContentType,
                Length = dto.Length,
                Name = dto.Name,
                FileName = dto.FileName,
                Data = dto.Data,
                Thumbnail=dto.Thumbnail
            };
            _dbContext.Images.Add(image);

            // Save image in database
            _dbContext.SaveChanges();

            PicturesXref xRef = new PicturesXref
            {
                RecipeId = dto.RecipeId,
                ImageId = image.Id
            };
            _dbContext.PicturesXref.Add(xRef);

            // Save xRef in database
            _dbContext.SaveChanges();

            dto.Id = image.Id;
            return dto;
        }

        /// <summary>
        /// Add an <see cref="ImageFile"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ImageFile"/></param>
        /// <returns><see cref="Task{ImageFile}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
        public async Task<ImageFile> AddAsync(ImageFile dto)
        {
            Images image = new Images
            {
                ContentType = dto.ContentType,
                Length = dto.Length,
                Name = dto.Name,
                FileName = dto.FileName,
                Data = dto.Data,
                Thumbnail=dto.Thumbnail
            };
            _dbContext.Images.Add(image);

            // Save image in database
            await _dbContext.SaveChangesAsync();

            PicturesXref xRef = new PicturesXref
            {
                RecipeId = dto.RecipeId,
                ImageId = image.Id
            };
            _dbContext.PicturesXref.Add(xRef);

            // Save xRef in database
            await _dbContext.SaveChangesAsync();

            dto.Id = image.Id;
            return dto;
        }

        /// <summary>
        /// Update an <see cref="ImageFile"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ImageFile"/></param>
        /// <returns><see cref="ImageFile"/></returns>
        /// <inheritdoc cref="ICommand{T}.Update(T)"/>
        public ImageFile Update(ImageFile dto)
        {
            var xRef = _dbContext.PicturesXref.Where(r => r.ImageId == dto.Id && r.RecipeId == dto.RecipeId).First();
            xRef.ImageId = dto.Id;
            xRef.RecipeId = dto.RecipeId;

            var image = _dbContext.Images.Where(r => r.Id == dto.Id).First();
            image.ContentType = dto.ContentType;
            image.Length = dto.Length;
            image.Name = dto.Name;
            image.FileName = dto.FileName;
            image.Data = dto.Data;
            image.Thumbnail = dto.Thumbnail;

            // Update entity in DbSet
            _dbContext.Images.Update(image);
            _dbContext.PicturesXref.Update(xRef);

            // Save changes in database
            _dbContext.SaveChanges();

            return dto;
        }

        /// <summary>
        /// Update an <see cref="ImageFile"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ImageFile"/></param>
        /// <returns><see cref="Task{ImageFile}"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<ImageFile> UpdateAsync(ImageFile dto)
        {
            var xRef = _dbContext.PicturesXref.Where(r => r.ImageId == dto.Id && r.RecipeId == dto.RecipeId).First();
            xRef.ImageId = dto.Id;
            xRef.RecipeId = dto.RecipeId;

            var image = _dbContext.Images.Where(r => r.Id == dto.Id).First();
            image.ContentType = dto.ContentType;
            image.Length = dto.Length;
            image.Name = dto.Name;
            image.FileName = dto.FileName;
            image.Thumbnail = dto.Thumbnail;
            image.Data = dto.Data;

            // Update entity in DbSet
            _dbContext.Images.Update(image);
            _dbContext.PicturesXref.Update(xRef);

            // Save changes in database
            await _dbContext.SaveChangesAsync();

            return dto;
        }

        /// <summary>
        /// Delete an <see cref="ImageFile"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ImageFile"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
        public void Delete(ImageFile dto)
        {
            var xRef = _dbContext.PicturesXref.Where(r => r.ImageId == dto.Id && r.RecipeId == dto.RecipeId).First();
            if (xRef != null)
            {
                // Update entity in DbSet
                _dbContext.PicturesXref.Remove(xRef);
            }

            var image = _dbContext.Images.Where(r => r.Id == dto.Id).First();
            if (image != null)
            {
                // Update entity in DbSet
                _dbContext.Images.Remove(image);
            }

            // Save changes in database
            _dbContext.SaveChanges();

        }

        /// <summary>
        /// Delete an <see cref="ImageFile"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ImageFile"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
        public async Task DeleteAsync(ImageFile dto)
        {
            var xRef = _dbContext.PicturesXref.Where(r => r.ImageId == dto.Id && r.RecipeId == dto.RecipeId).First();
            if (xRef != null)
            {
                // Update entity in DbSet
                _dbContext.PicturesXref.Remove(xRef);
            }

            var image = _dbContext.Images.Where(r => r.Id == dto.Id).First();
            if (image != null)
            {
                // Update entity in DbSet
                _dbContext.Images.Remove(image);
            }

            // Save changes in database
            await _dbContext.SaveChangesAsync();


        }

    }
}
