using University.Entities;

namespace University.Repositories;

public interface ITeacherRepository
{
    void Add(Teacher teacher);
    Teacher? Get(Guid id);
    IReadOnlyCollection<Teacher> GetAll();
}
