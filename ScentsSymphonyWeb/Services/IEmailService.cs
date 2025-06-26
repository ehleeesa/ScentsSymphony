namespace ScentsSymphonyWeb.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string toName, string subject, string htmlContent, byte[] attachment = null, string attachmentName = null);
    }
}