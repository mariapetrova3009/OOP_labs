namespace University.Entities;

public abstract class Course
{
    public Guid Id { get; }
    public string Title { get; }
    public string Description { get; }
    public Guid? TeacherId { get; private set; }
    public List<Guid> StudentIds { get; } = new();
    
    protected Course(Guid id, string title, string description)
    {
        Id = id; Title = title; Description = description;
    }

    // назначение преподавателя на курс
    public void AssignTeacher(Guid teacherId) => TeacherId = teacherId;

    // запись студента на курс

    public void EnrollStudent(Guid studentId)
    {
        if (StudentIds.Contains(studentId))
            throw new InvalidOperationException("Student already enrolled");
        StudentIds.Add(studentId);
    }
}
