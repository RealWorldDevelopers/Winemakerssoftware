using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Data.CosmosDB.Interfaces;

namespace WMS.Data.CosmosDB.Configuration
{
   // TODO 
   public class CosmosDbConfiguration : ICosmosDbConfiguration
   {
      public string ConnectionString { get; set; }
      public string DatabaseName { get; set; }
      //public string CarContainerName { get; set; }
      //public string CarContainerPartitionKeyPath { get; set; }
      //public string EnquiryContainerName { get; set; }
      //public string EnquiryContainerPartitionKeyPath { get; set; }
      //public string CarReservationContainerName { get; set; }
      //public string CarReservationPartitionKeyPath { get; set; }
      public string YeastBrandContainerName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public string YeastBrandContainerPartitionKeyPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
   }

}
