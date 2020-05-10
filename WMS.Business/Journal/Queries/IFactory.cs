using WMS.Business.Common;

namespace WMS.Business.Journal.Queries
{
   /// <summary>
   /// Factory Object used to create <see cref="IQuery{T}"/> objects
   /// </summary>
   public interface IFactory
   {
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
