namespace Application.Features.Warranties.Queries.GetWarrantyById;

public class GetWarrantyByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetWarrantyByIdQuery, ApiResponse<GetWarrantyByIdResponse>>
{
    public async Task<ApiResponse<GetWarrantyByIdResponse>> Handle(GetWarrantyByIdQuery request, CancellationToken cancellationToken)
    {
        var warranty = await unitOfWork.Warranties.GetTableNoTracking()
            .Where(w => w.Id == request.Id)
            .Select(w => new GetWarrantyByIdResponse(
                w.Id,
                w.Name,
                w.Description,
                w.Duration,
                w.DurationPeriod,
                w.IsActive,
                w.CreatedTime,
                w.ModifiedTime
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (warranty == null)
            return NotFound<GetWarrantyByIdResponse>("Warranty not found");

        return Success(warranty);
    }
}
