namespace Application.Features.VariantAttributes.Commands.AddVariantAttribute;

public class AddVariantAttributeCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<AddVariantAttributeCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddVariantAttributeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var variantAttribute = new VariantAttribute
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive
            };

            await unitOfWork.VariantAttributes.AddAsync(variantAttribute, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(VariantAttributeErrors.DuplicatedVariantAttributeName());
        }
    }
}
