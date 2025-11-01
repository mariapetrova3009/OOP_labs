namespace University.Entities;

public sealed class OnlineCourse : Course
{
    public string Platform { get; }
    public string MeetingLink { get; }

    public OnlineCourse(Guid id, string title, string description, string platform, string link)
        : base(id, title, description)
    { Platform = platform; MeetingLink = link; }
}
