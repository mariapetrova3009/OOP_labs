using University.Entities;

namespace University.Repositories;

public class InMemoryTeacherRepository : ITeacherRepository
{
    // для хранения преподавателей
    private readonly Dictionary<Guid, Teacher> _data = new();

    public void Add(Teacher t) => _data[t.Id] = t;
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

    public IReadOnlyCollection<Teacher> GetAll() => _data.Values.ToList();
}
