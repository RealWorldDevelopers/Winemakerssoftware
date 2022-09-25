using System.Collections.Generic;
using System.Threading.Tasks;

namespace WMS.Business.Common
{
    /// <summary>
    /// Generic Entity Framework Query for SQL DB Entities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQuery<T>
    {
        /// <summary>
        /// Asynchronously query all Data in SQL DB
        /// </summary>
        Task<List<T>> Execute();

        /// <summary>
        /// Asynchronously query a specific record in SQL DB by primary key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        Task<T> Execute(int id);

        /// <summary>
        /// Asynchronously query data in SQL DB by pagination
        /// </summary>
        /// <param name="start">Starting Record Number <see cref="int"/></param>
        /// <param name="length">Count of Records to Return <see cref="int"/></param>
        Task<List<T>> Execute(int start, int length);
        
        /// <summary>
        /// Asynchronously query a specific record in SQL DB by foreign key
        /// </summary>
        /// <param name="fk">Foreign Key as <see cref="int"/></param>
        Task<List<T>> ExecuteByFK(int fk);

        /// <summary>
        /// Asynchronously query a specific record in SQL DB by User ID
        /// </summary>
        /// <param name="userId">User Id as <see cref="string"/></param>
        Task<List<T>> ExecuteByUser(string userId);

    }
}
