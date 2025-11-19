namespace University.Entities;

public class Teacher
{
    public Guid Id { get; } = Guid.NewGuid();
    public string FullName { get; }
    public string Email { get; }

    public Teacher(string fullName, string email)
    { FullName = fullName; Email = email; }
}
