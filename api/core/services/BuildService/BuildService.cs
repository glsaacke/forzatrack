using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;

namespace api.core.services.BuildService
{
    public class BuildService : IBuildService
    {
        private IBuildRepository buildRepository;

        public BuildService (IBuildRepository buildRepository){
            this.buildRepository = buildRepository;
        }

        public void CreateBuild(Build build)
        {
            buildRepository.CreateBuild(build);
        }

        public void DeleteBuild(int id)
        {
            buildRepository.DeleteBuild(id);
        }

        public List<Build> GetAllBuilds()
        {
            var builds = buildRepository.GetAllBuilds();
            return builds;
        }

        public Build GetBuildByID(int id)
        {
            Build build = buildRepository.GetBuildByID(id);
            return build;
        }

        public bool SetBuildDeleted(int id)
        {
            bool rowsAffected = buildRepository.SetBuildDeleted(id);
            return rowsAffected;
        }

        public bool UpdateBuild(Build build, int id)
        {
            bool rowsAffected = buildRepository.UpdateBuild(build, id);
            return rowsAffected;
        }
    }
}