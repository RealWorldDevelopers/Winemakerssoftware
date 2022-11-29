

using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using WMS.Business.Common;
using WMS.Business.Journal.Commands;
using WMS.Business.Journal.Dto;
using WMS.Business.Journal.Queries;
using WMS.Data.SQL;

namespace WMS.Business.Journal
{

    public class Factory : IFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WMSContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Command Factory Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
        /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
        public Factory(IServiceProvider serviceProvider, WMSContext dbContext, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ICommand<BatchEntryDto> CreateBatchEntriesCommand()
        {
            return ActivatorUtilities.CreateInstance<ModifyBatchEntry>(_serviceProvider, _dbContext, _mapper);
        }

        /// <inheritdoc cref="IFactory.CreateBatchesCommand"/>>
        public ICommand<BatchDto> CreateBatchesCommand()
        {
            return ActivatorUtilities.CreateInstance<ModifyBatch>(_serviceProvider, _dbContext, _mapper);
        }

        /// <inheritdoc cref="IFactory.CreateTargetsCommand"/>>
        public ICommand<TargetDto> CreateTargetsCommand()
        {
            return ActivatorUtilities.CreateInstance<ModifyTarget>(_serviceProvider, _dbContext, _mapper);

        }

        /// <inheritdoc cref="IFactory.CreateNewBatch"/>>
        public BatchDto CreateNewBatch(int? id, string title, string description, double? volume, UnitOfMeasureDto volumeUom,
            string submittedBy, int? vintage, CodeDto variety, TargetDto target, int? recipeId, bool? complete)
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
                Complete = complete ?? false
            };

            return dto;
        }

        /// <inheritdoc cref="IFactory.CreateNewTarget"/>>
        public TargetDto CreateNewTarget(int? id, double? temp, UnitOfMeasureDto tempUom, double? pH, double? ta,
           double? startSugar, UnitOfMeasureDto startSugarUom, double? endSugar, UnitOfMeasureDto endSugarUom)
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
           double? so2, double? temp, UnitOfMeasureDto tempUom, double? pH, double? ta, double? sugar, UnitOfMeasureDto sugarUom,
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
                Racked = racked ?? false,
                Filtered = filtered ?? false,
                Bottled = bottled ?? false
            };

            return dto;
        }

        /// <summary>
        /// Instance of Create Batches Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateBatchesQuery"/>>
        public IQuery<BatchDto> CreateBatchesQuery()
        {
            return ActivatorUtilities.CreateInstance<GetBatches>(_serviceProvider, _dbContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Batch Entries Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateBatchEntriesQuery"/>>
        public IQuery<BatchEntryDto> CreateBatchEntriesQuery()
        {
            return ActivatorUtilities.CreateInstance<GetBatchEntries>(_serviceProvider, _dbContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Targets Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateTargetsQuery"/>>
        public IQuery<TargetDto> CreateTargetsQuery()
        {
            return ActivatorUtilities.CreateInstance<GetTargets>(_serviceProvider, _dbContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Units of Measure Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateBatchVolumeUOM"/>>
        public IQuery<IUnitOfMeasureDto> CreateBatchVolumeUOMQuery()
        {
            return ActivatorUtilities.CreateInstance<GetBatchVolumeUOM>(_serviceProvider, _dbContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Units of Measure Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateBatchVolumeUOM"/>>
        public IQuery<IUnitOfMeasureDto> CreateBatchTempUOMQuery()
        {
            return ActivatorUtilities.CreateInstance<GetBatchTempUOM>(_serviceProvider, _dbContext, _mapper);
        }

        /// <summary>
        /// Instance of Create Units of Measure Query
        /// </summary>
        /// <inheritdoc cref="IFactory.CreateBatchVolumeUOM"/>>
        public IQuery<IUnitOfMeasureDto> CreateBatchSugarUOMQuery()
        {
            return ActivatorUtilities.CreateInstance<GetBatchSugarUOM>(_serviceProvider, _dbContext, _mapper);
        }


    }

}
