public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body);
}

public class EmailService : IEmailService
{
    public Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        // Simulate email sending
        Console.WriteLine($"Email sent to {to}: {subject} - {body}");
        return Task.FromResult(true);
    }
}
