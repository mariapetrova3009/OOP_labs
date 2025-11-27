using System;
using System.Collections.Generic;

namespace UniversityApp;

public class InMemoryTeacherRepository : ITeacherRepository
{
    private readonly List<Teacher> _teachers = new();

    public void Add(Teacher teacher)
    {
        _teachers.Add(teacher);
    }

    public Teacher? Get(Guid id)
    {
        foreach (var t in _teachers)
        {
            if (t.Id == id)
                return t;
        }

        return null;
    }

    public List<Teacher> GetAll()
    {
        return _teachers;
    }
}