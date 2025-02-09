namespace gradeManagerServerAPi.Services.UserService.Notification
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string email, string subject, string message);
    }
}
