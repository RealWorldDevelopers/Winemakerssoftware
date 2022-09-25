using AutoMapper;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WMS.Data.SQL;

namespace WMS.Business.Common.Queries
{

    public class GetUOMs : IQueryUOM
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        /// <summary>
        /// Rating Query Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public GetUOMs(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

      
        /// <inheritdoc cref="IQueryUOM.ExecuteAsync(string)"/>
        public async Task<List<UnitOfMeasureDto>> ExecuteAsync(string uomType)
        {
            var uoms = await _dbContext.UnitsOfMeasures.Where(uom => uom.Subset == uomType).ToListAsync().ConfigureAwait(false);
            var list = _mapper.Map<List<UnitOfMeasureDto>>(uoms);
            return list;
        }
                
        /// <inheritdoc cref="IQueryUOM.ExecuteAsync(string)"/>
        public async Task<UnitOfMeasureDto> ExecuteAsync(int id)
        {
            var uom = await _dbContext.UnitsOfMeasures.FirstOrDefaultAsync(uom => uom.Id == id).ConfigureAwait(false);
            var dto = _mapper.Map<UnitOfMeasureDto>(uom);
            return dto;
        }


    }



}
