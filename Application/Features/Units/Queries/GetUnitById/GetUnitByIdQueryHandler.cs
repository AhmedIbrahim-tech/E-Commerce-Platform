using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Units.Queries.GetUnitById;

public class GetUnitByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetUnitByIdQuery, ApiResponse<GetUnitByIdResponse>>
{
    public async Task<ApiResponse<GetUnitByIdResponse>> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var unit = await unitOfWork.UnitOfMeasures.GetTableNoTracking()
            .Where(u => u.Id == request.Id)
            .Select(u => new GetUnitByIdResponse(
                u.Id,
                u.Name,
                u.ShortName,
                u.Description,
                u.IsActive,
                u.CreatedTime
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (unit == null)
            return NotFound<GetUnitByIdResponse>("Unit of measure not found");

        return Success(unit);
    }
}
