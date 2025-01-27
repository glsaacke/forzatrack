using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;

namespace api.core.services.BuildService
{
    public interface IBuildService
    {
        List<Build> GetAllBuilds();
        Build GetBuildByID(int id);
        void CreateBuild(Build build);
        bool UpdateBuild(Build build, int id);
        bool SetBuildDeleted(int id);
        void DeleteBuild(int id);
    }
}