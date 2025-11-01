using University.Entities;

namespace University.Repositories;

public class InMemoryCourseRepository : ICourseRepository
{
    // словарь для хранения курсов
    private readonly Dictionary<Guid, Course> _data = new();

    // добавление курса 
    public void Add(Course course) => _data[course.Id] = course;
    // удаление курса 
    public void Remove(Guid id) => _data.Remove(id);
    // получение курса по id
    public Course? Get(Guid id) {
    if (_data.TryGetValue(id, out var c)) {
        return c;
    } else {
        return null;
        }
    }

    // получение всех курсов
    public IReadOnlyCollection<Course> GetAll() => _data.Values.ToList();
}
