namespace Application.Features.Tags.Commands.EditTag;

public class EditTagCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<EditTagCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await unitOfWork.Tags.GetTableAsTracking()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (tag is null)
            return NotFound<string>("Tag not found");

        var exists = await unitOfWork.Tags.GetTableNoTracking()
            .AnyAsync(t => t.Name == request.Name && t.Id != request.Id, cancellationToken);

        if (exists)
            return BadRequest<string>("Tag with this name already exists");

        tag.Name = request.Name.Trim();
        tag.IsActive = request.IsActive;
        tag.ModifiedTime = DateTimeOffset.UtcNow;

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.Tags.UpdateAsync(tag, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Edit(tag.Id.ToString(), "Tag updated successfully");
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BadRequest<string>($"Failed to update tag: {ex.Message}");
        }
    }
}

