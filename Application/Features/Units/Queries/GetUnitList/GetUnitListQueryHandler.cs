using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Units.Queries.GetUnitList;

public class GetUnitListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetUnitListQuery, ApiResponse<List<GetUnitListResponse>>>
{
    public async Task<ApiResponse<List<GetUnitListResponse>>> Handle(GetUnitListQuery request, CancellationToken cancellationToken)
    {
        var unitList = await unitOfWork.UnitOfMeasures.GetTableNoTracking()
            .Where(u => u.IsActive)
            .Select(u => new GetUnitListResponse(
                u.Id,
                u.Name,
                u.ShortName,
                u.Description,
                u.IsActive,
                u.CreatedTime
            ))
            .ToListAsync(cancellationToken);

        return Success(unitList);
    }
}
