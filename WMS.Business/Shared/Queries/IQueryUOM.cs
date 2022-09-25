using System.Collections.Generic;
using System.Threading.Tasks;

namespace WMS.Business.Common.Queries
{
    public interface IQueryUOM
    {
        /// <summary>
        /// Get All Units of Measure By Types
        /// </summary>
        /// <param name="uomType">Subset of UOM as <see cref="string"/></param>
        /// <returns><see cref="List{UnitOfMeasureDto}}"/></returns>
        Task<List<UnitOfMeasureDto>> ExecuteAsync(string uomType);

        /// <summary>
        /// Asynchronously query SQL DB by Primary Key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns>Ratings as <see cref="Task{UnitOfMeasureDto}"/></returns>
        Task<UnitOfMeasureDto> ExecuteAsync(int id);

    }
}