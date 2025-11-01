namespace University.Entities;

public class Student
{
    public Guid Id { get; } = Guid.NewGuid();
    public string FullName { get; }
    public string Email { get; }

    public Student(string fullName, string email)
    { FullName = fullName; Email = email; }
}
