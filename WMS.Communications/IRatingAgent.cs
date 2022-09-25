using WMS.Domain;

namespace WMS.Communications
{
    public interface IRatingAgent
    {
        Task<Rating> AddRating(Rating rating);
        Task<bool> DeleteRating(int id);
        Task<Rating> GetRating(int id);
        Task<IEnumerable<Rating>> GetRatings();
        Task<Rating> UpdateRating(Rating rating);
    }
}