using WMS.Business.Common;
using WMS.Business.Yeast.Dto;

namespace WMS.Business.Yeast
{
    /// <summary>
    /// Factory Object used to create <see cref="IQuery{T}"/> objects
    /// </summary>
    public interface IFactory
    {

        ICommand<YeastPairDto> CreateYeastPairCommand();

        ICommand<YeastDto> CreateYeastsCommand();

        CodeDto CreateNewCode(int id, int parentId, string literal);

        YeastDto CreateNewYeast(int id, CodeDto brand, CodeDto style, string trademark, int? tempMin, int? tempMax, double? alcohol, string note);


        /// <summary>
        /// Create a Yeast Query
        /// </summary>
        /// <returns><see cref="IQuery{Code}"/></returns>
        IQuery<ICodeDto> CreateBrandsQuery();

        /// <summary>
        /// Create a Yeast Query
        /// </summary>
        /// <returns><see cref="IQuery{Code}"/></returns>
        IQuery<ICodeDto> CreateStylesQuery();

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