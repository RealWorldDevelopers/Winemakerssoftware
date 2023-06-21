using Microsoft.Azure.Cosmos;
using WMS.Data.CosmosDB.Entities;
using WMS.Data.CosmosDB.Interfaces;

namespace WMS.Data.CosmosDB.Data
{
   public class YeastBrandRepository : CosmosDbDataRepository<YeastBrand>
   {
      public YeastBrandRepository(ICosmosDbConfiguration cosmosDbConfiguration, CosmosClient client) : 
         base(cosmosDbConfiguration, client)
      {
      }

      public override string ContainerName => _cosmosDbConfiguration.YeastBrandContainerName;
   }
}
