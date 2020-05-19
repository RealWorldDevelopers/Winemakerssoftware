﻿using WMS.Business.Common;

namespace WMS.Business.Journal.Dto
{
   /// <summary>
   /// Factory Object used to create <see cref="Dto"/> objects
   /// </summary>
   public interface IFactory
   {
      /// <summary>
      /// Creates a New <see cref="BatchDto"/> Instance
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <param name="title">Title of Batch</param>
      /// <param name="description">Description of Batch</param>
      /// <param name="volume">Volume of Batch</param>
      /// <param name="volumeUom">Foreign Key to a Unit of Measure for Volume</param>
      /// <param name="submittedBy">User that Submitted Batch</param>
      /// <param name="vintage">Vintage of Batch</param>
      /// <param name="variety">Foreign Key to a Variety</param>
      /// <param name="target">Foreign Key to a Target Set</param>
      /// <param name="recipeId">Foreign Key to a Recipe</param>
      /// <param name="complete">Is Batch complete</param>
      /// <returns></returns>
      BatchDto CreateNewBatch(int? id, string title, string description, double? volume,
         IUnitOfMeasure volumeUom, string submittedBy, int? vintage, ICode variety, TargetDto target,
         int? recipeId, bool? complete);



      /// <summary>
      /// Creates a New <see cref="TargetDto"/> Instance
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <param name="temp">Temperature Value</param>
      /// <param name="tempUom">Foreign Key to a Unit of Measure for Temp</param>
      /// <param name="pH">pH Value</param>
      /// <param name="ta">Total Acid Value</param>
      /// <param name="startSugar">Starting Sugar Value</param>
      /// <param name="startSugarUom">Foreign Key to a Unit of Measure for Starting Sugar</param>
      /// <param name="endSugar">Ending Sugar Value</param>
      /// <param name="endSugarUom">Foreign Key to a Unit of Measure for Ending Sugar</param>
      /// <returns></returns>
      TargetDto CreateNewTarget(int? id, double? temp, IUnitOfMeasure tempUom, double? pH, double? ta,
         double? startSugar, IUnitOfMeasure startSugarUom, double? endSugar, IUnitOfMeasure endSugarUom);

   }
}