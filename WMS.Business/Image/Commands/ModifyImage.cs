using System;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Image.Dto;
using AutoMapper;
using WMS.Data.SQL;
using WMS.Data.SQL.Entities;
using Microsoft.EntityFrameworkCore;


namespace WMS.Business.Image.Commands
{
    public class ModifyImage : ICommand<ImageDto>
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        /// <summary>
        /// Category Command Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public ModifyImage(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Add an <see cref="ImageFileDto"/> to Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ImageFileDto"/></param>
        /// <returns><see cref="Task{ImageFile}"/></returns>
        /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
        public async Task<ImageDto> Add(ImageDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var image = new Data.SQL.Entities.Image
            {
                ContentType = dto.ContentType,
                Length = dto.Length,
                Name = dto.Name,
                FileName = dto.FileName,
                Thumbnail = dto.Thumbnail
            };
            if (dto.Data != null)
                image.Data = dto.Data;

            await _dbContext.Images.AddAsync(image).ConfigureAwait(false);

            // Save image in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            var xRef = new PicturesXref
            {
                RecipeId = dto.RecipeId,
                ImageId = image.Id
            };
            _dbContext.PicturesXrefs.Add(xRef);

            // Save xRef in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            dto.Id = image.Id;
            return dto;
        }

        /// <summary>
        /// Update an <see cref="ImageFileDto"/> in the Database
        /// </summary>
        /// <param name="dto">Data Transfer Object as <see cref="ImageFileDto"/></param>
        /// <returns><see cref="Task{ImageFile}"/></returns>
        /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
        public async Task<ImageDto> Update(ImageDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var xRef = await _dbContext.PicturesXrefs
               .FirstAsync(r => r.ImageId == dto.Id && r.RecipeId == dto.RecipeId)
               .ConfigureAwait(false);

            xRef.ImageId = dto.Id;
            xRef.RecipeId = dto.RecipeId;

            var image = await _dbContext.Images.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
            image.ContentType = dto.ContentType;
            image.Length = dto.Length;
            image.Name = dto.Name;
            image.FileName = dto.FileName;
            image.Thumbnail = dto.Thumbnail;
            if (dto.Data != null)
                image.Data = dto.Data;

            // Update entity in DbSet
            _dbContext.Images.Update(image);
            _dbContext.PicturesXrefs.Update(xRef);

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dto;
        }

        /// <summary>
        /// Delete an <see cref="ImageFileDto"/> in the Database
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
        public async Task Delete(int id)
        {
            var xRef = await _dbContext.PicturesXrefs
               .FirstAsync(r => r.ImageId == id)
               .ConfigureAwait(false);

            if (xRef != null)
            {
                // Update entity in DbSet
                _dbContext.PicturesXrefs.Remove(xRef);
            }

            var image = await _dbContext.Images.FirstAsync(r => r.Id == id).ConfigureAwait(false);
            if (image != null)
            {
                // Update entity in DbSet
                _dbContext.Images.Remove(image);
            }

            // Save changes in database
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);


        }

    }

}
