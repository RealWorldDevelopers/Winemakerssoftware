using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Image.Dto;
using WMS.Data.SQL;

namespace WMS.Business.Image.Queries
{
   /// <summary>
   /// Dto.Image Query Instance
   /// </summary>
   /// <inheritdoc cref="IQuery{T}"/>
   public class GetImages : IQuery<ImageDto>
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
      /// Asynchronously query all Images in SQL DB
      /// </summary>
      /// <returns>Images as <see cref="Task{List{ImageDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<ImageDto>> Execute()
      {
         var images = await _dbContext.Images.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<ImageDto>>(images);
         return list;
      }

      /// <summary>
      /// Asynchronously query a specific Image in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Image as <see cref="Task{ImageDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<ImageDto> Execute(int id)
      {
         var images = await _dbContext.Images
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);
         var dto = _mapper.Map<ImageDto>(images);
         return dto;
      }

        public Task<List<ImageDto>> Execute(int start, int length)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ImageDto>> ExecuteByFK(int fk)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ImageDto>> ExecuteByUser(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
