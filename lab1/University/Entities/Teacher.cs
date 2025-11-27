using System;

namespace UniversityApp;

public class Teacher
{
    public Guid Id { get; }
    public string Name { get; }

    public Teacher(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}