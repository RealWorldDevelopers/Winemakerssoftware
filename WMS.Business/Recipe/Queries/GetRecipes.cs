﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Recipe.Dto;
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
         var dtoList = _dbContext.Recipes
            .Include("PicturesXref").Include("Ratings")
            .ProjectTo<RecipeDto>(_mapper.ConfigurationProvider).ToList();
         var categories = _dbContext.Categories.ToList();
         var varieties = _dbContext.Varieties.ToList();


         foreach (var item in dtoList)
         {
            if (item.Variety != null)
            {
               var code = varieties.SingleOrDefault(a => a.Id == item.Variety.Id);
               item.Variety.Literal = code.Variety;
               item.Category = _dbContext.Categories
                  .ProjectTo<ICode>(_mapper.ConfigurationProvider)
                  .SingleOrDefault(a => a.Id == code.CategoryId);
            }
         }

         return dtoList;
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
         var img = recipe.PicturesXref.Where(p => p.RecipeId == id).ToList();
         dto.ImageFiles.Clear();
         dto.ImageFiles.AddRange(_mapper.Map<List<ImageFileDto>>(img));

         if (dto.Variety != null)
         {
            var code = _dbContext.Varieties.SingleOrDefault(a => a.Id == dto.Variety.Id);
            dto.Variety.Literal = code.Variety;
            dto.Category = _dbContext.Categories
               .ProjectTo<ICode>(_mapper.ConfigurationProvider)
               .SingleOrDefault(a => a.Id == code.CategoryId);
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
         // using TPL to parallel call gets
         List<Task> tasks = new List<Task>();
         var t1 = Task.Run(async () =>
            await _dbContext.Recipes.Include("PicturesXref").Include("Ratings")
               .ProjectTo<RecipeDto>(_mapper.ConfigurationProvider)
               .ToListAsync().ConfigureAwait(false));
         tasks.Add(t1);
         var list = await t1.ConfigureAwait(false);

         var t2 = Task.Run(async () => await _dbContext.Categories.ToListAsync().ConfigureAwait(false));
         tasks.Add(t2);
         var categories = await t2.ConfigureAwait(false);

         var t3 = Task.Run(async () => await _dbContext.Varieties.ToListAsync().ConfigureAwait(false));
         tasks.Add(t3);
         var varieties = await t3.ConfigureAwait(false);

         Task.WaitAll(tasks.ToArray());

         foreach (var item in list)
         {
            if (item.Variety != null)
            {
               var code = varieties.SingleOrDefault(a => a.Id == item.Variety.Id);
               item.Variety.Literal = code.Variety;
               item.Category = await _dbContext.Categories
                  .ProjectTo<ICode>(_mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync(a => a.Id == code.CategoryId).ConfigureAwait(false);
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
         dto.ImageFiles.Clear();
         dto.ImageFiles.AddRange(_mapper.Map<List<ImageFileDto>>(img));

         if (dto.Variety != null)
         {
            var code = await _dbContext.Varieties.SingleOrDefaultAsync(a => a.Id == dto.Variety.Id).ConfigureAwait(false);
            dto.Variety.Literal = code.Variety;
            dto.Category = await _dbContext.Categories
                  .ProjectTo<ICode>(_mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync(a => a.Id == code.CategoryId)
                  .ConfigureAwait(false);
         }

         return dto;
      }

   }
}
