namespace gradeManagerServerAPi.Services.UserService.Notification
{
    using MailKit.Net.Smtp;
    using MimeKit;
    using System.Threading.Tasks;

    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer = "sandbox.smtp.mailtrap.io";
        private readonly int _smtpPort = 25;
        private readonly string _smtpUser = "299ea06e683802";
        private readonly string _smtpPassword = "689f6f9af480a4";
        private readonly string _fromEmail = "no-reply@yourdomain.com";

        public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("Your App Name", _fromEmail));
                mimeMessage.To.Add(new MailboxAddress("", email));
                mimeMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    TextBody = message
                };
                mimeMessage.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, false);
                    await client.AuthenticateAsync(_smtpUser, _smtpPassword);
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }


}
