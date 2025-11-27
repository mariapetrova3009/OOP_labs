using System;
using System.Collections.Generic;

namespace UniversityApp;

public class InMemoryStudentRepository : IStudentRepository
{
    private readonly List<Student> _students = new();

    public void Add(Student student)
    {
        _students.Add(student);
    }

    public Student? Get(Guid id)
    {
        foreach (var s in _students)
        {
            if (s.Id == id)
                return s;
        }

        return null;
    }

    public List<Student> GetAll()
    {
        return _students;
    }
}