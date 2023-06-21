using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Data.CosmosDB.Configuration
{
   // TODO 
   public class CosmosDbConfigurationValidation : IValidateOptions<CosmosDbConfiguration>
   {
      public ValidateOptionsResult Validate(string name, CosmosDbConfiguration options)
      {
         if (string.IsNullOrEmpty(options.ConnectionString))
         {
            return ValidateOptionsResult.Fail($"{nameof(options.ConnectionString)} configuration parameter for the Azure Cosmos DB is required");
         }

         if (string.IsNullOrEmpty(options.YeastBrandContainerName))
         {
            return ValidateOptionsResult.Fail($"{nameof(options.YeastBrandContainerName)} configuration parameter for the Azure Cosmos DB is required");
         }

         //if (string.IsNullOrEmpty(options.EnquiryContainerName))
         //{
         //   return ValidateOptionsResult.Fail($"{nameof(options.EnquiryContainerName)} configuration parameter for the Azure Cosmos DB is required");
         //}

         //if (string.IsNullOrEmpty(options.CarReservationContainerName))
         //{
         //   return ValidateOptionsResult.Fail($"{nameof(options.CarReservationContainerName)} configuration parameter for the Azure Cosmos DB is required");
         //}

         if (string.IsNullOrEmpty(options.DatabaseName))
         {
            return ValidateOptionsResult.Fail($"{nameof(options.DatabaseName)} configuration parameter for the Azure Cosmos DB is required");
         }

         if (string.IsNullOrEmpty(options.YeastBrandContainerPartitionKeyPath))
         {
            return ValidateOptionsResult.Fail($"{nameof(options.YeastBrandContainerPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
         }

         //if (string.IsNullOrEmpty(options.EnquiryContainerPartitionKeyPath))
         //{
         //   return ValidateOptionsResult.Fail($"{nameof(options.EnquiryContainerPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
         //}

         //if (string.IsNullOrEmpty(options.CarReservationPartitionKeyPath))
         //{
         //   return ValidateOptionsResult.Fail($"{nameof(options.CarReservationPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
         //}

         return ValidateOptionsResult.Success;
      }
   }

}
