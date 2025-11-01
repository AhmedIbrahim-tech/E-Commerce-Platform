namespace Domain.Helpers
{
    public class IdentityTablesSettings
    {
        public string Users { get; set; } = "AspNetUsers";
        public string Roles { get; set; } = "AspNetRoles";
        public string UserClaims { get; set; } = "AspNetUserClaims";
        public string UserRoles { get; set; } = "AspNetUserRoles";
        public string UserLogins { get; set; } = "AspNetUserLogins";
        public string RoleClaims { get; set; } = "AspNetRoleClaims";
        public string UserTokens { get; set; } = "AspNetUserTokens";
    }
}
