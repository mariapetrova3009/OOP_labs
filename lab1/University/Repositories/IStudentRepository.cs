using University.Entities;

namespace University.Repositories;

public interface IStudentRepository
{
    void Add(Student student);
    Student? Get(Guid id);
    IReadOnlyCollection<Student> GetAll();
}
