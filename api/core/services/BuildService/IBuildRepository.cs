using api.core.models;

namespace api.core.services.BuildService
{
    public interface IBuildRepository
    {
        Task<List<Build>> GetAllBuildsAsync();
        Task<Build?> GetBuildByIdAsync(int id);
        Task CreateBuildAsync(Build build);
        Task<bool> UpdateBuildAsync(Build build, int id);
        Task<bool> SetBuildDeletedAsync(int id);
        Task DeleteBuildAsync(int id);
    }
}