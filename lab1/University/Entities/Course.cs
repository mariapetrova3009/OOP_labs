using System;
using System.Collections.Generic;

namespace UniversityApp;

public class Course
{
    public Guid Id { get; }
    public string Name { get; }

    public Guid? TeacherId { get; private set; }

    public List<Guid> StudentIds { get; } = new();

    public Course(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public void AssignTeacher(Guid teacherId)
    {
        TeacherId = teacherId;
    }

    public void EnrollStudent(Guid studentId)
    {
        if (!StudentIds.Contains(studentId))
            StudentIds.Add(studentId);
    }
}