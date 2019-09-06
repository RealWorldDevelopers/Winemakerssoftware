using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Image.Dto;
using WMS.Business.Recipe.Dto;
using WMS.Business.Shared;
using WMS.Data;

namespace WMS.Business.Image.Queries
{
    /// <summary>
    /// Dto.Image Query Instance
    /// </summary>
    /// <inheritdoc cref="IQuery{T}"/>
    public class GetImages : IQuery<Dto.Image>
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
        /// <returns>Images as <see cref="List{Dto.Image}"/></returns>
        /// <inheritdoc cref="IQuery{T}.Execute"/>
        public List<Dto.Image> Execute()
        {
            var images = _dbContext.Images.ToList();
            var list = _mapper.Map<List<Dto.Image>>(images);
            return list;
        }

        /// <summary>
        /// Query a specific Image in SQL DB by primary key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns>Image as <see cref="Dto.Image"/></returns>
        /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
        public Dto.Image Execute(int id)
        {
            var image =  _dbContext.Images
               .Where(r => r.Id == id)
               .FirstOrDefault();

            var dto = _mapper.Map<Dto.Image>(image);

            return dto;
        }

        /// <summary>
        /// Asynchronously query all Images in SQL DB
        /// </summary>
        /// <returns>Images as <see cref="Task{List{Dto.Image}}"/></returns>
        /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
        public async Task<List<Dto.Image>> ExecuteAsync()
        {
            var images = await _dbContext.Images.ToListAsync();
            var list = _mapper.Map<List<Dto.Image>>(images);
            return list;
        }

        /// <summary>
        /// Asynchronously query a specific Image in SQL DB by primary key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns>Image as <see cref="Task{Dto.Image}"/></returns>
        /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
        public async Task<Dto.Image> ExecuteAsync(int id)
        {
            var images = await _dbContext.Images
               .Where(r => r.Id == id)
               .FirstOrDefaultAsync();

            var dto = _mapper.Map<Dto.Image>(images);

            return dto;
        }

      
    }
}
