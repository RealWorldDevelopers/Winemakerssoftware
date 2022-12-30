
using WMS.Data.CosmosDB.Entities;

namespace WMS.Data.CosmosDB.Interfaces
{
   public interface ICarReservationRepository : IDataRepository<CarReservation>
   {
      Task<CarReservation> GetExistingReservationByCarIdAsync(string carId, DateTime rentFrom);
   }

}
