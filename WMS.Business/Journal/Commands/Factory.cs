using AutoMapper;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Data;

namespace WMS.Business.Journal.Commands
{

   public class Factory: IFactory
   {

      private readonly WMSContext _journalContext;
      private readonly IMapper _mapper;

      /// <summary>
      /// Command Factory Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public Factory(WMSContext dbContext, IMapper mapper)
      {
         _journalContext = dbContext;
         _mapper = mapper;
      }

      public ICommand<BatchEntryDto> CreateBatchEntriesCommand()
      {
         return new ModifyBatchEntry(_journalContext, _mapper);
      }

      /// <inheritdoc cref="IFactory.CreateBatchesCommand"/>>
      public ICommand<BatchDto> CreateBatchesCommand()
      {
         return new ModifyBatch(_journalContext, _mapper);
      }

      /// <inheritdoc cref="IFactory.CreateTargetsCommand"/>>
      public ICommand<TargetDto> CreateTargetsCommand()
      {
         return new ModifyTarget(_journalContext, _mapper);
      }


   }
}
