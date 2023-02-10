using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace ET_ShiftManagementSystem.Servises
{
    public class EmailSendGrid
    {
        private static void Main()
        {
            Execute().Wait();
        }

        static async Task Execute()
        {
            var apiKey = Environment.GetEnvironmentVariable("ET-ShiftManagement");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("harshith.kumar@euphoricthought.com", "harsh");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("harshavardhan78143@gmail.com", "ha");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}