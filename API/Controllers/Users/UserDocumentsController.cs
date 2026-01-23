using API.Controllers.Base;
using Application.Features.Documents.Commands.DeleteMyDocument;
using Application.Features.Documents.Commands.UploadMyDocument;
using Application.Features.Documents.Queries.GetMyDocumentDownload;
using Application.Features.Documents.Queries.GetMyDocuments;
using Infrastructure.Data.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace API.Controllers.Users;

[Authorize]
public class UserDocumentsController(IWebHostEnvironment webHostEnvironment) : AppControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    [Authorize(Policy = Policies.Auth.ViewOwnProfile)]
    [HttpGet(Router.UserRouting.Documents)]
    public async Task<IActionResult> GetMyDocuments()
    {
        var response = await Mediator.Send(new GetMyDocumentsQuery());
        return NewResult(response);
    }

    [Authorize(Policy = Policies.Auth.EditOwnProfile)]
    [HttpPost(Router.UserRouting.Documents)]
    public async Task<IActionResult> UploadMyDocument([FromForm] UploadMyDocumentCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Policy = Policies.Auth.EditOwnProfile)]
    [HttpDelete(Router.UserRouting.DocumentById)]
    public async Task<IActionResult> DeleteMyDocument([FromRoute] Guid id)
    {
        var response = await Mediator.Send(new DeleteMyDocumentCommand(id));
        return NewResult(response);
    }

    [Authorize(Policy = Policies.Auth.ViewOwnProfile)]
    [HttpGet(Router.UserRouting.DocumentDownload)]
    public async Task<IActionResult> DownloadMyDocument([FromRoute] Guid id)
    {
        var response = await Mediator.Send(new GetMyDocumentDownloadQuery(id));
        if (!response.Succeeded || response.Data == null)
            return NewResult(response);

        var relativePath = response.Data.RelativePath.Trim().TrimStart('/');
        var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));

        if (!System.IO.File.Exists(fullPath))
            return NotFound();

        return PhysicalFile(fullPath, response.Data.ContentType, response.Data.FileName);
    }
}

