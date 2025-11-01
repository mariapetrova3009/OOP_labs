namespace University.Entities;

public class OfflineCourse : Course
{
    public string Campus { get; }
    public string Room { get; }

    public OfflineCourse(Guid id, string title, string description, string campus, string room)
        : base(id, title, description)
    { Campus = campus; Room = room; }
}
