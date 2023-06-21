using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WMS.Data.CosmosDB.Data;
using WMS.Data.CosmosDB.Entities;
using WMS.Data.CosmosDB.Interfaces;

namespace WMS.Service.WebAPI.Extensions
{

   public static class DataServiceCollectionExtensions
   {
      public static IServiceCollection AddCosmosDBServices(this IServiceCollection services)
      {

         services.TryAddSingleton(implementationFactory =>
         {
            var cosmoDbConfiguration = implementationFactory.GetRequiredService<ICosmosDbConfiguration>();
            CosmosClient cosmosClient = new CosmosClient(cosmoDbConfiguration.ConnectionString);
            Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(cosmoDbConfiguration.DatabaseName).GetAwaiter().GetResult();

            // TODO
            database.CreateContainerIfNotExistsAsync(
                cosmoDbConfiguration.YeastBrandContainerName,
                cosmoDbConfiguration.YeastBrandContainerPartitionKeyPath, 400).GetAwaiter().GetResult();

            //database.CreateContainerIfNotExistsAsync(
            //    cosmoDbConfiguration.EnquiryContainerName,
            //    cosmoDbConfiguration.EnquiryContainerPartitionKeyPath, 400).GetAwaiter().GetResult();

            //database.CreateContainerIfNotExistsAsync(
            //    cosmoDbConfiguration.CarReservationContainerName,
            //    cosmoDbConfiguration.CarReservationPartitionKeyPath, 400).GetAwaiter().GetResult();

            return cosmosClient;
         });

         // TODO
         services.AddSingleton<IDataRepository<YeastBrand>, YeastBrandRepository>();
         //services.AddSingleton<IDataRepository<Enquiry>, EnquiryRepository>();
         //services.AddSingleton<ICarReservationRepository, CarReservationRepository>();
         //services.AddSingleton<ICarReservationService, CarReservationService>();

         return services;
      }
   }
}


