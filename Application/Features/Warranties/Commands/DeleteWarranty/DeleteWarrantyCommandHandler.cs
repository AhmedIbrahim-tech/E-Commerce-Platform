namespace Application.Features.Warranties.Commands.DeleteWarranty;

public class DeleteWarrantyCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor) : ApiResponseHandler(),
    IRequestHandler<DeleteWarrantyCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteWarrantyCommand request, CancellationToken cancellationToken)
    {
        var warranty = await unitOfWork.Warranties.GetTableAsTracking()
            .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        if (warranty == null)
            return NotFound<string>("Warranty not found");

        var userIdClaim = httpContextAccessor.HttpContext?.User?.Claims
            ?.FirstOrDefault(c => c.Type == "Id");

        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            warranty.MarkDeleted(userId);
        }
        else
        {
            warranty.MarkDeleted(Guid.Empty);
        }

        await unitOfWork.Warranties.UpdateAsync(warranty, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Deleted<string>("Warranty deleted successfully");
    }
}
