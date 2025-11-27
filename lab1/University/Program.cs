using System;
using UniversityApp;

class Program
{
    static void Main()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var teacherRepo = new InMemoryTeacherRepository();

        var service = new CourseService(courseRepo, teacherRepo, studentRepo);

        var teacher = new Teacher(Guid.NewGuid(), "Иванов");
        service.AddTeacher(teacher);

        var student = new Student(Guid.NewGuid(), "Петров");
        service.AddStudent(student);

        var course = new Course(Guid.NewGuid(), "C# базовый");
        service.AddCourse(course);

        service.AssignTeacher(course.Id, teacher.Id);
        service.EnrollStudent(course.Id, student.Id);

        var coursesOfTeacher = service.GetCoursesByTeacher(teacher.Id);
        var studentsOfCourse = service.GetStudentsByCourse(course.Id);

        Console.WriteLine("Курсы преподавателя:");
        foreach (var c in coursesOfTeacher)
            Console.WriteLine(c.Name);

        Console.WriteLine("Студенты на курсе:");
        foreach (var s in studentsOfCourse)
            Console.WriteLine(s.Name);
    }
}