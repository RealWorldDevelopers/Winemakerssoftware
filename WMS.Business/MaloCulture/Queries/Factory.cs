using AutoMapper;
using WMS.Business.Common;
using WMS.Data;

namespace WMS.Business.MaloCulture.Queries
{

   public class Factory:IFactory
   {
      private readonly WMSContext _maloCultureContext;
      private readonly IMapper _mapper;

      /// <summary>
      /// Query Factory Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">Auto Mapper Instance as <see cref="IMapper"/></param>
      public Factory(WMSContext dbContext, IMapper mapper)
      {
         _maloCultureContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Instance of Create MaloCulture Query
      /// </summary>
      /// <inheritdoc cref="IFactory.CreateVarietiesQuery"/>>
      public IQuery<Dto.MaloCultureDto> CreateMaloCulturesQuery()
      {
         return new GetMaloCultures(_maloCultureContext, _mapper);
      }
            

      /// <summary>
      /// Instance of Create Brands Query
      /// </summary>
      /// <returns></returns>
      public IQuery<ICode> CreateBrandsQuery()
      {
         return new GetBrands(_maloCultureContext, _mapper);
      }

      /// <summary>
      /// Instance of Create Styles Query
      /// </summary>
      /// <returns></returns>
      public IQuery<ICode> CreateStylesQuery()
      {
         return new GetStyles(_maloCultureContext, _mapper);
      }
   }
}
