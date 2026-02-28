using api.core.models;

namespace api.core.services.BuildService
{
    public class BuildService : IBuildService
    {
        private readonly IBuildRepository _buildRepository;

        public BuildService(IBuildRepository buildRepository)
        {
            _buildRepository = buildRepository;
        }

        public async Task CreateBuildAsync(Build build)
        {
            await _buildRepository.CreateBuildAsync(build);
        }

        public async Task DeleteBuildAsync(int id)
        {
            await _buildRepository.DeleteBuildAsync(id);
        }

        public async Task<List<Build>> GetAllBuildsAsync()
        {
            return await _buildRepository.GetAllBuildsAsync();
        }

        public async Task<Build?> GetBuildByIdAsync(int id)
        {
            return await _buildRepository.GetBuildByIdAsync(id);
        }

        public async Task<bool> SetBuildDeletedAsync(int id)
        {
            return await _buildRepository.SetBuildDeletedAsync(id);
        }

        public async Task<bool> UpdateBuildAsync(Build build, int id)
        {
            return await _buildRepository.UpdateBuildAsync(build, id);
        }
    }
}