using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;
using ScentsSymphonyWeb.Models;

namespace ScentsSymphonyWeb.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _secretKey;

        public EmailService(IOptions<MailjetSettings> options)
        {
            _apiKey = options.Value.ApiKey;
            _secretKey = options.Value.SecretKey;
        }

        public async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlContent, byte[] attachment = null, string attachmentName = null)
        {
            var client = new RestClient("https://api.mailjet.com/v3.1/send");
            var request = new RestRequest("", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic " +
                Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_apiKey}:{_secretKey}")));

            var message = new
            {
                From = new { Email = "scentssymphony@outlook.com", Name = "Scents Symphony" },
                To = new[] { new { Email = toEmail, Name = toName } },
                Subject = subject,
                HTMLPart = htmlContent,
                Attachments = attachment != null ? new[]
                {
                    new {
                        ContentType = "application/pdf",
                        Filename = attachmentName,
                        Base64Content = Convert.ToBase64String(attachment)
                    }
                } : null
            };

            var body = new { Messages = new[] { message } };
            var jsonBody = JsonSerializer.Serialize(body, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            request.AddStringBody(jsonBody, DataFormat.Json);

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Failed to send email: " + response.Content);
            }
        }
    }
}
