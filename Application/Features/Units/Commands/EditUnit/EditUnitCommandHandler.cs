using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Units.Commands.EditUnit;

public class EditUnitCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<EditUnitCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await unitOfWork.UnitOfMeasures.GetTableNoTracking()
            .Where(u => u.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (unit == null) return new ApiResponse<string>(UnitOfMeasureErrors.UnitOfMeasureNotFound());

        unit.Name = request.Name;
        unit.ShortName = request.ShortName;
        unit.Description = request.Description;
        unit.IsActive = request.IsActive;

        try
        {
            await unitOfWork.UnitOfMeasures.UpdateAsync(unit, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(UnitOfMeasureErrors.DuplicatedUnitOfMeasureName());
        }
    }
}
