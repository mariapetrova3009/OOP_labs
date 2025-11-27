using System;
using System.Collections.Generic;

namespace UniversityApp;

public interface ICourseRepository
{
    void Add(Course course);
    Course? Get(Guid id);
    List<Course> GetAll();
}