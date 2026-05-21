using api.core.models;
using api.core.data;

namespace api.core.services.BuildService
{
    public class BuildRepository : IBuildRepository
    {
        private readonly AppDbContext _context;
        public BuildRepository(AppDbContext context){
            _context = context;
        }

        public List<Build> GetAllBuilds(){
            return _context.Builds.ToList();
        }

        public Build GetBuildByID(int id){
            return _context.Builds.FirstOrDefault(b => b.BuildId == id);
        }

        public void CreateBuild(Build build){
            _context.Builds.Add(build);
            _context.SaveChanges();
        }

        public bool UpdateBuild(Build build, int id){
            var existing = _context.Builds.FirstOrDefault(b => b.BuildId == id);
            if (existing == null) return false;

            existing.CarId = build.CarId;
            existing.Rank = build.Rank;
            existing.SpeedST = build.SpeedST;
            existing.HandlingST = build.HandlingST;
            existing.AccelerationST = build.AccelerationST;
            existing.LaunchST = build.LaunchST;
            existing.BrakingST = build.BrakingST;
            existing.OffroadST = build.OffroadST;
            existing.TopSpeed = build.TopSpeed;
            existing.ZeroToSixty = build.ZeroToSixty;
            return _context.SaveChanges() > 0;
        }

        public bool SetBuildDeleted(int id){
            var existing = _context.Builds.FirstOrDefault(b => b.BuildId == id);
            if (existing == null) return false;

            existing.Deleted = 1;
            return _context.SaveChanges() > 0;
        }

        public void DeleteBuild(int id){
            var build = new Build { BuildId = id };
            _context.Builds.Attach(build);
            _context.Builds.Remove(build);
            _context.SaveChanges();
        }
    }
}
