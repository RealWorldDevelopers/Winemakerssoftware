using Azure;
using Microsoft.Azure.Cosmos;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WMS.Data.CosmosDB.Entities;
using WMS.Data.CosmosDB.Interfaces;
using static WMS.Data.CosmosDB.OperationErrorDictionary;

namespace WMS.Data.CosmosDB.Data
{
   public class CarReservationRepository : CosmosDbDataRepository<Entities.CarReservation>, ICarReservationRepository
   {
      public CarReservationRepository(ICosmosDbConfiguration cosmosDbConfiguration,
               CosmosClient client) : base(cosmosDbConfiguration, client)
      {
      }

      public override string ContainerName => _cosmosDbConfiguration.CarReservationContainerName;

      public async Task<Entities.CarReservation> GetExistingReservationByCarIdAsync(string carId, DateTime rentFrom)
      {
         try
         {
            Container container = GetContainer();
            var entities = new List<Entities.CarReservation>();
            QueryDefinition queryDefinition = new QueryDefinition("select * from c where c.rentTo > @rentFrom AND c.carId = @carId")
                .WithParameter("@rentFrom", rentFrom)
                .WithParameter("@carId", carId);

            //AsyncPageable<Entities.CarReservation> queryResultSetIterator = container.GetItemQueryIterator<CarReservation>(queryDefinition);

            //await foreach (Entities.CarReservation carReservation in queryResultSetIterator)
            //{
            //   entities.Add(carReservation);
            //}

            using (FeedIterator<Entities.CarReservation> queryResultSetIterator = container.GetItemQueryIterator<Entities.CarReservation>())
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

            return entities.FirstOrDefault();
         }
         catch (CosmosException ex)
         {
            Log.Error($"Entity with ID: {carId} was not retrieved successfully - error details: {ex.Message}");

            if (ex.StatusCode != HttpStatusCode.NotFound)
            {
               throw;
            }

            return null;
         }
      }

   }
}
