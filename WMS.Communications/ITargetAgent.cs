using WMS.Domain;

namespace WMS.Communications
{
    public interface ITargetAgent
    {
        Task<Target> AddTarget(Target Target);
        Task<bool> DeleteTarget(int id);
        Task<Target> GetTarget(int id);
        Task<IEnumerable<Target>> GetTargets();
        Task<IEnumerable<Target>> GetTargets(int start, int length);
        Task<Target> UpdateTarget(Target Target);
    }
}