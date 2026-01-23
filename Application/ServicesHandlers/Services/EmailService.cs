using Hangfire;
using MailKit.Net.Smtp;
using MimeKit;

namespace Application.ServicesHandlers.Services;

public interface IEmailService
{
    Task<EmailDto> SendEmailsAsync(EmailDto emailDto, CancellationToken cancellationToken = default);
}

public class EmailService(IOptions<EmailSettings> emailSettings, IBackgroundJobClient backgroundJobClient) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public async Task<EmailDto> SendEmailsAsync(EmailDto emailDto, CancellationToken cancellationToken = default)
    {
        if (emailDto == null)
            throw new ArgumentNullException(nameof(emailDto), "EmailDto cannot be null.");

        // Validate that at least one recipient is provided
        var hasRecipients = !string.IsNullOrWhiteSpace(emailDto.MailTo) || (emailDto.MailToList != null && emailDto.MailToList.Any());

        if (!hasRecipients)
            throw new ArgumentException("At least one recipient email address is required.", nameof(emailDto));

        if (string.IsNullOrWhiteSpace(emailDto.Subject))
            throw new ArgumentException("Email subject is required.", nameof(emailDto));

        if (string.IsNullOrWhiteSpace(emailDto.Body))
            throw new ArgumentException("Email body is required.", nameof(emailDto));

        // Convert IFormFile attachments to byte arrays for serialization
        var serializableEmailDto = ConvertToSerializableEmailDto(emailDto);

        // Enqueue email sending as a background job using Hangfire
        backgroundJobClient.Enqueue(() => SendEmailBackgroundJob(serializableEmailDto));

        return emailDto;
    }

    private static SerializableEmailDto ConvertToSerializableEmailDto(EmailDto emailDto)
    {
        var serializableDto = new SerializableEmailDto
        {
            MailTo = emailDto.MailTo,
            Subject = emailDto.Subject,
            Body = emailDto.Body,
            MailToList = emailDto.MailToList?.ToList() ?? [],
            EmailCC = emailDto.EmailCC?.ToList() ?? [],
            EmailBCC = emailDto.EmailBCC?.ToList() ?? [],
            Priority = emailDto.Priority,
            DocumentsList = emailDto.DocumentsList?.ToList() ?? []
        };

        // Convert IFormFile attachments to byte arrays
        if (emailDto.Attachments != null && emailDto.Attachments.Any())
        {
            serializableDto.Attachments = [];
            foreach (var attachment in emailDto.Attachments)
            {
                if (attachment != null && attachment.Length > 0)
                {
                    using var stream = attachment.OpenReadStream();
                    using var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    serializableDto.Attachments.Add(new AttachmentData
                    {
                        FileName = attachment.FileName,
                        Content = memoryStream.ToArray(),
                        ContentType = attachment.ContentType
                    });
                }
            }
        }

        return serializableDto;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task SendEmailBackgroundJob(SerializableEmailDto emailDto)
    {
        var message = CreateMimeMessage(emailDto);

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);
            await smtp.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        catch (SmtpCommandException)
        {
            throw; // Let Hangfire handle retries
        }
        catch (SmtpProtocolException)
        {
            throw; // Let Hangfire handle retries
        }
        catch (Exception)
        {
            throw; // Let Hangfire handle retries
        }
    }

    private MimeMessage CreateMimeMessage(SerializableEmailDto emailDto)
    {
        var message = new MimeMessage();

        // Set From
        message.From.Add(new MailboxAddress("Tajerly Support", _emailSettings.FromEmail));

        // Set To recipients
        if (!string.IsNullOrWhiteSpace(emailDto.MailTo))
        {
            message.To.Add(new MailboxAddress("", emailDto.MailTo));
        }

        if (emailDto.MailToList != null && emailDto.MailToList.Any())
        {
            foreach (var email in emailDto.MailToList)
            {
                if (!string.IsNullOrWhiteSpace(email))
                {
                    message.To.Add(new MailboxAddress("", email));
                }
            }
        }

        // Set CC recipients
        if (emailDto.EmailCC != null && emailDto.EmailCC.Any())
        {
            foreach (var email in emailDto.EmailCC)
            {
                if (!string.IsNullOrWhiteSpace(email))
                {
                    message.Cc.Add(new MailboxAddress("", email));
                }
            }
        }

        // Set BCC recipients
        if (emailDto.EmailBCC != null && emailDto.EmailBCC.Any())
        {
            foreach (var email in emailDto.EmailBCC)
            {
                if (!string.IsNullOrWhiteSpace(email))
                {
                    message.Bcc.Add(new MailboxAddress("", email));
                }
            }
        }

        // Set Subject
        message.Subject = emailDto.Subject;

        // Set Priority
        if (!string.IsNullOrWhiteSpace(emailDto.Priority))
        {
            message.Priority = emailDto.Priority.ToLower() switch
            {
                "high" => MessagePriority.Urgent,
                "low" => MessagePriority.NonUrgent,
                _ => MessagePriority.Normal
            };
        }

        // Create body builder
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = emailDto.Body,
            TextBody = emailDto.Body
        };

        // Add attachments from byte arrays
        if (emailDto.Attachments != null && emailDto.Attachments.Any())
        {
            foreach (var attachment in emailDto.Attachments)
            {
                if (attachment != null && attachment.Content != null && attachment.Content.Length > 0)
                {
                    using var stream = new MemoryStream(attachment.Content);
                    bodyBuilder.Attachments.Add(attachment.FileName, stream, ContentType.Parse(attachment.ContentType));
                }
            }
        }

        // Add attachments from file paths
        if (emailDto.DocumentsList != null && emailDto.DocumentsList.Any())
        {
            foreach (var document in emailDto.DocumentsList)
            {
                if (!string.IsNullOrWhiteSpace(document.FilePath) && File.Exists(document.FilePath))
                {
                    using var fileStream = File.OpenRead(document.FilePath);
                    bodyBuilder.Attachments.Add(document.FileName, fileStream, ContentType.Parse(document.ContentType));
                }
            }
        }

        message.Body = bodyBuilder.ToMessageBody();

        return message;
    }
}

public class SerializableEmailDto
{
    public string MailTo { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<AttachmentData>? Attachments { get; set; }
    public List<DocumentsInfo> DocumentsList { get; set; } = new List<DocumentsInfo>();
    public List<string> MailToList { get; set; } = new List<string>();
    public List<string> EmailCC { get; set; } = new List<string>();
    public List<string> EmailBCC { get; set; } = new List<string>();
    public string Priority { get; set; } = "Normal";
}

public class AttachmentData
{
    public string FileName { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = "application/octet-stream";
}
