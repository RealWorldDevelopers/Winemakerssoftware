using WMS.Domain;

namespace WMS.Communications
{
    public interface ICategoryAgent
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategory(int id);
        Task<Category> AddCategory(Category recipe);
        Task<Category> UpdateCategory(Category recipe);
        Task<bool> DeleteCategory(int id);
    }


}
