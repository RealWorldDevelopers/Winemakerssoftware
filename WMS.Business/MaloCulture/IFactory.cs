using WMS.Business.Common;
using WMS.Business.MaloCulture.Dto;

namespace WMS.Business.MaloCulture
{
    /// <summary>
    /// Factory Object used to create <see cref="IQuery{T}"/> objects
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Instance of Create MaloCulture Command
        /// </summary>
        ICommand<MaloCultureDto> CreateMaloCulturesCommand();               

        /// <summary>
        /// Create a MaloCulture Query
        /// </summary>
        /// <returns><see cref="IQuery{Code}"/></returns>
        IQuery<ICodeDto> CreateBrandsQuery();

        /// <summary>
        /// Create a MaloCulture Query
        /// </summary>
        /// <returns><see cref="IQuery{Code}"/></returns>
        IQuery<ICodeDto> CreateStylesQuery();

        /// <summary>
        /// Create a MaloCulture Query
        /// </summary>
        /// <returns><see cref="IQuery{MaloCultureDto}"/></returns>
        IQuery<MaloCultureDto> CreateMaloCulturesQuery();

      
    }
}
