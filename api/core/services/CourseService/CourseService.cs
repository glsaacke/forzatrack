using api.core.models;

namespace api.core.services.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public List<Course> GetAllCourses() =>
            _courseRepository.GetAllCourses();

        public Course GetCourseById(int id) =>
            _courseRepository.GetCourseById(id);

        public List<Course> GetCoursesByGame(string game) =>
            _courseRepository.GetCoursesByGame(game);

        public void CreateCourse(Course course) =>
            _courseRepository.CreateCourse(course);

        public bool UpdateCourse(Course course, int id) =>
            _courseRepository.UpdateCourse(course, id);

        public bool SetCourseDeleted(int id) =>
            _courseRepository.SetCourseDeleted(id);

        public void DeleteCourse(int id) =>
            _courseRepository.DeleteCourse(id);
    }
}
