using University.Entities;

namespace University.Repositories;

public interface IStudentRepository
{
    // добавить студента
    void Add(Student student);
    // получить студента
    Student? Get(Guid id);
    // вернуть всех студентов
    IReadOnlyCollection<Student> GetAll();
}
