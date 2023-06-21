
namespace WMS.Data.CosmosDB.Interfaces
{
   
   public interface ICosmosDbConfiguration
   {
      string ConnectionString { get; set; }
      string DatabaseName { get; set; }

      // TODO 
      string YeastBrandContainerName { get; set; }
      string YeastBrandContainerPartitionKeyPath { get; set; }
      // string EnquiryContainerName { get; set; }
      // string EnquiryContainerPartitionKeyPath { get; set; }
      //string CarReservationContainerName { get; set; }
      //string CarReservationPartitionKeyPath { get; set; }

   }

}
