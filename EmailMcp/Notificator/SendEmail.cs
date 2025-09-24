using EmailMcp.Configuration;
using EmailMcp.Dto;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailMcp.Notificator;
public interface ISendEmail
{
    Task Execute(EmailDto request);
}
public class SendEmail(IOptions<MailOptions> mailOptions):ISendEmail
{
    private readonly MailOptions _mailOptionsOptions = mailOptions.Value;
   
    public async Task Execute(EmailDto request)
    {
        MimeMessage email = new();
        email.From.Add(MailboxAddress.Parse(_mailOptionsOptions.UserName));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = request.Body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(
            _mailOptionsOptions.Host,
            _mailOptionsOptions.Port,
            SecureSocketOptions.StartTls
        );
        await smtp.AuthenticateAsync(
            _mailOptionsOptions.UserName,
            _mailOptionsOptions.Password
        );
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}