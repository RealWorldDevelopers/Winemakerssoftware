using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Data;

namespace WMS.Business.Journal.Queries
{
   public class GetTargets : IQuery<TargetDto>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      public GetTargets(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Query all Targets in SQL DB
      /// </summary>
      /// <returns>Targets as <see cref="List{TargetDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<TargetDto> Execute()
      {
         var targets = _dbContext.Targets.ToList();
         var list = _mapper.Map<List<TargetDto>>(targets);
         return list;
      }

      /// <summary>
      /// Query a specific Target in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Batch as <see cref="TargetDto"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public TargetDto Execute(int id)
      {
         var target = _dbContext.Targets.FirstOrDefault(r => r.Id == id);
         var dto = _mapper.Map<TargetDto>(target);
         return dto;
      }

      /// <summary>
      /// Asynchronously query all Targets in SQL DB
      /// </summary>
      /// <returns>Targets as <see cref="Task{List{TargetDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<TargetDto>> ExecuteAsync()
      {
         var targets = await _dbContext.Targets.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<TargetDto>>(targets);
         return list;
      }

      /// <summary>
      /// Asynchronously query a specific Target in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Batch as <see cref="Task{TargetDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<TargetDto> ExecuteAsync(int id)
      {
         var target = await _dbContext.Targets
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);
         var dto = _mapper.Map<TargetDto>(target);
         return dto;
      }

      public List<TargetDto> ExecuteByFK(int fk)
      {
         throw new NotImplementedException();
      }

      public Task<List<TargetDto>> ExecuteByFKAsync(int fk)
      {
         throw new NotImplementedException();
      }
   }
}
