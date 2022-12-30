using Microsoft.Extensions.Options;
using WMS.Data.CosmosDB.Interfaces;

namespace WMS.Service.WebAPI.Extensions
{
   public static class ConfigurationServiceCollectionExtensions
   {
      public static IServiceCollection AddAppStorageConfiguration(this IServiceCollection services, IConfiguration config)
      {
         // TODO move SQL configues here as well

         //services.Configure<BlobStorageServiceConfiguration>(config.GetSection("BlobStorageSettings"));
         //services.AddSingleton<IValidateOptions<BlobStorageServiceConfiguration>, BlobStorageServiceConfigurationValidation>();
         //var blobStorageServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<BlobStorageServiceConfiguration>>().Value;
         //services.AddSingleton<IBlobStorageServiceConfiguration>(blobStorageServiceConfiguration);

         services.Configure<CosmosDbConfiguration>(config.GetSection("CosmosDbSettings"));
         services.AddSingleton<IValidateOptions<CosmosDbConfiguration>, CosmosDbConfigurationValidation>();
         var cosmosDbConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<CosmosDbConfiguration>>().Value;
         services.AddSingleton<ICosmosDbConfiguration>(cosmosDbConfiguration);


         //services.Configure<MessagingServiceConfiguration>(config.GetSection("ServiceBusSettings"));
         //services.AddSingleton<IValidateOptions<MessagingServiceConfiguration>, MessagingServiceConfigurationValidation>();
         //var messagingServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<MessagingServiceConfiguration>>().Value;
         //services.AddSingleton<IMessagingServiceConfiguration>(messagingServiceConfiguration);

         return services;
      }
   }

}
