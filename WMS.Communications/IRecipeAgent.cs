using WMS.Domain;

namespace WMS.Communications
{
    public interface IRecipeAgent
    {
        Task<IEnumerable<Recipe>> GetRecipes();
        Task<Recipe> GetRecipe(int id);
        Task<Recipe> AddRecipe(Recipe recipe);
        Task<Recipe> UpdateRecipe(Recipe recipe);
        Task<bool> DeleteRecipe(int id);

    }


}
