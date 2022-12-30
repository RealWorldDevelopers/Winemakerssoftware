

namespace WMS.Data.CosmosDB
{
   public static class OperationErrorDictionary
   {
      public static class CarReservation
      {
         public static OperationError CarAlreadyReserved() =>
            new OperationError("Unfortunately the car was already reserved by another client in this specific term.");

         public static OperationError CarDoesNotExist() =>
            new OperationError("Unfortunately the car specified in the reservation does not exist in out catalog.");
      }
   }

   public record OperationError
   {
      public string Details { get; }

      public OperationError(string details) => (Details) = (details);

   }

   public class OperationResponse
   {
      protected bool _forcedFailedResponse;

      public bool CompletedWithSuccess => OperationError == null && !_forcedFailedResponse;

      public OperationError OperationError { get; set; }

      public OperationResponse SetAsFailureResponse(OperationError operationError)
      {
         OperationError = operationError;
         _forcedFailedResponse = true;
         return this;
      }

   }

   public class OperationResponse<T> : OperationResponse
   {
      public OperationResponse() { }

      public OperationResponse(T result)
      {
         Result = result;
      }

      public T Result { get; set; }

      public new OperationResponse<T> SetAsFailureResponse(OperationError operationError)
      {
         base.SetAsFailureResponse(operationError);
         return this;
      }

   }

}
