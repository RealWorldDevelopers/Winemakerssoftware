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
      /// Query all Batches in SQL DB
      /// </summary>
      /// <returns>Batches as <see cref="List{BatchDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<BatchDto> Execute()
      {
         var batches = _dbContext.Batches.ToList();
         var list = _mapper.Map<List<BatchDto>>(batches);
         return list;
      }

      /// <summary>
      /// Query a specific Batch in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Batch as <see cref="BatchDto"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public BatchDto Execute(int id)
      {
         var Batch = _dbContext.Batches
            .FirstOrDefault(r => r.Id == id);
         var dto = _mapper.Map<BatchDto>(Batch);
         return dto;
      }

      /// <summary>
      /// Asynchronously query all Batches in SQL DB
      /// </summary>
      /// <returns>Batches as <see cref="Task{List{BatchDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<BatchDto>> ExecuteAsync()
      {
         var Batches = await _dbContext.Batches.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<BatchDto>>(Batches);
         return list;
      }

      /// <summary>
      /// Asynchronously query a specific Batch in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Batch as <see cref="Task{BatchDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<BatchDto> ExecuteAsync(int id)
      {
         var Batch = await _dbContext.Batches
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);
         var dto = _mapper.Map<BatchDto>(Batch);
         return dto;
      }


   }

}
