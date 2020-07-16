using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Business.Recipe.Dto;
using WMS.Business.Yeast.Dto;
using WMS.Data;

namespace WMS.Business.Recipe.Queries
{
   /// <summary>
   /// Recipe Query Instance
   /// </summary>
   /// <inheritdoc cref="IQuery{T}"/>
   public class GetRecipes : IQuery<Dto.RecipeDto>
   {
      private readonly IMapper _mapper;
      private readonly WMSContext _dbContext;

      /// <summary>
      /// Recipe Query Constructor
      /// </summary>
      /// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      /// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      public GetRecipes(WMSContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Query all Recipes in SQL DB
      /// </summary>
      /// <returns>Recipes as <see cref="List{RecipeDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute()"/>
      public List<RecipeDto> Execute()
      {
         // NOTE: Not loading images on purpose to avoid slowing down first page loading
         // NOTE: Not loading target on purpose to avoid slowing down first page loading

         var recipes = _dbContext.Recipes.Include("PicturesXref").Include("Ratings").ToList();
         var list = _mapper.Map<List<RecipeDto>>(recipes);
         var categories = _dbContext.Categories.ToList();
         var varieties = _dbContext.Varieties.ToList();
         var yeasts = _dbContext.Yeasts.ToList();
         var targets = _dbContext.Targets.ToList();

         foreach (var item in list)
         {
            if (item.Variety != null)
            {
               var code = varieties.SingleOrDefault(a => a.Id == item.Variety.Id);
               item.Variety.Literal = code.Variety;
               var cat = _dbContext.Categories.SingleOrDefault(a => a.Id == code.CategoryId);
               item.Category = _mapper.Map<ICode>(cat);
            }
            if (item.Yeast != null)
            {
               var y = _dbContext.Yeasts.SingleOrDefault(a => a.Id == item.Yeast.Id);
               item.Yeast = _mapper.Map<YeastDto>(y);
            }
            if (item.Target?.Id.HasValue == true)
            {
               var t = targets.SingleOrDefault(t => t.Id == item.Target.Id);
               item.Target = _mapper.Map<TargetDto>(t);
            }
         }

         return list;
      }

      /// <summary>
      /// Query a specific Recipe in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Recipe as <see cref="RecipeDto"/></returns>
      /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
      public RecipeDto Execute(int id)
      {
         var recipe = _dbContext.Recipes
          .Include("PicturesXref")
          .Include("Ratings")
          .FirstOrDefault(r => r.Id == id);

         var dto = _mapper.Map<RecipeDto>(recipe);

         var imgs = recipe.PicturesXref.Where(p => p.RecipeId == id).ToList();
         var dtoList = _mapper.Map<List<ImageFileDto>>(imgs);
         dto.ImageFiles.Clear();
         dto.ImageFiles.AddRange(dtoList);

         if (dto.Variety != null)
         {
            var code = _dbContext.Varieties.SingleOrDefault(a => a.Id == dto.Variety.Id);
            dto.Variety.Literal = code.Variety;
            dto.Category = _dbContext.Categories.ProjectTo<ICode>(_mapper.ConfigurationProvider).SingleOrDefault(a => a.Id == code.CategoryId);
         }

         if (dto.Yeast != null)
         {
            var yeast = _dbContext.Yeasts.SingleOrDefault(a => a.Id == dto.Yeast.Id);
            dto.Yeast = _mapper.Map<YeastDto>(yeast);
            dto.Yeast.Brand = _dbContext.YeastBrand.ProjectTo<ICode>(_mapper.ConfigurationProvider)
              .SingleOrDefault(a => a.Id == dto.Yeast.Brand.Id);
         }

         if (dto.Target != null)
         {
            var target = _dbContext.Targets.SingleOrDefault(a => a.Id == dto.Target.Id);
            dto.Target = _mapper.Map<TargetDto>(target);
         }

         return dto;
      }

      /// <summary>
      /// Asynchronously query all Recipes in SQL DB
      /// </summary>
      /// <returns>Recipes as <see cref="Task{List{RecipeDto}}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
      public async Task<List<RecipeDto>> ExecuteAsync()
      {
         // NOTE: Not loading images on purpose to avoid slowing down first page loading
         // NOTE: Not loading target on purpose to avoid slowing down first page loading

         // using TPL to parallel call gets
         List<Task> tasks = new List<Task>();
         var t1 = Task.Run(async () =>
            await _dbContext.Recipes.Include("PicturesXref").Include("Ratings").ToListAsync().ConfigureAwait(false));
         tasks.Add(t1);
         var list = _mapper.Map<List<RecipeDto>>(await t1.ConfigureAwait(false));

         var t2 = Task.Run(async () => await _dbContext.Categories.ToListAsync().ConfigureAwait(false));
         tasks.Add(t2);
         var categories = await t2.ConfigureAwait(false);

         var t3 = Task.Run(async () => await _dbContext.Varieties.ToListAsync().ConfigureAwait(false));
         tasks.Add(t3);
         var varieties = await t3.ConfigureAwait(false);

         var t4 = Task.Run(async () => await _dbContext.Yeasts.ToListAsync().ConfigureAwait(false));
         tasks.Add(t4);
         var yeasts = await t4.ConfigureAwait(false);

         var t5 = Task.Run(async () => await _dbContext.Targets.ToListAsync().ConfigureAwait(false));
         tasks.Add(t5);
         var targets = await t5.ConfigureAwait(false);

         Task.WaitAll(tasks.ToArray());

         foreach (var item in list)
         {
            if (item.Variety != null)
            {
               var code = varieties.SingleOrDefault(a => a.Id == item.Variety.Id);
               item.Variety.Literal = code.Variety;
               var cat = categories.SingleOrDefault(a => a.Id == code.CategoryId);
               item.Category = _mapper.Map<ICode>(cat);
            }
            if (item.Yeast != null)
            {
               var y = yeasts.SingleOrDefault(a => a.Id == item.Yeast.Id);
               item.Yeast = _mapper.Map<YeastDto>(y);
            }
            if (item.Target?.Id.HasValue == true)
            {
               var t = targets.SingleOrDefault(t => t.Id == item.Target.Id);
               item.Target = _mapper.Map<TargetDto>(t);
            }

         }

         return list;
      }

      /// <summary>
      /// Asynchronously query a specific Recipe in SQL DB by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns>Recipe as <see cref="Task{RecipeDto}"/></returns>
      /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
      public async Task<RecipeDto> ExecuteAsync(int id)
      {
         var recipe = await _dbContext.Recipes
             .Include("PicturesXref")
             .Include("Ratings")
             .FirstOrDefaultAsync(r => r.Id == id).ConfigureAwait(false);

         var dto = _mapper.Map<RecipeDto>(recipe);

         var img = recipe.PicturesXref.Where(p => p.RecipeId == id).ToList();
         var dtoList = _mapper.Map<List<ImageFileDto>>(img);
         dto.ImageFiles.Clear();
         dto.ImageFiles.AddRange(dtoList);

         if (dto.Variety != null)
         {
            var code = await _dbContext.Varieties.SingleOrDefaultAsync(a => a.Id == dto.Variety.Id).ConfigureAwait(false);
            dto.Variety.Literal = code.Variety;
            dto.Category = await _dbContext.Categories.ProjectTo<ICode>(_mapper.ConfigurationProvider)
               .SingleOrDefaultAsync(a => a.Id == code.CategoryId).ConfigureAwait(false);
         }

         if (dto.Yeast != null)
         {
            var yeast = await _dbContext.Yeasts.SingleOrDefaultAsync(a => a.Id == dto.Yeast.Id).ConfigureAwait(false);
            dto.Yeast = _mapper.Map<YeastDto>(yeast);
            dto.Yeast.Brand = await _dbContext.YeastBrand.ProjectTo<ICode>(_mapper.ConfigurationProvider)
               .SingleOrDefaultAsync(a => a.Id == dto.Yeast.Brand.Id).ConfigureAwait(false);
         }

         if (dto.Target != null)
         {
            var target = await _dbContext.Targets.SingleOrDefaultAsync(a => a.Id == dto.Target.Id).ConfigureAwait(false);
            dto.Target = _mapper.Map<TargetDto>(target);

            if (dto.Target.StartSugarUom != null)
               dto.Target.StartSugarUom = await _dbContext.UnitsOfMeasure.ProjectTo<UnitOfMeasure>(_mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync(m => m.Id == dto.Target.StartSugarUom.Id).ConfigureAwait(false);
            if (dto.Target.TempUom != null)
               dto.Target.TempUom = await _dbContext.UnitsOfMeasure.ProjectTo<UnitOfMeasure>(_mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync(m => m.Id == dto.Target.TempUom.Id).ConfigureAwait(false);
            if (dto.Target.EndSugarUom != null)
               dto.Target.EndSugarUom = await _dbContext.UnitsOfMeasure.ProjectTo<UnitOfMeasure>(_mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync(m => m.Id == dto.Target.EndSugarUom.Id).ConfigureAwait(false);
         }

         return dto;
      }

   }
}
