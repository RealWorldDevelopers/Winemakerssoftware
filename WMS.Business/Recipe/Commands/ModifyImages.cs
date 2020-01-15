
using System;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Recipe.Dto;
using WMS.Business.Common;
using WMS.Data;
using WMS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace WMS.Business.Recipe.Commands
{
   /// <summary>
   /// Image Command Instance
   /// </summary>
   public class ModifyImages : ICommand<ImageFileDto>
   {
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Image Command Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      public ModifyImages(WMSContext dbContext)
      {
         _dbContext = dbContext;
      }

      /// <summary>
      /// Add an <see cref="ImageFileDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="ImageFileDto"/></param>
      /// <returns><see cref="ImageFileDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Add(T)"/>
      public ImageFileDto Add(ImageFileDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         Images image = new Images()
         {
            ContentType = dto.ContentType,
            Length = dto.Length,
            Name = dto.Name,
            FileName = dto.FileName,
            Data = dto.Data(),
            Thumbnail = dto.Thumbnail()
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
      /// Add an <see cref="ImageFileDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="ImageFileDto"/></param>
      /// <returns><see cref="Task{ImageFile}"/></returns>
      /// <inheritdoc cref="ICommand{T}.AddAsync(T)"/>
      public async Task<ImageFileDto> AddAsync(ImageFileDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         Images image = new Images
         {
            ContentType = dto.ContentType,
            Length = dto.Length,
            Name = dto.Name,
            FileName = dto.FileName,
            Data = dto.Data(),
            Thumbnail = dto.Thumbnail()
         };
         await _dbContext.Images.AddAsync(image).ConfigureAwait(false);

         // Save image in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         PicturesXref xRef = new PicturesXref
         {
            RecipeId = dto.RecipeId,
            ImageId = image.Id
         };
         _dbContext.PicturesXref.Add(xRef);

         // Save xRef in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         dto.Id = image.Id;
         return dto;
      }

      /// <summary>
      /// Update an <see cref="ImageFileDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="ImageFileDto"/></param>
      /// <returns><see cref="ImageFileDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Update(T)"/>
      public ImageFileDto Update(ImageFileDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var xRef = _dbContext.PicturesXref.First(r => r.ImageId == dto.Id && r.RecipeId == dto.RecipeId);
         xRef.ImageId = dto.Id;
         xRef.RecipeId = dto.RecipeId;

         var image = _dbContext.Images.First(r => r.Id == dto.Id);
         image.ContentType = dto.ContentType;
         image.Length = dto.Length;
         image.Name = dto.Name;
         image.FileName = dto.FileName;
         image.Data = dto.Data();
         image.Thumbnail = dto.Thumbnail();

         // Update entity in DbSet
         _dbContext.Images.Update(image);
         _dbContext.PicturesXref.Update(xRef);

         // Save changes in database
         _dbContext.SaveChanges();

         return dto;
      }

      /// <summary>
      /// Update an <see cref="ImageFileDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="ImageFileDto"/></param>
      /// <returns><see cref="Task{ImageFile}"/></returns>
      /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
      public async Task<ImageFileDto> UpdateAsync(ImageFileDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var xRef = await _dbContext.PicturesXref
            .FirstAsync(r => r.ImageId == dto.Id && r.RecipeId == dto.RecipeId)
            .ConfigureAwait(false);

         xRef.ImageId = dto.Id;
         xRef.RecipeId = dto.RecipeId;

         var image = await _dbContext.Images.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
         image.ContentType = dto.ContentType;
         image.Length = dto.Length;
         image.Name = dto.Name;
         image.FileName = dto.FileName;
         image.Thumbnail = dto.Thumbnail();
         image.Data = dto.Data();

         // Update entity in DbSet
         _dbContext.Images.Update(image);
         _dbContext.PicturesXref.Update(xRef);

         // Save changes in database
         await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         return dto;
      }

      /// <summary>
      /// Delete an <see cref="ImageFileDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="ImageFileDto"/></param>
      /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
      public void Delete(ImageFileDto dto)
      {
         var xRef = _dbContext.PicturesXref.First(r => r.ImageId == dto.Id && r.RecipeId == dto.RecipeId);
         if (xRef != null)
         {
            // Update entity in DbSet
            _dbContext.PicturesXref.Remove(xRef);
         }

         var image = _dbContext.Images.First(r => r.Id == dto.Id);
         if (image != null)
         {
            // Update entity in DbSet
            _dbContext.Images.Remove(image);
         }

         // Save changes in database
         _dbContext.SaveChanges();

      }

      /// <summary>
      /// Delete an <see cref="ImageFileDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="ImageFileDto"/></param>
      /// <inheritdoc cref="ICommand{T}.DeleteAsync(T)"/>
      public async Task DeleteAsync(ImageFileDto dto)
      {
         var xRef = await _dbContext.PicturesXref
            .FirstAsync(r => r.ImageId == dto.Id && r.RecipeId == dto.RecipeId)
            .ConfigureAwait(false);

         if (xRef != null)
         {
            // Update entity in DbSet
            _dbContext.PicturesXref.Remove(xRef);
         }

         var image = await _dbContext.Images.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
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
