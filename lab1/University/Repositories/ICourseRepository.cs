using University.Entities;

namespace University.Repositories;

public interface ICourseRepository
{
    void Add(Course course);
    void Remove(Guid id);
    Course? Get(Guid id); 
    IReadOnlyCollection<Course> GetAll();
}
