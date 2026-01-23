namespace Application.Features.Warranties.Commands.EditWarranty;

public class EditWarrantyCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<EditWarrantyCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditWarrantyCommand request, CancellationToken cancellationToken)
    {
        var warranty = await unitOfWork.Warranties.GetTableAsTracking()
            .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        if (warranty == null)
            return NotFound<string>("Warranty not found");

        warranty.Name = request.Name;
        warranty.Description = request.Description;
        warranty.Duration = request.Duration;
        warranty.DurationPeriod = request.DurationPeriod;
        warranty.IsActive = request.IsActive;

        await unitOfWork.Warranties.UpdateAsync(warranty, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Edit(warranty.Id.ToString(), "Warranty updated successfully");
    }
}
