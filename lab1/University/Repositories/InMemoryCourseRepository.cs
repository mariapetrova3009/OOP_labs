using System;
using System.Collections.Generic;

namespace UniversityApp;

public class InMemoryCourseRepository : ICourseRepository
{
    private readonly List<Course> _courses = new();

    public void Add(Course course)
    {
        _courses.Add(course);
    }

    public Course? Get(Guid id)
    {
        foreach (var c in _courses)
        {
            if (c.Id == id)
                return c;
        }

        return null;
    }

    public List<Course> GetAll()
    {
        return _courses;
    }
}