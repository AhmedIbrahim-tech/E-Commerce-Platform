namespace Domain.Entities.Identity
{
    public class User
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public Gender? Gender { get; set; }
        public Guid AppUserId { get; set; }
    }
}
