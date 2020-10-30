
using System;
using WMS.Business.Common;

namespace WMS.Business.Journal.Dto
{
   /// <summary>
   /// Instance of <see cref="Dto"/> Factory
   /// </summary>
   /// <inheritdoc cref="IFactory"/>>
   public class Factory : IFactory
   {
      /// <inheritdoc cref="IFactory.CreateNewBatch"/>>
      public BatchDto CreateNewBatch(int? id, string title, string description, double? volume, IUnitOfMeasure volumeUom,
          string submittedBy, int? vintage, ICode variety, TargetDto target, int? recipeId, bool? complete)
      {
         var dto = new BatchDto
         {
            Id = id,
            Title = title,
            Description = description,
            Volume = volume,
            VolumeUom = volumeUom,
            SubmittedBy = submittedBy,
            Vintage = vintage,
            Variety = variety,
            Target = target,
            RecipeId = recipeId,
            Complete = complete
         };

         return dto;
      }

      /// <inheritdoc cref="IFactory.CreateNewTarget"/>>
      public TargetDto CreateNewTarget(int? id, double? temp, IUnitOfMeasure tempUom, double? pH, double? ta,
         double? startSugar, IUnitOfMeasure startSugarUom, double? endSugar, IUnitOfMeasure endSugarUom)
      {
         var dto = new TargetDto
         {
            Id = id,
            Temp = temp,
            TempUom = tempUom,
            pH = pH,
            TA = ta,
            StartSugar = startSugar,
            StartSugarUom = startSugarUom,
            EndSugar = endSugar,
            EndSugarUom = endSugarUom
         };

         return dto;
      }

      /// <inheritdoc cref="IFactory.CreateNewBatchEntry"/>>
      public BatchEntryDto CreateNewBatchEntry(int? id, int? batchId, DateTime? entryDateTime, DateTime? actionDateTime,
         double? so2, double? temp, IUnitOfMeasure tempUom, double? pH, double? ta, double? sugar, IUnitOfMeasure sugarUom, 
         string additions, string comments, bool? racked, bool? filtered, bool? bottled)
      {
         var dto = new BatchEntryDto
         {
            Id = id,
            BatchId = batchId,
            EntryDateTime = entryDateTime,
            ActionDateTime = actionDateTime,
            Temp = temp,
            TempUom = tempUom,
            pH = pH,
            Sugar = sugar,
            SugarUom = sugarUom,
            Ta = ta,
            So2 = so2,
            Additions = additions,
            Comments = comments,
            Racked = racked,
            Filtered = filtered,
            Bottled = bottled
         };

         return dto;
      }
   }
}
