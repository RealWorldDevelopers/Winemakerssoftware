using WMS.Business.Common;
using WMS.Business.MaloCulture.Dto;

namespace WMS.Business.MaloCulture.Commands
{
   public interface IFactory
   {
      ICommand<MaloCultureDto> CreateMaloCulturesCommand();
   }
}