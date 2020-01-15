using System.Threading.Tasks;

namespace WMS.Business.Common
{
    /// <summary>
    /// Generic Entity Framework Command for Adding or Updating SQL DB Entities
    /// </summary>
    public interface ICommand<T>
    {
        /// <summary>
        /// Add Data to SQL DB Entity
        /// </summary>
        T Add(T dto);

        /// <summary>
        /// Asynchronously add Data to SQL DB Entity
        /// </summary>
        Task<T> AddAsync(T dto);

        /// <summary>
        /// Update Data in a SQL DB Entity
        /// </summary>
        T Update(T dto);

        /// <summary>
        /// Asynchronously update Data in a SQL DB Entity
        /// </summary>
        Task<T> UpdateAsync(T dto);

        /// <summary>
        /// Delete Data in a SQL DB Entity
        /// </summary>
        void Delete(T dto);

        /// <summary>
        /// Asynchronously delete Data in a SQL DB Entity
        /// </summary>
        Task DeleteAsync(T dto);

    }
}
