using WMS.Business.Common;

namespace WMS.Business.MaloCulture.Queries
{
   /// <summary>
   /// Factory Object used to create <see cref="IQuery{T}"/> objects
   /// </summary>
   public interface IFactory
   {
      /// <summary>
      /// Create a MaloCulture Query
      /// </summary>
      /// <returns><see cref="IQuery{Code}"/></returns>
      IQuery<ICode> CreateBrandsQuery();

      /// <summary>
      /// Create a MaloCulture Query
      /// </summary>
      /// <returns><see cref="IQuery{Code}"/></returns>
      IQuery<ICode> CreateStylesQuery();

      /// <summary>
      /// Create a MaloCulture Query
      /// </summary>
      /// <returns><see cref="IQuery{MaloCultureDto}"/></returns>
      IQuery<Dto.MaloCultureDto> CreateMaloCulturesQuery();

   

   }
}
