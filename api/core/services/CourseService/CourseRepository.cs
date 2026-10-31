using api.core.models;
using api.core.data;

namespace api.core.services.CourseService
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Course> GetAllCourses() =>
            _context.Courses.ToList();

        public Course GetCourseById(int id) =>
            _context.Courses.FirstOrDefault(c => c.CourseId == id);

        public List<Course> GetCoursesByGame(string game) =>
            _context.Courses.Where(c => c.Game == game).ToList();

        public void CreateCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public bool UpdateCourse(Course course, int id)
        {
            var existing = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (existing == null) return false;
            existing.Name = course.Name;
            existing.Game = course.Game;
            return _context.SaveChanges() > 0;
        }

        public bool SetCourseDeleted(int id)
        {
            var existing = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (existing == null) return false;
            existing.Deleted = 1;
            return _context.SaveChanges() > 0;
        }

        public void DeleteCourse(int id)
        {
            var course = new Course { CourseId = id };
            _context.Courses.Attach(course);
            _context.Courses.Remove(course);
            _context.SaveChanges();
        }
    }
}
