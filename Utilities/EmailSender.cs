
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace EmployeeManagementAuth.Utilities;
public class EmailSender : IEmailSender
{
    private readonly string MAILJET_SENDER_EMAIL_ADDRESS = Environment.GetEnvironmentVariable("MAILJET_SENDER_EMAIL_ADDRESS");
    private readonly string MAILJET_API_KEY = Environment.GetEnvironmentVariable("MAILJET_API_KEY");
    private readonly string MAILJET_SECRET_KEY = Environment.GetEnvironmentVariable("MAILJET_SECRET_KEY");

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        MailjetClient client = new MailjetClient(MAILJET_API_KEY, MAILJET_SECRET_KEY)
        {

        };

        MailjetRequest request = new MailjetRequest
        {
            Resource = Send.Resource,
        }
           .Property(Send.FromEmail, MAILJET_SENDER_EMAIL_ADDRESS)
           .Property(Send.FromName, "Employee Management")
           .Property(Send.Subject, subject)
           .Property(Send.HtmlPart, htmlMessage)
           .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", email}
                 }
               });
        MailjetResponse response = await client.PostAsync(request);

    }
}
