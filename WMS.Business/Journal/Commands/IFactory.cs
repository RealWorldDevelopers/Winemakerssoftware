using WMS.Business.Common;
using WMS.Business.Journal.Dto;

namespace WMS.Business.Journal.Commands
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

   }
}
