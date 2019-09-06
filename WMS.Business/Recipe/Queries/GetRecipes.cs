using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Shared;
using WMS.Data;

namespace WMS.Business.Recipe.Queries
{
    /// <summary>
    /// Recipe Query Instance
    /// </summary>
    /// <inheritdoc cref="IQuery{T}"/>
    public class GetRecipes : IQuery<Dto.Recipe>
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
        /// <returns>Recipes as <see cref="List{Dto.Recipe}"/></returns>
        /// <inheritdoc cref="IQuery{T}.Execute"/>
        public List<Dto.Recipe> Execute()
        {
            var recipes = _dbContext.Recipes.Include("PicturesXref").Include("Ratings").ToList();
            var list = _mapper.Map<List<Dto.Recipe>>(recipes);
            var categories = _dbContext.Categories.ToList();
            var varieties = _dbContext.Varieties.ToList();


            foreach (var item in list)
            {
                if (item.Variety != null)
                {
                    var code = varieties.SingleOrDefault(a => a.Id == item.Variety.Id);
                    item.Variety.Literal = code.Variety;
                    var cat = _dbContext.Categories.SingleOrDefault(a => a.Id == code.CategoryId);
                    item.Category = _mapper.Map<ICode>(cat);
                }
            }

            return list;
        }

        /// <summary>
        /// Query a specific Recipe in SQL DB by primary key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns>Recipe as <see cref="Dto.Recipe"/></returns>
        /// <inheritdoc cref="IQuery{T}.Execute(int)"/>
        public Dto.Recipe Execute(int id)
        {
            var recipe = _dbContext.Recipes
                .Where(r => r.Id == id)
                .Include("PicturesXref")
                .Include("Ratings")
                .FirstOrDefault();

            var dto = _mapper.Map<Dto.Recipe>(recipe);
            var img = recipe.PicturesXref.Where(p => p.RecipeId == id).ToList();
            dto.ImageFiles = _mapper.Map<List<Dto.ImageFile>>(img);

            if (dto.Variety != null)
            {
                var code = _dbContext.Varieties.SingleOrDefault(a => a.Id == dto.Variety.Id);
                dto.Variety.Literal = code.Variety;
                var cat = _dbContext.Categories.SingleOrDefault(a => a.Id == code.CategoryId);
                dto.Category = _mapper.Map<ICode>(cat);
            }

            return dto;
        }

        /// <summary>
        /// Asynchronously query all Recipes in SQL DB
        /// </summary>
        /// <returns>Recipes as <see cref="Task{List{Dto.Recipe}}"/></returns>
        /// <inheritdoc cref="IQuery{T}.ExecuteAsync"/>
        public async Task<List<Dto.Recipe>> ExecuteAsync()
        {
            // using TPL to parallel call gets
            List<Task> tasks = new List<Task>();
            var t1 = Task.Run(async () => await _dbContext.Recipes.Include("PicturesXref").Include("Ratings").ToListAsync());
            tasks.Add(t1);
            var list = _mapper.Map<List<Dto.Recipe>>(await t1);

            var t2 = Task.Run(async () => await _dbContext.Categories.ToListAsync());
            tasks.Add(t2);
            var categories = await t2;

            var t3 = Task.Run(async () => await _dbContext.Varieties.ToListAsync());
            tasks.Add(t3);
            var varieties = await t3;

            Task.WaitAll(tasks.ToArray());

            foreach (var item in list)
            {
                if (item.Variety != null)
                {
                    var code = varieties.SingleOrDefault(a => a.Id == item.Variety.Id);
                    item.Variety.Literal = code.Variety;
                    var cat = await _dbContext.Categories.SingleOrDefaultAsync(a => a.Id == code.CategoryId);
                    item.Category = _mapper.Map<ICode>(cat);                    
                }
            }

            return list;
        }

        /// <summary>
        /// Asynchronously query a specific Recipe in SQL DB by primary key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns>Recipe as <see cref="Task{Dto.Recipe}"/></returns>
        /// <inheritdoc cref="IQuery{T}.ExecuteAsync(int)"/>
        public async Task<Dto.Recipe> ExecuteAsync(int id)
        {
            var recipe = await _dbContext.Recipes
                .Where(r => r.Id == id)
                .Include("PicturesXref")
                .Include("Ratings")
                .FirstOrDefaultAsync();

            var dto = _mapper.Map<Dto.Recipe>(recipe);
            var img = recipe.PicturesXref.Where(p => p.RecipeId == id).ToList();
            dto.ImageFiles = _mapper.Map<List<Dto.ImageFile>>(img);            

            if (dto.Variety != null)
            {
                var code = await _dbContext.Varieties.SingleOrDefaultAsync(a => a.Id == dto.Variety.Id);
                dto.Variety.Literal = code.Variety;
                var cat = await _dbContext.Categories.SingleOrDefaultAsync(a => a.Id == code.CategoryId);
                dto.Category = _mapper.Map<ICode>(cat);
            }

            return dto;
        }

    }
}
