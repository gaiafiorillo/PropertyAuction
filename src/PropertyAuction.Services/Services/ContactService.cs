namespace PropertyAuction.Services.Services;

public class ContactSubmission
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Message { get; set; } = "";
    public DateTime SubmittedAt { get; set; }
    public bool IsRead { get; set; } = false;
}

public class ContactService
{
    private readonly List<ContactSubmission> _submissions = new();
    private int _nextId = 1;

    public void Submit(string name, string email, string subject, string message)
    {
        _submissions.Add(new ContactSubmission
        {
            Id = _nextId++,
            Name = name,
            Email = email,
            Subject = subject,
            Message = message,
            SubmittedAt = DateTime.Now
        });
    }

    public List<ContactSubmission> GetAll() => _submissions.OrderByDescending(s => s.SubmittedAt).ToList();
    public void MarkAsRead(int id)
    {
        var submission = _submissions.FirstOrDefault(s => s.Id == id);
        if (submission != null) submission.IsRead = true;
    }
    public int UnreadCount() => _submissions.Count(s => !s.IsRead);
}