using University.Entities;

namespace University.Repositories;

public interface ITeacherRepository
{
    // добавить преподавателя
    void Add(Teacher teacher);
    // получить преподавателя
    Teacher? Get(Guid id);

    // получить список всех преподавателей
    IReadOnlyCollection<Teacher> GetAll();
}
