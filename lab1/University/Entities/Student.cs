using System;

namespace UniversityApp;

public class Student
{
    public Guid Id { get; }
    public string Name { get; }

    public Student(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}