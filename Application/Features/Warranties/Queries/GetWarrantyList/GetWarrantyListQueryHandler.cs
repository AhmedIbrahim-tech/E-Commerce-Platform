namespace Application.Features.Warranties.Queries.GetWarrantyList;

public class GetWarrantyListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetWarrantyListQuery, ApiResponse<List<GetWarrantyListResponse>>>
{
    public async Task<ApiResponse<List<GetWarrantyListResponse>>> Handle(GetWarrantyListQuery request, CancellationToken cancellationToken)
    {
        var warranties = await unitOfWork.Warranties.GetTableNoTracking()
            .Where(w => w.IsActive)
            .OrderBy(w => w.Name)
            .Select(w => new GetWarrantyListResponse(
                w.Id,
                w.Name,
                w.Description,
                w.Duration,
                w.DurationPeriod,
                w.IsActive
            ))
            .ToListAsync(cancellationToken);

        return Success(warranties);
    }
}
