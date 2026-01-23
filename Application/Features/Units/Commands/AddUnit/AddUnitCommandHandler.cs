namespace Application.Features.Units.Commands.AddUnit;

public class AddUnitCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<AddUnitCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddUnitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var unit = new UnitOfMeasure
            {
                Name = request.Name,
                ShortName = request.ShortName,
                Description = request.Description,
                IsActive = request.IsActive
            };

            await unitOfWork.UnitOfMeasures.AddAsync(unit, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(UnitOfMeasureErrors.DuplicatedUnitOfMeasureName());
        }
    }
}
