using University.Entities;
using University.Repositories;

namespace University.Services;

public sealed class CourseService
{
    private readonly ICourseRepository _courses;
    private readonly ITeacherRepository _teachers;
    private readonly IStudentRepository _students;

    public CourseService(ICourseRepository courses, ITeacherRepository teachers, IStudentRepository students)
    {
        _courses = courses;
        _teachers = teachers;
        _students = students;
    }

    public void AddCourse(Course course) => _courses.Add(course);

    public void RemoveCourse(Guid id) => _courses.Remove(id);

    public void AssignTeacher(Guid courseId, Guid teacherId)
    {
        var course = _courses.Get(courseId) ?? throw new KeyNotFoundException("Course not found");
        if (_teachers.Get(teacherId) is null)
            throw new KeyNotFoundException("Teacher not found");
        course.AssignTeacher(teacherId);
    }

    // Получить список всех курсов преподавателя
    public IReadOnlyCollection<Course> GetCoursesByTeacher(Guid teacherId) =>
        _courses.GetAll().Where(c => c.TeacherId == teacherId).ToList();

    // Записать студента на курс
    public void EnrollStudent(Guid courseId, Guid studentId)
    {
        var course = _courses.Get(courseId) ?? throw new KeyNotFoundException("Course not found");
        if (_students.Get(studentId) is null)
            throw new KeyNotFoundException("Student not found");
        course.EnrollStudent(studentId);
    }

    // Отписать студента от курса
    public void UnenrollStudent(Guid courseId, Guid studentId)
    {
        var course = _courses.Get(courseId) ?? throw new KeyNotFoundException("Course not found");
        if (_students.Get(studentId) is null)
            throw new KeyNotFoundException("Student not found");

        if (!course.StudentIds.Contains(studentId))
            throw new InvalidOperationException("Student is not enrolled in this course");

        course.StudentIds.Remove(studentId);
    }

    // Получить список курсов, на которые записан данный студент
    public IReadOnlyCollection<Course> GetCoursesByStudent(Guid studentId)
    {
        if (_students.Get(studentId) is null)
            throw new KeyNotFoundException("Student not found");

        return _courses.GetAll()
            .Where(c => c.StudentIds.Contains(studentId))
            .ToList();
    }

    // Получить всех студентов, записанных на курс
    public IReadOnlyCollection<Student> GetStudentsByCourse(Guid courseId)
    {
        var course = _courses.Get(courseId) ?? throw new KeyNotFoundException("Course not found");

        var list = new List<Student>();

        foreach (var id in course.StudentIds) {
            var student = _students.Get(id);
            if (student != null) {
                list.Add(student);
            }
            
        }
    return list;
    }
}
