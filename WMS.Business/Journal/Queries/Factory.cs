using AutoMapper;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Data;

namespace WMS.Business.Journal.Queries
{
   /// <summary>
   /// Instance of <see cref="Commands.ICommand{T}"/> Factory
   /// </summary>
   /// <inheritdoc cref="IFactory"/>>
   public class Factory : IFactory
   {
      private readonly WMSContext _dbContext;
      private readonly IMapper _mapper;

      /// <summary>
      /// Query Factory Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public Factory(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Instance of Create Batches Query
      /// </summary>
      /// <inheritdoc cref="IFactory.CreateBatchesQuery"/>>
      public IQuery<BatchDto> CreateBatchesQuery()
      {
         return new GetBatches(_dbContext, _mapper);
      }

      /// <summary>
      /// Instance of Create Units of Measure Query
      /// </summary>
      /// <inheritdoc cref="IFactory.CreateBatchVolumeUOM"/>>
      public IQuery<IUnitOfMeasure> CreateBatchVolumeUOMQuery()
      {
         return new GetBatchVolumeUOM(_dbContext, _mapper);
      }

      /// <summary>
      /// Instance of Create Units of Measure Query
      /// </summary>
      /// <inheritdoc cref="IFactory.CreateBatchVolumeUOM"/>>
      public IQuery<IUnitOfMeasure> CreateBatchTempUOMQuery()
      {
         return new GetBatchTempUOM(_dbContext, _mapper);
      }

      /// <summary>
      /// Instance of Create Units of Measure Query
      /// </summary>
      /// <inheritdoc cref="IFactory.CreateBatchVolumeUOM"/>>
      public IQuery<IUnitOfMeasure> CreateBatchSugarUOMQuery()
      {
         return new GetBatchSugarUOM(_dbContext, _mapper);
      }

   }
}
