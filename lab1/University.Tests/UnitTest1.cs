using System;
using System.Linq;
using University.Entities;
using University.Repositories;
using University.Services;
using Xunit;

namespace University.Tests;

public class CourseServiceTests
{
    private (CourseService svc, InMemoryCourseRepository courseRepo,
             InMemoryTeacherRepository teacherRepo, InMemoryStudentRepository studentRepo) MakeSut()
    {
        var courseRepo  = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var svc = new CourseService(courseRepo, teacherRepo, studentRepo);
        return (svc, courseRepo, teacherRepo, studentRepo);
    }

    [Fact]
    public void AddCourse_And_RemoveCourse_Works()
    {
        var (svc, courseRepo, _, _) = MakeSut();
        var c1 = new OnlineCourse(Guid.NewGuid(), "Алгосы", "Алгоритмы", "Moodle", "link");

        svc.AddCourse(c1);
        Assert.Single(courseRepo.GetAll());

        svc.RemoveCourse(c1.Id);
        Assert.Empty(courseRepo.GetAll());
    }

    [Fact]
    public void AssignTeacher_Succeeds_WhenCourseAndTeacherExist()
    {
        var (svc, courseRepo, teacherRepo, _) = MakeSut();

        var c = new OfflineCourse(Guid.NewGuid(), "Матан", "Базовый", "Главный", "203");
        var t = new Teacher("Иван Иванов", "ivanov@u.ru");

        courseRepo.Add(c);
        teacherRepo.Add(t);

        svc.AssignTeacher(c.Id, t.Id);

        var updated = courseRepo.Get(c.Id)!;
        Assert.Equal(t.Id, updated.TeacherId);
    }

    [Fact]
    public void AssignTeacher_Throws_WhenCourseNotFound()
    {
        var (svc, _, teacherRepo, _) = MakeSut();
        var t = new Teacher("Иван Иванов", "ivanov@u.ru");
        teacherRepo.Add(t);

        var ex = Assert.Throws<KeyNotFoundException>(() =>
            svc.AssignTeacher(Guid.NewGuid(), t.Id));
        Assert.Equal("Course not found", ex.Message);
    }

    [Fact]
    public void AssignTeacher_Throws_WhenTeacherNotFound()
    {
        var (svc, courseRepo, _, _) = MakeSut();
        var c = new OnlineCourse(Guid.NewGuid(), "ООП", "Основы", "Moodle", "link");
        courseRepo.Add(c);

        var ex = Assert.Throws<KeyNotFoundException>(() =>
            svc.AssignTeacher(c.Id, Guid.NewGuid()));
        Assert.Equal("Teacher not found", ex.Message);
    }

    [Fact]
    public void EnrollStudent_Succeeds_WhenCourseAndStudentExist()
    {
        var (svc, courseRepo, _, studentRepo) = MakeSut();

        var c = new OnlineCourse(Guid.NewGuid(), "Сети", "Основы", "Platform", "link");
        var s = new Student("Петров Пётр", "petrov@u.ru");

        courseRepo.Add(c);
        studentRepo.Add(s);

        svc.EnrollStudent(c.Id, s.Id);

        var updated = courseRepo.Get(c.Id)!;
        Assert.Contains(s.Id, updated.StudentIds);
    }

    [Fact]
    public void EnrollStudent_Throws_WhenCourseNotFound()
    {
        var (svc, _, _, studentRepo) = MakeSut();
        var s = new Student("Петров Пётр", "petrov@u.ru");
        studentRepo.Add(s);

        var ex = Assert.Throws<KeyNotFoundException>(() =>
            svc.EnrollStudent(Guid.NewGuid(), s.Id));
        Assert.Equal("Course not found", ex.Message);
    }

    [Fact]
    public void EnrollStudent_Throws_WhenStudentNotFound()
    {
        var (svc, courseRepo, _, _) = MakeSut();
        var c = new OfflineCourse(Guid.NewGuid(), "БД", "SQL", "A", "101");
        courseRepo.Add(c);

        var ex = Assert.Throws<KeyNotFoundException>(() =>
            svc.EnrollStudent(c.Id, Guid.NewGuid()));
        Assert.Equal("Student not found", ex.Message);
    }

    [Fact]
    public void UnenrollStudent_RemovesStudent_WhenEnrolled()
    {
        var (svc, courseRepo, _, studentRepo) = MakeSut();
        var c = new OnlineCourse(Guid.NewGuid(), "ML", "Введение", "Platform", "link");
        var s = new Student("Иван Петров", "ivan@u.ru");

        courseRepo.Add(c);
        studentRepo.Add(s);

        // сначала запишем
        svc.EnrollStudent(c.Id, s.Id);
        Assert.Contains(s.Id, courseRepo.Get(c.Id)!.StudentIds);

        // затем отпишем
        svc.UnenrollStudent(c.Id, s.Id);
        Assert.DoesNotContain(s.Id, courseRepo.Get(c.Id)!.StudentIds);
    }

    [Fact]
    public void UnenrollStudent_Throws_WhenNotEnrolled()
    {
        var (svc, courseRepo, _, studentRepo) = MakeSut();
        var c = new OnlineCourse(Guid.NewGuid(), "DevOps", "Intro", "Platform", "link");
        var s = new Student("Мария Сидорова", "maria@u.ru");
        courseRepo.Add(c);
        studentRepo.Add(s);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            svc.UnenrollStudent(c.Id, s.Id));
        Assert.Equal("Student is not enrolled in this course", ex.Message);
    }

    [Fact]
    public void GetCoursesByTeacher_ReturnsOnlyCoursesOfThatTeacher()
    {
        var (svc, courseRepo, teacherRepo, _) = MakeSut();

        var t1 = new Teacher("Преп1", "t1@u.ru");
        var t2 = new Teacher("Преп2", "t2@u.ru");
        teacherRepo.Add(t1);
        teacherRepo.Add(t2);

        var c1 = new OfflineCourse(Guid.NewGuid(), "Курс1", "Desc", "B", "201");
        var c2 = new OnlineCourse(Guid.NewGuid(), "Курс2", "Desc", "Plat", "link");
        var c3 = new OfflineCourse(Guid.NewGuid(), "Курс3", "Desc", "C", "305");

        courseRepo.Add(c1); courseRepo.Add(c2); courseRepo.Add(c3);

        svc.AssignTeacher(c1.Id, t1.Id);
        svc.AssignTeacher(c2.Id, t1.Id);
        svc.AssignTeacher(c3.Id, t2.Id);

        var byT1 = svc.GetCoursesByTeacher(t1.Id);
        Assert.Equal(2, byT1.Count);
        Assert.All(byT1, c => Assert.Equal(t1.Id, c.TeacherId));
    }

    [Fact]
    public void GetStudentsByCourse_ReturnsAllMappedStudents()
    {
        var (svc, courseRepo, _, studentRepo) = MakeSut();
        var c = new OfflineCourse(Guid.NewGuid(), "Контуры", "Desc", "D", "110");
        var s1 = new Student("A", "a@u.ru");
        var s2 = new Student("B", "b@u.ru");

        courseRepo.Add(c);
        studentRepo.Add(s1);
        studentRepo.Add(s2);

        svc.EnrollStudent(c.Id, s1.Id);
        svc.EnrollStudent(c.Id, s2.Id);

        var students = svc.GetStudentsByCourse(c.Id);
        var names = students.Select(s => s.FullName).ToArray();

        Assert.Equal(2, students.Count);
        Assert.Contains("A", names);
        Assert.Contains("B", names);
    }

    [Fact]
    public void GetCoursesByStudent_ReturnsAllHisCourses()
    {
        var (svc, courseRepo, _, studentRepo) = MakeSut();

        var s = new Student("Студент", "s@u.ru");
        studentRepo.Add(s);

        var c1 = new OnlineCourse(Guid.NewGuid(), "Теорвер", "Desc", "Plat", "link");
        var c2 = new OfflineCourse(Guid.NewGuid(), "Дискра", "Desc", "K1", "12");
        var c3 = new OfflineCourse(Guid.NewGuid(), "История", "Desc", "K2", "21");

        courseRepo.Add(c1); courseRepo.Add(c2); courseRepo.Add(c3);

        svc.EnrollStudent(c1.Id, s.Id);
        svc.EnrollStudent(c2.Id, s.Id);

        var courses = svc.GetCoursesByStudent(s.Id);
        var titles = courses.Select(c => c.Title).ToArray();

        Assert.Equal(2, courses.Count);
        Assert.Contains("Теорвер", titles);
        Assert.Contains("Дискра", titles);
        Assert.DoesNotContain("История", titles);
    }
}
