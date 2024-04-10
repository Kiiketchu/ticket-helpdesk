public interface IEmailService
{
    Task SendPasswordResetEmail(string email, string resetLink);
}