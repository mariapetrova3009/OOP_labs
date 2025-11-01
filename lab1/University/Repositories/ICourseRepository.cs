using University.Entities;

namespace University.Repositories;

public interface ICourseRepository
{
    // добавить курс
    void Add(Course course);
    // удалить курс
    void Remove(Guid id);
    // вернуть курс по id
    Course? Get(Guid id); 
    // вернуть коллекцию всех курсов
    IReadOnlyCollection<Course> GetAll();
}
