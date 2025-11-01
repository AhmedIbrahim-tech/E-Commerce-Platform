
namespace Core.Mapping.Reviews
{
    public partial class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            AddReviewCommandMapping();
            EditReviewCommandMapping();
        }
    }
}
