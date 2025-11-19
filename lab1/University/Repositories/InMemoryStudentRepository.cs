using University.Entities;

namespace University.Repositories;

public class InMemoryStudentRepository : IStudentRepository
{
    private readonly Dictionary<Guid, Student> _data = new();
    public void Add(Student s) => _data[s.Id] = s;
    public Student? Get(Guid id) {
    if (_data.TryGetValue(id, out var s))
        return s;
    else
        return null;
    }

    public IReadOnlyCollection<Student> GetAll() => _data.Values.ToList();
}
