
using Microsoft.Azure.Cosmos;
using Serilog;
using System.Net;
using WMS.Data.CosmosDB.Entities;
using WMS.Data.CosmosDB.Interfaces;

namespace WMS.Data.CosmosDB.Data
{
   public abstract class CosmosDbDataRepository<T> : IDataRepository<T> where T : BaseEntity
   {
      protected readonly ICosmosDbConfiguration _cosmosDbConfiguration;
      protected readonly CosmosClient _client;

      public abstract string ContainerName { get; }

      public CosmosDbDataRepository(ICosmosDbConfiguration cosmosDbConfiguration, CosmosClient client)
      {
         _cosmosDbConfiguration = cosmosDbConfiguration ??
            throw new ArgumentNullException(nameof(cosmosDbConfiguration));

         _client = client ??
            throw new ArgumentNullException(nameof(client));
      }

      public async Task<T> AddAsync(T newEntity)
      {
         try
         {
            Container container = GetContainer();
            ItemResponse<T> createResponse = await container.CreateItemAsync(newEntity);
            return createResponse.Resource;
         }
         catch (CosmosException ex)
         {
            Log.Error($"New entity with ID: {newEntity.Id} was not added successfully - error details: {ex.Message}");

            if (ex.StatusCode != HttpStatusCode.NotFound)
            {
               throw;
            }

            return null;
         }
      }

      public async Task DeleteAsync(string entityId)
      {
         try
         {
            Container container = GetContainer();

            await container.DeleteItemAsync<T>(entityId, new PartitionKey(entityId));
         }
         catch (CosmosException ex)
         {
            Log.Error($"Entity with ID: {entityId} was not removed successfully - error details: {ex.Message}");

            if (ex.StatusCode != HttpStatusCode.NotFound)
            {
               throw;
            }
         }
      }

      public async Task<T> GetAsync(string entityId)
      {
         try
         {
            Container container = GetContainer();

            ItemResponse<T> entityResult = await container.ReadItemAsync<T>(entityId, new PartitionKey(entityId));
            return entityResult.Resource;
         }
         catch (CosmosException ex)
         {
            Log.Error($"Entity with ID: {entityId} was not retrieved successfully - error details: {ex.Message}");

            if (ex.StatusCode != HttpStatusCode.NotFound)
            {
               throw;
            }

            return null;
         }
      }

      public async Task<T> UpdateAsync(T entity)
      {
         try
         {
            Container container = GetContainer();
            ItemResponse<T> entityResult = await container
               .ReadItemAsync<T>(entity.Id.ToString(), new PartitionKey(entity.Id.ToString()));

            if (entityResult != null)
            {
               await container
                     .ReplaceItemAsync(entity, entity.Id.ToString(), new PartitionKey(entity.Id.ToString()));
            }
            return entity;
         }
         catch (CosmosException ex)
         {
            Log.Error($"Entity with ID: {entity.Id} was not updated successfully - error details: {ex.Message}");

            if (ex.StatusCode != HttpStatusCode.NotFound)
            {
               throw;
            }

            return null;
         }
      }

      public async Task<IReadOnlyList<T>> GetAllAsync()
      {
         try
         {
            Container container = GetContainer();
            List<T> entities = new List<T>();

            //FeedIterator<T> queryResultSetIterator = container.GetItemQueryIterator<T>();
            //await foreach (var entity in queryResultSetIterator)
            //{
            //   entities.Add(entity);
            //}

            using (FeedIterator<T> queryResultSetIterator = container.GetItemQueryIterator<T>())
            {
               //Asynchronous query execution
               while (queryResultSetIterator.HasMoreResults)
               {
                  foreach (var entity in await queryResultSetIterator.ReadNextAsync())
                  {
                     entities.Add(entity);
                  }
               }
            }

            return entities;

         }
         catch (CosmosException ex)
         {
            Log.Error($"Entities was not retrieved successfully - error details: {ex.Message}");

            if (ex.StatusCode != HttpStatusCode.NotFound)
            {
               throw;
            }

            return null;
         }
      }


      protected Container GetContainer()
      {
         var database = _client.GetDatabase(_cosmosDbConfiguration.DatabaseName);
         var container = database.GetContainer(ContainerName);
         return container;
      }

   }
}
