using System;
using System.Threading.Tasks;
using WMS.Data.CosmosDB;
using WMS.Data.CosmosDB.Entities;
using WMS.Data.CosmosDB.Interfaces;

namespace WMS.Business.CarReservation.Queries
{
   public interface ICarReservations
   {
      Task<OperationResponse<Data.CosmosDB.Entities.CarReservation>> MakeReservationAsync(Data.CosmosDB.Entities.CarReservation carReservation);
   }

   public class CarReservations : ICarReservations
   {
      private readonly ICarReservationRepository _carReservationRepository;
      private readonly IDataRepository<YeastBrand> _carRepository;
      private readonly IIdentityService _identityService;

      public CarReservations(ICarReservationRepository carReservationRepository,
                                   IDataRepository<YeastBrand> carRepository,
                                   IIdentityService identityService)
      {

         _carReservationRepository = carReservationRepository
                                    ?? throw new ArgumentNullException(nameof(carReservationRepository));


         _carRepository = carRepository
                                    ?? throw new ArgumentNullException(nameof(carRepository));

         _identityService = identityService
                                    ?? throw new ArgumentNullException(nameof(identityService));
      }

      public async Task<OperationResponse<Data.CosmosDB.Entities.CarReservation>> MakeReservationAsync(Data.CosmosDB.Entities.CarReservation carReservation)
      {
         var carFromReservation = await _carRepository.GetAsync(carReservation.CarId);
         if (carFromReservation == null)
         {
            return new OperationResponse<Data.CosmosDB.Entities.CarReservation>()
                                   .SetAsFailureResponse(OperationErrorDictionary.CarReservation.CarDoesNotExist());
         }

         var existingCarReservation = await _carReservationRepository.GetExistingReservationByCarIdAsync(carReservation.CarId, carReservation.RentFrom);

         if (existingCarReservation != null)
         {
            return new OperationResponse<Data.CosmosDB.Entities.CarReservation>()
                                   .SetAsFailureResponse(OperationErrorDictionary.CarReservation.CarAlreadyReserved());
         }

         else
         {
            carReservation.Id = Guid.NewGuid().ToString();
            carReservation.CustomerId = _identityService.GetUserIdentity().ToString();
            var createdCarReservation = await _carReservationRepository.AddAsync(carReservation);
            return new OperationResponse<Data.CosmosDB.Entities.CarReservation>(createdCarReservation);
         }
      }

   }

}
