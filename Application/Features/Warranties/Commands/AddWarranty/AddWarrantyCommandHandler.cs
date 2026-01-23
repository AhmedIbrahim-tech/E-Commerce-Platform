namespace Application.Features.Warranties.Commands.AddWarranty;

public class AddWarrantyCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<AddWarrantyCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddWarrantyCommand request, CancellationToken cancellationToken)
    {
        var warranty = new Warranty
        {
            Name = request.Name,
            Description = request.Description,
            Duration = request.Duration,
            DurationPeriod = request.DurationPeriod,
            IsActive = request.IsActive
        };

        await unitOfWork.Warranties.AddAsync(warranty, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Created(warranty.Id.ToString(), "Warranty created successfully");
    }
}
