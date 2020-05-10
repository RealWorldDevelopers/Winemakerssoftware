

using AutoMapper;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Data;

namespace WMS.Business.Journal.Commands
{

   public class Factory: IFactory
   {

      private readonly WMSContext _recipeContext;
      private readonly IMapper _mapper;

      /// <summary>
      /// Command Factory Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public Factory(WMSContext dbContext, IMapper mapper)
      {
         _recipeContext = dbContext;
         _mapper = mapper;
      }

      /// <inheritdoc cref="IFactory.CreateBatchesCommand"/>>
      public ICommand<BatchDto> CreateBatchesCommand()
      {
         return new ModifyBatch(_recipeContext, _mapper);
      }

      /// <inheritdoc cref="IFactory.CreateTargetsCommand"/>>
      public ICommand<TargetDto> CreateTargetsCommand()
      {
         return new ModifyTarget(_recipeContext, _mapper);
      }


   }
}
