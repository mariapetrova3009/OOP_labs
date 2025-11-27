using System;
using System.Collections.Generic;

namespace UniversityApp;

public interface ITeacherRepository
{
    void Add(Teacher teacher);
    Teacher? Get(Guid id);
    List<Teacher> GetAll();
}