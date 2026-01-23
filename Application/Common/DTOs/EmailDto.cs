namespace Application.Common.DTOs;

public class EmailDto
{
    public string MailTo { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public IList<IFormFile> Attachments { get; set; } = new List<IFormFile>();
    public IList<DocumentsInfo> DocumentsList { get; set; } = new List<DocumentsInfo>();
    public IEnumerable<string> MailToList { get; set; } = new List<string>();
    public IEnumerable<string> EmailCC { get; set; } = new List<string>();
    public IEnumerable<string> EmailBCC { get; set; } = new List<string>();
    public string Priority { get; set; } = "Normal";
}
