using System;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;

namespace WMS.Business.Journal
{
    public interface IFactory
    {
        /// <summary>
        /// Create a <see cref="BatchDto"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{BatchDto}"/></returns>
        ICommand<BatchDto> CreateBatchesCommand();

        /// <summary>
        /// Create a <see cref="TargetDto"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{TargetDto}"/></returns>
        public ICommand<TargetDto> CreateTargetsCommand();

        /// <summary>
        /// Create a <see cref="BatchEntryDto"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{BatchEntryDto}"/></returns>
        ICommand<BatchEntryDto> CreateBatchEntriesCommand();

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
           UnitOfMeasureDto volumeUom, string submittedBy, int? vintage, CodeDto variety, TargetDto target,
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
        TargetDto CreateNewTarget(int? id, double? temp, UnitOfMeasureDto tempUom, double? pH, double? ta,
           double? startSugar, UnitOfMeasureDto startSugarUom, double? endSugar, UnitOfMeasureDto endSugarUom);

        /// <summary>
        /// Creates a New <see cref="BatchEntryDto"/> Instance
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <param name="batchId">Foreign Key as <see cref="int"/></param>
        /// <param name="entryDateTime">Database Transaction DateTime</param>
        /// <param name="actionDateTime">DateTime of Action</param>
        /// <param name="so2">SO2 Level at time of Action</param>
        /// <param name="temp">Temperature at time of Action</param>
        /// <param name="tempUom">Foreign Key to a Unit of Measure for Temp</param>
        /// <param name="pH">pH Level at time of Action</param>
        /// <param name="ta">TA Level at time of Action</param>
        /// <param name="sugar">Sugar Level at time of Action</param>
        /// <param name="sugarUom">Foreign Key to a Unit of Measure for Sugar</param>
        /// <param name="additions">Items Added at tome of Action</param>
        /// <param name="comments">Observation Notes</param>
        /// <param name="racked">Did Action include Racking</param>
        /// <param name="filtered">Did Action include Filtering</param>
        /// <param name="bottled">Did Action include Bottling</param>
        /// <returns></returns>
        BatchEntryDto CreateNewBatchEntry(int? id, int? batchId, DateTime? entryDateTime, DateTime? actionDateTime,
           double? so2, double? temp, UnitOfMeasureDto tempUom, double? pH, double? ta, double? sugar, UnitOfMeasureDto sugarUom,
           string additions, string comments, bool? racked, bool? filtered, bool? bottled);

        /// <summary>
        /// Create a Batches Query
        /// </summary>
        /// <returns><see cref="IQuery{BatchDto}"/></returns>
        IQuery<BatchDto> CreateBatchesQuery();

        /// <summary>
        /// Create a Batch Entries Query
        /// </summary>
        /// <returns><see cref="IQuery{BatchEntryDto}"/></returns>
        IQuery<BatchEntryDto> CreateBatchEntriesQuery();

        /// <summary>
        /// Create a Targets Query
        /// </summary>
        /// <returns><see cref="IQuery{TargetDto}"/></returns>
        IQuery<TargetDto> CreateTargetsQuery();

        /// <summary>
        /// Create a Batch Volume UOM Query
        /// </summary>
        /// <returns><see cref="IQuery{IUnitOfMeasure}"/></returns>
        IQuery<IUnitOfMeasureDto> CreateBatchVolumeUOMQuery();

        /// <summary>
        /// Create a Batch Volume UOM Query
        /// </summary>
        /// <returns><see cref="IQuery{IUnitOfMeasure}"/></returns>
        IQuery<IUnitOfMeasureDto> CreateBatchTempUOMQuery();

        /// <summary>
        /// Create a Batch Volume UOM Query
        /// </summary>
        /// <returns><see cref="IQuery{IUnitOfMeasure}"/></returns>
        IQuery<IUnitOfMeasureDto> CreateBatchSugarUOMQuery();

    }

}
