namespace Application.Features.Tags.Commands.DeleteTag;

public class DeleteTagCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<DeleteTagCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await unitOfWork.Tags.GetTableNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (tag is null)
            return NotFound<string>("Tag not found");

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.Tags.DeleteAsync(tag, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BadRequest<string>($"Failed to delete tag: {ex.Message}");
        }
    }
}

