using Core.Features.Reviews.Commands.Models;

namespace Core.Mapping.Reviews
{
    public partial class ReviewProfile
    {
        public void AddReviewCommandMapping()
        {
            CreateMap<AddReviewCommand, Review>();
        }
    }
}
