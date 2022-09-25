using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Business.Yeast.Dto;
using WMS.Data.SQL;

namespace WMS.Business.Journal.Queries
{
    /// <summary>
    /// Batches Query Instance
    /// </summary>
    /// <inheritdoc cref="IQuery{T}"/>
    public class GetBatches : IQuery<BatchDto>
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _dbContext;

        /// <summary>
        /// Batches Query Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public GetBatches(WMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Asynchronously query all Batches in SQL DB
        /// </summary>
        /// <returns>Batches as <see cref="Task{List{BatchDto}}"/></returns>
        /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
        public async Task<List<BatchDto>> Execute()
        {
            // using TPL to parallel call gets
            List<Task> tasks = new List<Task>();
            var t1 = Task.Run(async () =>
                await _dbContext.Batches.ToListAsync().ConfigureAwait(false));
            tasks.Add(t1);
            var list = _mapper.Map<List<BatchDto>>(await t1.ConfigureAwait(false));

            var t2 = Task.Run(async () => await _dbContext.Categories.ToListAsync().ConfigureAwait(false));
            tasks.Add(t2);
            var categories = await t2.ConfigureAwait(false);

            var t3 = Task.Run(async () => await _dbContext.Varieties.ToListAsync().ConfigureAwait(false));
            tasks.Add(t3);
            var varieties = await t3.ConfigureAwait(false);

            var t4 = Task.Run(async () => await _dbContext.Yeasts.ToListAsync().ConfigureAwait(false));
            tasks.Add(t4);
            var yeasts = await t4.ConfigureAwait(false);

            var t5 = Task.Run(async () => await _dbContext.Targets.ToListAsync().ConfigureAwait(false));
            tasks.Add(t5);
            var targets = await t5.ConfigureAwait(false);

            Task.WaitAll(tasks.ToArray());

            foreach (var item in list)
            {
                if (item.Variety != null)
                {
                    var code = varieties.SingleOrDefault(a => a.Id == item.Variety.Id);
                    item.Variety.Literal = code.Variety1;
                }
                if (item.Yeast != null)
                {
                    var y = yeasts.SingleOrDefault(a => a.Id == item.Yeast.Id);
                    item.Yeast = _mapper.Map<YeastDto>(y);
                }
                if (item.Target?.Id.HasValue == true)
                {
                    var t = targets.SingleOrDefault(t => t.Id == item.Target.Id);
                    item.Target = _mapper.Map<TargetDto>(t);
                }
            }

            return list;
        }

        /// <summary>
        /// Asynchronously query a specific Batch in SQL DB by primary key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns>Batch as <see cref="Task{BatchDto}"/></returns>
        /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
        public async Task<BatchDto> Execute(int id)
        {
            var batch = await _dbContext.Batches
               .FirstOrDefaultAsync(r => r.Id == id)
               .ConfigureAwait(false);
            var dto = _mapper.Map<BatchDto>(batch);


            if (dto.Yeast?.Id != null)
            {
                var yeast = await _dbContext.Yeasts
                    .FirstOrDefaultAsync(y => y.Id == dto.Yeast.Id)
                    .ConfigureAwait(false);
                dto.Yeast.Brand = new CodeDto { Id = yeast?.Brand };
                dto.Yeast.Style = new CodeDto { Id = yeast?.Style };
            }

            if (dto.Target?.Id.HasValue == true)
            {
                var target = await _dbContext.Targets
                   .FirstOrDefaultAsync(t => t.Id == dto.Target.Id)
                   .ConfigureAwait(false);
                dto.Target = _mapper.Map<TargetDto>(target);
            }

            return dto;
        }



        public Task<List<BatchDto>> Execute(int start, int length)
        {
            throw new NotImplementedException();
        }

        public Task<List<BatchDto>> ExecuteByFK(int fk)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BatchDto>> ExecuteByUser(string userId)
        {
            // using TPL to parallel call gets
            List<Task> tasks = new List<Task>();
            var t1 = Task.Run(async () =>
                await _dbContext.Batches.Where(b => b.SubmittedBy == userId).ToListAsync().ConfigureAwait(false));
            tasks.Add(t1);
            var list = _mapper.Map<List<BatchDto>>(await t1.ConfigureAwait(false));

            var t2 = Task.Run(async () => await _dbContext.Categories.ToListAsync().ConfigureAwait(false));
            tasks.Add(t2);
            var categories = await t2.ConfigureAwait(false);

            var t3 = Task.Run(async () => await _dbContext.Varieties.ToListAsync().ConfigureAwait(false));
            tasks.Add(t3);
            var varieties = await t3.ConfigureAwait(false);

            var t4 = Task.Run(async () => await _dbContext.Yeasts.ToListAsync().ConfigureAwait(false));
            tasks.Add(t4);
            var yeasts = await t4.ConfigureAwait(false);

            var t5 = Task.Run(async () => await _dbContext.Targets.ToListAsync().ConfigureAwait(false));
            tasks.Add(t5);
            var targets = await t5.ConfigureAwait(false);

            Task.WaitAll(tasks.ToArray());

            foreach (var item in list)
            {
                if (item.Variety != null)
                {
                    var code = varieties.SingleOrDefault(a => a.Id == item.Variety.Id);
                    item.Variety.Literal = code.Variety1;
                }
                if (item.Yeast != null)
                {
                    var y = yeasts.SingleOrDefault(a => a.Id == item.Yeast.Id);
                    item.Yeast = _mapper.Map<YeastDto>(y);
                }
                if (item.Target?.Id.HasValue == true)
                {
                    var t = targets.SingleOrDefault(t => t.Id == item.Target.Id);
                    item.Target = _mapper.Map<TargetDto>(t);
                }
            }

            return list;
        }

    }

}
