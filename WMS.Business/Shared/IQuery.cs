﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace WMS.Business.Shared
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
        /// Asynchronously query all Data in SQL DB
        /// </summary>
        Task<List<T>> ExecuteAsync();

        /// <summary>
        /// Asynchronously query a specific record in SQL DB by primary key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        Task<T> ExecuteAsync(int id);
    }
}
