using FolioWatcher.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace FolioWatcher
{
    public class Emailer
    {
        private readonly SettingsDto _settings;

        public Emailer(SettingsDto settings)
        {
            _settings = settings;
        }

        public async Task SendEmailAsync(string toEmailAddress, string toName, string subject, string body, CancellationToken cancellationToken = default)
        {
            var host = _settings.SmtpHost;

            var port = _settings.SmtpPort;

            var encryption = _settings.SmtpEncryption;

            var userName = _settings.SmtpUsername;

            var password = _settings.SmtpPassword;

            var defaultFromEmail = _settings.SmtpDefaultFromEmail;

            var defaultFromName = _settings.SmtpDefaultFromName;

            var fromMailboxAddress = new MailboxAddress(defaultFromName, defaultFromEmail);

            var toMailboxAddress = new MailboxAddress(toName, toEmailAddress);

            var textPart = new TextPart("html") { Text = body };

            var message = new MimeMessage
            {
                From = { fromMailboxAddress },
                To = { toMailboxAddress },
                Subject = subject,
                Body = textPart
            };

            using var client = new SmtpClient();

            var secureSocketOptions = (SecureSocketOptions)Enum.Parse(typeof(SecureSocketOptions), encryption);

            await client.ConnectAsync(host, port, secureSocketOptions, cancellationToken);

            await client.AuthenticateAsync(userName, password, cancellationToken);

            await client.SendAsync(message, cancellationToken);

            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}
