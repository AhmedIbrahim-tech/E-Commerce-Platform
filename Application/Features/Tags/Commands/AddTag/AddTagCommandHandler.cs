namespace Application.Features.Tags.Commands.AddTag;

public class AddTagCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<AddTagCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddTagCommand request, CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.Tags.GetTableNoTracking()
            .AnyAsync(t => t.Name == request.Name, cancellationToken);

        if (exists)
            return BadRequest<string>("Tag with this name already exists");

        var tag = new Tag
        {
            Name = request.Name.Trim(),
            IsActive = request.IsActive
        };

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.Tags.AddAsync(tag, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Created(tag.Id.ToString(), "Tag created successfully");
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BadRequest<string>($"Failed to create tag: {ex.Message}");
        }
    }
}

