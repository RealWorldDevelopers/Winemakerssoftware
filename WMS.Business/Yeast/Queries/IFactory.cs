using WMS.Business.Common;

namespace WMS.Business.Yeast.Queries
{
    /// <summary>
    /// Factory Object used to create <see cref="IQuery{T}"/> objects
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Create a Yeast Query
        /// </summary>
        /// <returns><see cref="IQuery{Code}"/></returns>
        IQuery<ICode> CreateBrandsQuery();

        /// <summary>
        /// Create a Yeast Query
        /// </summary>
        /// <returns><see cref="IQuery{Code}"/></returns>
        IQuery<ICode> CreateStylesQuery();

        /// <summary>
        /// Create a Yeast Query
        /// </summary>
        /// <returns><see cref="IQuery{Yeast}"/></returns>
        IQuery<Dto.YeastDto> CreateYeastsQuery();

        /// <summary>
        /// Create a Yeast Pair Query
        /// </summary>
        /// <returns><see cref="IQuery{YeastPair}"/></returns>
        IQuery<Dto.YeastPairDto> CreateYeastPairQuery();

    }
}