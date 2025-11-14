using System;
using System.Linq;
using University.Repositories;
using University.Services;
using University.Entities;

Console.WriteLine("Система управления курсами\n");

// репозитории и сервис
var courseRepo  = new InMemoryCourseRepository();
var teacherRepo = new InMemoryTeacherRepository();
var studentRepo = new InMemoryStudentRepository();
var svc = new CourseService(courseRepo, teacherRepo, studentRepo);

// преподаватели
var t1 = new Teacher("Иван Иванов",     "ivanov@university.ru");
var t2 = new Teacher("Алексей Смирнов", "smirnov@university.ru");
teacherRepo.Add(t1);
teacherRepo.Add(t2);

// студенты
var s1 = new Student("Екатерина Попова", "popova@university.ru");
var s2 = new Student("Ольга Кузнецова",  "kuznetsova@university.ru");
var s3 = new Student("Дмитрий Павлов",   "pavlov@university.ru");
studentRepo.Add(s1);
studentRepo.Add(s2);
studentRepo.Add(s3);

// курсы
var c1 = new OnlineCourse(Guid.NewGuid(),  "Алгоритмы",             "Алгоритмы на Python", "online system", "https://online.itmo.ru/");
var c2 = new OfflineCourse(Guid.NewGuid(), "Математический анализ", "Основы матанализа",   "Главный корпус", "203");

// добавление курсов
svc.AddCourse(c1);
svc.AddCourse(c2);

// назначение преподавателей
svc.AssignTeacher(c1.Id, t1.Id);
svc.AssignTeacher(c2.Id, t2.Id);

// запись студентов
svc.EnrollStudent(c1.Id, s1.Id);
svc.EnrollStudent(c1.Id, s2.Id);
svc.EnrollStudent(c2.Id, s1.Id);
svc.EnrollStudent(c2.Id, s3.Id);


// вывод результата

foreach (var c in courseRepo.GetAll())
{
    var teacherName = c.TeacherId.HasValue ? teacherRepo.Get(c.TeacherId.Value)?.FullName : "не назначен";
    Console.WriteLine($"Курс: {c.Title} | Преподаватель: {teacherName}");
    if (c.StudentIds.Any())
        foreach (var sid in c.StudentIds)
            Console.WriteLine($"  - {studentRepo.Get(sid)?.FullName}");
    else
        Console.WriteLine("  (студентов нет)");
    Console.WriteLine("\n");
}


// получить курсы преподавателя
var t1Courses = svc.GetCoursesByTeacher(t1.Id);
Console.WriteLine($"\nКурсы преподавателя {t1.FullName}: " +
                  (t1Courses.Any() ? string.Join(", ", t1Courses.Select(x => x.Title)) : "(нет)"));

//  получить студентов по курсу
var c1Students = svc.GetStudentsByCourse(c1.Id);
Console.WriteLine($"\nСтуденты на курсе \"{c1.Title}\": " +
                  (c1Students.Any() ? string.Join(", ", c1Students.Select(x => x.FullName)) : "(нет)"));

//  получить курсы по студенту
var s1Courses = svc.GetCoursesByStudent(s1.Id);
Console.WriteLine($"\nКурсы студента {s1.FullName}: " +
                  (s1Courses.Any() ? string.Join(", ", s1Courses.Select(x => x.Title)) : "(нет)"));

//  отписка студента
svc.UnenrollStudent(c1.Id, s2.Id);

// показать изменения по курсу c1
Console.WriteLine($"\nПосле отписки: студенты на \"{c1.Title}\": " +
                  (svc.GetStudentsByCourse(c1.Id).Any()
                      ? string.Join(", ", svc.GetStudentsByCourse(c1.Id).Select(x => x.FullName))
                      : "(нет)"));

// удаление курса
svc.RemoveCourse(c2.Id);

// финальный список курсов
Console.WriteLine("\nКурсы после удаления одного из них:");
foreach (var c in courseRepo.GetAll())
{
    var teacherName = c.TeacherId.HasValue ? teacherRepo.Get(c.TeacherId.Value)?.FullName : "не назначен";
    Console.WriteLine($"- {c.Title} (преподаватель: {teacherName})");
}

