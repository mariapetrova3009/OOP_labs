using University.Entities;

namespace University.Repositories;

public class InMemoryTeacherRepository : ITeacherRepository
{
    // словарь для преподавателей
    private readonly Dictionary<Guid, Teacher> _data = new();

    // добавить преподавателя
    public void Add(Teacher t) => _data[t.Id] = t;
    // получить преподавателя по id
    public Teacher? Get(Guid id)
    {
        if (_data.TryGetValue(id, out var t))
        {
            return t;
        }
        else
        {
            return null;
        }
    }

    // вернуть список всех преподавателей
    public IReadOnlyCollection<Teacher> GetAll() => _data.Values.ToList();
}
