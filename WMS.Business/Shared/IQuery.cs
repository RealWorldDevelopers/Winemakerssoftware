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
      /// Query all Data in SQL DB
      /// </summary>
      List<T> Execute();

      /// <summary>
      /// Query a specific record in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      T Execute(int id);

      /// <summary>
      /// Query a specific record in SQL DB by foreign key
      /// </summary>
      /// <param name="fk">Foreign Key as <see cref="int"/></param>
      List<T> ExecuteByFK(int fk);

      /// <summary>
      /// Query a specific record in SQL DB by User ID
      /// </summary>
      /// <param name="userId">Foreign Key as <see cref="string"/></param>
      List<T> ExecuteByUser(string userId);

      /// <summary>
      /// Asynchronously query all Data in SQL DB
      /// </summary>
      Task<List<T>> ExecuteAsync();

      /// <summary>
      /// Asynchronously query a specific record in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      Task<T> ExecuteAsync(int id);

      /// <summary>
      /// Asynchronously query a specific record in SQL DB by foreign key
      /// </summary>
      /// <param name="fk">Foreign Key as <see cref="int"/></param>
      Task<List<T>> ExecuteByFKAsync(int fk);

      /// <summary>
      /// Asynchronously query a specific record in SQL DB by User ID
      /// </summary>
      /// <param name="userId">User Id as <see cref="string"/></param>
      Task<List<T>> ExecuteByUserAsync(string userId);
   }
}
