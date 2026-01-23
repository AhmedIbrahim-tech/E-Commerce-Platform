namespace Application.Common.DTOs;

public class DocStreamDto
{
    public string FileName { get; set; } = string.Empty;
    public Stream ContentStream { get; set; } = null!;
    public string ContentType { get; set; } = "application/octet-stream";
}
