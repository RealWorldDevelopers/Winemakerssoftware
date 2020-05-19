using WMS.Business.Common;
using WMS.Business.Journal.Dto;

namespace WMS.Business.Journal.Queries
{
   /// <summary>
   /// Factory Object used to create <see cref="IQuery{T}"/> objects
   /// </summary>
   public interface IFactory
   {
      /// <summary>
      /// Create a Batches Query
      /// </summary>
      /// <returns><see cref="IQuery{BatchDto}"/></returns>
      IQuery<BatchDto> CreateBatchesQuery();

      /// <summary>
      /// Create a Batch Volume UOM Query
      /// </summary>
      /// <returns><see cref="IQuery{IUnitOfMeasure}"/></returns>
      IQuery<IUnitOfMeasure> CreateBatchVolumeUOMQuery();

      /// <summary>
      /// Create a Batch Volume UOM Query
      /// </summary>
      /// <returns><see cref="IQuery{IUnitOfMeasure}"/></returns>
      IQuery<IUnitOfMeasure> CreateBatchTempUOMQuery();

      /// <summary>
      /// Create a Batch Volume UOM Query
      /// </summary>
      /// <returns><see cref="IQuery{IUnitOfMeasure}"/></returns>
      IQuery<IUnitOfMeasure> CreateBatchSugarUOMQuery();

   }
}
