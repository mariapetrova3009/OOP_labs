using System;
using System.Collections.Generic;

namespace UniversityApp;

public interface IStudentRepository
{
    void Add(Student student);
    Student? Get(Guid id);
    List<Student> GetAll();
}