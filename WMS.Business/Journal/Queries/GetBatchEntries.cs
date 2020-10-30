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
   public class GetBatchEntries : IQuery<BatchEntryDto>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Batch Entry Query Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public GetBatchEntries(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }


      /// <summary>
      /// Query all Entries in SQL DB
      /// </summary>
      /// <returns>Entries as <see cref="List{BatchEntryDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<BatchEntryDto> Execute()
      {
         var batches = _dbContext.BatchEntries.ToList();
         var list = _mapper.Map<List<BatchEntryDto>>(batches);
         return list;
      }

      /// <summary>
      /// Query a specific Entry in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Entry as <see cref="BatchEntryDto"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public BatchEntryDto Execute(int id)
      {
         var Batch = _dbContext.BatchEntries
            .FirstOrDefault(r => r.Id == id);
         var dto = _mapper.Map<BatchEntryDto>(Batch);        
         return dto;
      }

      /// <summary>
      /// Asynchronously query all Entries in SQL DB
      /// </summary>
      /// <returns>Entries as <see cref="Task{List{BatchEntryDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<BatchEntryDto>> ExecuteAsync()
      {
         var Batches = await _dbContext.BatchEntries.ToListAsync().ConfigureAwait(false);
         var list = _mapper.Map<List<BatchEntryDto>>(Batches);
         return list;
      }

      /// <summary>
      /// Asynchronously query a specific Entry in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Entry as <see cref="Task{BatchEntryDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<BatchEntryDto> ExecuteAsync(int id)
      {
         var Batch = await _dbContext.BatchEntries
            .FirstOrDefaultAsync(r => r.Id == id)
            .ConfigureAwait(false);
         var dto = _mapper.Map<BatchEntryDto>(Batch);         
         return dto;
      }

   }

}
