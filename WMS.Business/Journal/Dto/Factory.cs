
namespace WMS.Business.Journal.Dto
{
   /// <summary>
   /// Instance of <see cref="Dto"/> Factory
   /// </summary>
   /// <inheritdoc cref="IFactory"/>>
   public class Factory : IFactory
   {
      /// <inheritdoc cref="IFactory.CreateNewBatch"/>>
      public BatchDto CreateNewBatch(int? id, string title, string description, double? volume, int? volumeUomId,
          string submittedBy, int? vintage, int? varietyId, int? targetId, int? recipeId, bool? complete)
      {
         var dto = new BatchDto
         {
            Id = id,
            Title = title,
            Description = description,
            Volume = volume,
            VolumeUomId = volumeUomId,
            SubmittedBy = submittedBy,
            Vintage = vintage,
            VarietyId = varietyId,
            TargetId = targetId,
            RecipeId = recipeId,
            Complete = complete
         };

         return dto;
      }

      /// <inheritdoc cref="IFactory.CreateNewTarget"/>>
      public TargetDto CreateNewTarget(int? id, double? temp, int? tempUomId, double? pH, double? ta, 
         double? startSugar, int? startSugarUomId, double? endSugar, int? endSugarUomId)
      {
         var dto = new TargetDto
         {
            Id = id,
            Temp = temp,
            TempUomId = tempUomId,
            pH = pH,
            TA = ta,
            StartSugar = startSugar,
            StartSugarUomId = startSugarUomId,
            EndSugar = endSugar,
            EndSugarUomId = endSugarUomId
         };

         return dto;
      }

   }
}
