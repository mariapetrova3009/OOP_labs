using University.Entities;

namespace University.Repositories;

public class InMemoryCourseRepository : ICourseRepository
{
    // для хранения курсов
    private readonly Dictionary<Guid, Course> _data = new();

    public void Add(Course course) => _data[course.Id] = course;
    public void Remove(Guid id) => _data.Remove(id);
    public Course? Get(Guid id) {
    if (_data.TryGetValue(id, out var c)) {
        return c;
    } else {
        return null;
        }
    }

    public IReadOnlyCollection<Course> GetAll() => _data.Values.ToList();
}
