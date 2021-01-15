using AutoMapper;
using WMS.Business.Common;
using WMS.Data;

namespace WMS.Business.MaloCulture.Commands
{
   public class Factory : IFactory
   {
      private readonly WMSContext _maloCultureContext;
      private readonly IMapper _mapper;

      /// <summary>
      /// MaloCulture Factory Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public Factory(WMSContext dbContext, IMapper mapper)
      {
         _maloCultureContext = dbContext;
         _mapper = mapper;
      }


      /// <summary>
      /// Instance of Create MaloCulture Command
      /// </summary>
      /// <inheritdoc cref="IFactory.CreateMaloCulturesCommand"/>>
      public ICommand<Dto.MaloCultureDto> CreateMaloCulturesCommand()
      {
         return new ModifyMaloCulture(_maloCultureContext, _mapper);
      }


   }
}
