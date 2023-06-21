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

   public class CarReservationRepository : CosmosDbDataRepository<YeastBrand>
   {
      public CarReservationRepository(ICosmosDbConfiguration cosmosDbConfiguration, CosmosClient client) :
         base(cosmosDbConfiguration, client)
      {
      }

      public override string ContainerName => "_cosmosDbConfiguration.CarReservationContainerName";


   }
}
