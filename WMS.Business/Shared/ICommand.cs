using System.Threading.Tasks;

namespace WMS.Business.Common
{
    /// <summary>
    /// Generic Entity Framework Command for Adding or Updating SQL DB Entities
    /// </summary>
    public interface ICommand<T>
    {
        /// <summary>
        /// Asynchronously add Data to SQL DB Entity
        /// </summary>
        Task<T> Add(T dto);

        /// <summary>
        /// Asynchronously update Data in a SQL DB Entity
        /// </summary>
        Task<T> Update(T dto);

        /// <summary>
        /// Asynchronously delete Data in a SQL DB Entity
        /// </summary>
        Task Delete(int id);

    }
}
