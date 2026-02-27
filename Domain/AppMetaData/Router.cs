namespace Domain.AppMetaData
{
    public static class Router
    {
        public const string SingleRoute = "{id}";

        public const string Root = "api";
        public const string Version = "v1";
        public const string Rule = Root + "/" + Version + "/";

        public static class Authentication
        {
            public const string Prefix = Rule + "authenticate/";
            public const string SignIn = Prefix + "signIn";
            public const string SignInViaGoogle = Prefix + "signInViaGoogle";
            public const string RefreshToken = Prefix + "refreshToken";
            public const string ValidateToken = Prefix + "validateToken";
            public const string SendResetPasswordCode = Prefix + "sendResetPasswordCode";
            public const string ConfirmResetPasswordCode = Prefix + "confirmResetPasswordCode";
            public const string ResetPassword = Prefix + "resetPassword";
            public const string ConfirmEmail = "api/authenticate/confirmEmail";
            public const string TwoStepVerification = Prefix + "twoStepVerification";
            public const string Logout = Prefix + "logout";
            public const string LogoutAll = Prefix + "logoutAll";
        }

        public static class Authorization
        {
            public const string Prefix = Rule + "authorize/";
            public const string Role = Prefix + "role/";
            public const string Claim = Prefix + "claim/";
            public const string CreateRole = Role + "create";
            public const string EditRole = Role + "edit";
            public const string DeleteRole = Role + SingleRoute;
            public const string GetAllRoles = Role + "getAll";
            public const string GetRoleById = Role + SingleRoute;
            public const string ManageUserRoles = Role + "manageUserRoles/" + SingleRoute;
            public const string UpdateUserRoles = Role + "updateUserRoles";
            public const string ManageUserClaims = Claim + "manageUserClaims/" + SingleRoute;
            public const string UpdateUserClaims = Claim + "updateUserClaims";
        }

        public static class UserRouting
        {
            public const string Prefix = Rule + "user/";
            public const string Register = Prefix + "register";
            public const string Profile = Prefix + "profile";
            public const string ChangePassword = Prefix + "changePassword";
            public const string CreateAdmin = Prefix + "createAdmin";
            public const string CreateVendor = Prefix + "createVendor";

            public const string MyActivitiesPaginated = Prefix + "activities/paginated";

            public const string Documents = Prefix + "documents";
            public const string DocumentById = Prefix + "documents/" + SingleRoute;
            public const string DocumentDownload = Prefix + "documents/" + SingleRoute + "/download";

            // Unified Users Management
            public const string UsersPaginated = Prefix + "users/paginated";
            public const string GetUserById = Prefix + "users/" + SingleRoute;
            public const string CreateUser = Prefix + "users/create";
            public const string EditUser = Prefix + "users/edit";
            public const string DeleteUser = Prefix + "users/" + SingleRoute;
            public const string ToggleUserStatus = Prefix + "users/" + SingleRoute + "/toggleStatus";
        }

        public static class CategoryRouting
        {
            public const string Prefix = Rule + "category/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + "getById";
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + "delete";
        }

        public static class SubCategoryRouting
        {
            public const string Prefix = Rule + "subCategory/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class BrandRouting
        {
            public const string Prefix = Rule + "brand/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class TagRouting
        {
            public const string Prefix = Rule + "tag/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + "getById";
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + "delete";
        }

        public static class UnitOfMeasureRouting
        {
            public const string Prefix = Rule + "unit/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class WarrantyRouting
        {
            public const string Prefix = Rule + "warranty/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + "getById";
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + "delete";
        }

        public static class VariantAttributeRouting
        {
            public const string Prefix = Rule + "variantAttribute/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class CouponRouting
        {
            public const string Prefix = Rule + "coupon/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class GiftCardRouting
        {
            public const string Prefix = Rule + "giftCard/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class DiscountRouting
        {
            public const string Prefix = Rule + "discount/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class AccountRouting
        {
            public const string Prefix = Rule + "account/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class ProductRouting
        {
            public const string Prefix = Rule + "product/";
            public const string Paginated = Prefix + "paginated";
            public const string GetSingle = Prefix + "getSingle";
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class OrderRouting
        {
            public const string Prefix = Rule + "order/";
            public const string GetMyOrders = Prefix + "getMyOrders";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Delete = Prefix + SingleRoute;
            public const string PlaceOrder = Prefix + "placeOrder/" + SingleRoute;
        }

        public static class CartRouting
        {
            public const string Prefix = Rule + "cart/";
            public const string GetById = Prefix + SingleRoute;
            public const string GetMyCart = Prefix + "myCart";
            public const string AddToCart = Prefix + "addToCart";
            public const string UpdateItemQuantity = Prefix + "updateItemQuantity";
            public const string RemoveFromCart = Prefix + "removeFromCart/" + SingleRoute;
            public const string Delete = Prefix + SingleRoute;
        }

        public static class CustomerRouting
        {
            public const string Prefix = Rule + "customer/";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
            public const string ToggleStatus = Prefix + "toggleStatus/" + SingleRoute;
        }

        public static class VendorRouting
        {
            public const string Prefix = Rule + "vendor/";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
            public const string ToggleStatus = Prefix + "toggleStatus";
        }

        public static class AdminRouting
        {
            public const string Prefix = Rule + "admin/";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
            public const string ToggleStatus = Prefix + "toggleStatus";
        }

        public static class EmployeeRouting
        {
            public const string Prefix = Rule + "employee/";
            public const string GetAll = Prefix + "getAll";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class PaymentRouting
        {
            public const string Prefix = Rule + "payment/";
            public const string SetPaymentMethod = Prefix + "setPaymentMethod";
            public const string ServerCallback = Prefix + "serverCallback";
            public const string PaymobCallback = Prefix + "paymobCallback";
        }

        public static class DeliveryRouting
        {
            public const string Prefix = Rule + "delivery/";
            public const string SetDeliveryMethod = Prefix + "setDeliveryMethod";
            public const string EditDeliveryMethod = Prefix + "editDeliveryMethod";
        }

        public static class ShippingAddressRouting
        {
            public const string Prefix = Rule + "shippingAddress/";
            public const string GetAll = Prefix + "getAll";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
            public const string SetShippingAddress = Prefix + "setShippingAddress";
        }

        public static class ReviewRouting
        {
            public const string Prefix = Rule + "review/";
            public const string Paginated = Prefix + "paginated";
            public const string Create = Prefix + "create";
            public const string Edit = Prefix + "edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class NotificationsRouting
        {
            public const string Prefix = Rule + "notifications/";
            public const string List = Prefix;
            public const string Paginated = Prefix + "paginated";
            public const string MarkAsRead = Prefix + "markAsRead/" + SingleRoute;
            public const string MarkAllAsRead = Prefix + "markAllAsRead";
            public const string MarkRead = Prefix + "mark-read";
        }

        public static class AuditLogRouting
        {
            public const string Prefix = Rule + "auditLog/";
            public const string Paginated = Prefix + "paginated";
            public const string GetById = Prefix + SingleRoute;
        }

        public static class LookUpsRouting
        {
            public const string Prefix = Rule + "lookups/";
            public const string Categories = Prefix + "categories";
            public const string SubCategories = Prefix + "subCategories";
            public const string SubCategoriesByCategory = Prefix + "subCategoriesByCategory";
            public const string Brands = Prefix + "brands";
            public const string UnitOfMeasures = Prefix + "unitOfMeasures";
            public const string Warranties = Prefix + "warranties";
            public const string VariantAttributes = Prefix + "variantAttributes";
            public const string Roles = Prefix + "roles";

            public const string ProductPublishStatuses = Prefix + "productPublishStatuses";
            public const string ProductVisibilities = Prefix + "productVisibilities";
            public const string ProductTypes = Prefix + "productTypes";
            public const string SellingTypes = Prefix + "sellingTypes";
            public const string TaxTypes = Prefix + "taxTypes";
            public const string DiscountTypes = Prefix + "discountTypes";
            public const string Tags = Prefix + "tags";
        }
    }
}
