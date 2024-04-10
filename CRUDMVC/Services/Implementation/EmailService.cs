using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService : IEmailService
{
    public async Task SendPasswordResetEmail(string email, string resetLink)
    {
        var fromAddress = new MailAddress("tareaskikemtz@gmail.com", "KikeBOT");
        var toAddress = new MailAddress(email);
        const string fromPassword = "vjzk esnw cjqe ocyt";
        const string subject = "Recuperación de contraseña";
        string body = $"Puedes restaurar tu contraseña con el siguiente link: {resetLink}";

        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };
        using (var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body
        })
        {
            await smtp.SendMailAsync(message);
        }
    }
}