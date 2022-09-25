using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Data.SQL;

namespace WMS.Business.Journal.Queries
{
    public class GetBatchSugarUOM : IQuery<IUnitOfMeasureDto>
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        /// <summary>
        /// Units of Measure Query Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public GetBatchSugarUOM(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Asynchronously query all Units of Measure in SQL DB
        /// </summary>
        /// <returns>Units of Measure as <see cref="Task{List{IUnitOfMeasureDto}}"/></returns>
        /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
        public async Task<List<IUnitOfMeasureDto>> Execute()
        {
            var UnitOfMeasure = await _dbContext.UnitsOfMeasures
               .Where(uom => uom.Subset == Common.Subsets.Sugar.Standard)
               .ToListAsync().ConfigureAwait(false);
            var list = _mapper.Map<List<IUnitOfMeasureDto>>(UnitOfMeasure);
            return list;
        }

        /// <summary>
        /// Asynchronously query a specific Unit of Measure in SQL DB by primary key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns>Unit of Measure as <see cref="Task{IUnitOfMeasure}"/></returns>
        /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
        public async Task<IUnitOfMeasureDto> Execute(int id)
        {
            var category = await _dbContext.UnitsOfMeasures
               .Where(uom => uom.Subset == Common.Subsets.Sugar.Standard)
               .FirstOrDefaultAsync(r => r.Id == id)
               .ConfigureAwait(false);
            var dto = _mapper.Map<IUnitOfMeasureDto>(category);
            return dto;
        }

        public Task<List<IUnitOfMeasureDto>> Execute(int start, int length)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<IUnitOfMeasureDto>> ExecuteByFK(int fk)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<IUnitOfMeasureDto>> ExecuteByUser(string userId)
        {
            throw new System.NotImplementedException();
        }
    }

}
