using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.VariantAttributes.Queries.GetVariantAttributePaginatedList;

public class GetVariantAttributePaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetVariantAttributePaginatedListQuery, PaginatedResult<GetVariantAttributePaginatedListResponse>>
{
    public async Task<PaginatedResult<GetVariantAttributePaginatedListResponse>> Handle(GetVariantAttributePaginatedListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<VariantAttribute, GetVariantAttributePaginatedListResponse>> expression = va => new GetVariantAttributePaginatedListResponse(
            va.Id,
            va.Name,
            va.Description,
            va.IsActive,
            va.CreatedTime
        );

        var queryable = unitOfWork.VariantAttributes.GetTableNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(va => va.Name.Contains(request.Search!) || 
                (va.Description != null && va.Description.Contains(request.Search!)));

        queryable = request.SortBy switch
        {
            VariantAttributeSortingEnum.NameAsc => queryable.OrderBy(va => va.Name),
            VariantAttributeSortingEnum.NameDesc => queryable.OrderByDescending(va => va.Name),
            VariantAttributeSortingEnum.CreatedTimeAsc => queryable.OrderBy(va => va.CreatedTime),
            VariantAttributeSortingEnum.CreatedTimeDesc => queryable.OrderByDescending(va => va.CreatedTime),
            _ => queryable.OrderBy(va => va.Name)
        };

        var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
        paginatedList.Meta = new { Count = paginatedList.Data.Count() };
        return paginatedList;
    }
}
