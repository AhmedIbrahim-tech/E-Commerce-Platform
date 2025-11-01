
namespace Service.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailsService _emailsService;
        private readonly ApplicationContext _dbContext;
        private readonly IUrlHelper _urlHelper;
        #endregion

        #region Constructors
        public ApplicationUserService(UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            IEmailsService emailsService,
            ApplicationContext dbContext,
            IUrlHelper urlHelper)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailsService = emailsService;
            _dbContext = dbContext;
            _urlHelper = urlHelper;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddUserAsync(User user, string password)
        {
            var trans = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                //Check if the user email already exists
                var userEmailIsExistResult = await _userManager.FindByEmailAsync(user.Email!);
                if (userEmailIsExistResult != null) return "EmailIsExist";

                //Check if the user name already exists
                var userByUserName = await _userManager.FindByNameAsync(user.UserName!);
                if (userByUserName != null) return "UserNameIsExist";

                user.EmailConfirmed = true;

                var createResult = await _userManager.CreateAsync(user, password);

                if (!createResult.Succeeded)
                    return string.Join(",", createResult.Errors.Select(x => x.Description).ToList());

                //Add default role "Customer"
                var addToRoleResult = await _userManager.AddToRoleAsync(user, "Customer");
                if (!addToRoleResult.Succeeded)
                    return "FailedToAddNewRoles";

                //Add default customer policies
                var claims = new List<Claim>
                {
                    new Claim("Edit Customer", "True"),
                    new Claim("Get Customer", "True")
                };
                var addDefaultClaimsResult = await _userManager.AddClaimsAsync(user, claims);
                if (!addDefaultClaimsResult.Succeeded)
                    return "FailedToAddNewClaims";

                //Send confirmation email
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var resquestAccessor = _httpContextAccessor.HttpContext!.Request;

                var returnUrl = resquestAccessor.Scheme + "://" + resquestAccessor.Host
                    + _urlHelper.Action("ConfirmEmail", "Authentication", new { userId = user.Id, code = code });

                /// $"/Api/V1/Authentication/ConfirmEmail?userId={user.Id}&code={code}";
                //var message = $"To Confirm Email Click Link: <a href='{returnUrl}'>Link Of Confirmation</a>";

                //Message or body

                //var sendEmailResult = await _emailsService.SendEmailAsync(user.Email!, returnUrl, EmailType.ConfirmEmail);
                //if (sendEmailResult == "Failed") return "SendEmailFailed";

                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                return "Failed";
            }
        }
        #endregion
    }
}
