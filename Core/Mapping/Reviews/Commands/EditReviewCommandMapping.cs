using Core.Features.Reviews.Commands.Models;

namespace Core.Mapping.Reviews
{
    public partial class ReviewProfile
    {
        public void EditReviewCommandMapping()
        {
            CreateMap<EditReviewCommand, Review>();
        }
    }
}
