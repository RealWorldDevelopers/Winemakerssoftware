using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Data;

namespace WMS.Business.Image.Queries
{
   /// <summary>
   /// Dto.Image Query Instance
   /// </summary>
   /// <inheritdoc cref="IQuery{T}"/>
   public class GetImages : IQuery<Dto.ImageDto>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Image Query Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public GetImages(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Query all Images in SQL DB
      /// </summary>
      /// <returns>Images as <see cref="List{Dto.ImageDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<Dto.ImageDto> Execute()
      {
         var dtoList = _dbContext.Images
            .ProjectTo<Dto.ImageDto>(_mapper.ConfigurationProvider).ToList();
         
         return dtoList;
      }

      /// <summary>
      /// Query a specific Image in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Image as <see cref="Dto.ImageDto"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public Dto.ImageDto Execute(int id)
      {
         var dto = _dbContext.Images
            .ProjectTo<Dto.ImageDto>(_mapper.ConfigurationProvider)
            .FirstOrDefault(r => r.Id == id);

         return dto;
      }

      /// <summary>
      /// Asynchronously query all Images in SQL DB
      /// </summary>
      /// <returns>Images as <see cref="Task{List{Dto.ImageDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<Dto.ImageDto>> ExecuteAsync()
      {
         var dtoList = await _dbContext.Images
            .ProjectTo<Dto.ImageDto>(_mapper.ConfigurationProvider)
            .ToListAsync().ConfigureAwait(false);
         
         return dtoList;
      }

      /// <summary>
      /// Asynchronously query a specific Image in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Image as <see cref="Task{Dto.ImageDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<Dto.ImageDto> ExecuteAsync(int id)
      {
         var dto = await _dbContext.Images
            .ProjectTo<Dto.ImageDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);

         return dto;
      }


   }
}
