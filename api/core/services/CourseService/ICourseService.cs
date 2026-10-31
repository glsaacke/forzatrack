using api.core.models;

namespace api.core.services.CourseService
{
    public interface ICourseService
    {
        List<Course> GetAllCourses();
        Course GetCourseById(int id);
        List<Course> GetCoursesByGame(string game);
        void CreateCourse(Course course);
        bool UpdateCourse(Course course, int id);
        bool SetCourseDeleted(int id);
        void DeleteCourse(int id);
    }
}
