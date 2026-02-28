import { API_URL } from '@/config/apiConfig';

export const SingleRoute = "{id}";
export const Root = "api";
export const Version = "v1";
export const Rule = `${API_URL}/${Root}/${Version}/`;

export const Router = {
    SingleRoute,
    Root,
    Version,
    Rule,

    Authentication: {
        Prefix: `${Rule}authenticate/`,
        SignIn: `${Rule}authenticate/signIn`,
        SignInViaGoogle: `${Rule}authenticate/signInViaGoogle`,
        RefreshToken: `${Rule}authenticate/refreshToken`,
        ValidateToken: `${Rule}authenticate/validateToken`,
        SendResetPasswordCode: `${Rule}authenticate/sendResetPasswordCode`,
        ConfirmResetPasswordCode: `${Rule}authenticate/confirmResetPasswordCode`,
        ResetPassword: `${Rule}authenticate/resetPassword`,
        ConfirmEmail: `${API_URL}/api/authenticate/confirmEmail`,
        TwoStepVerification: `${Rule}authenticate/twoStepVerification`,
        Logout: `${Rule}authenticate/logout`,
        LogoutAll: `${Rule}authenticate/logoutAll`,
        Register: `${Rule}authenticate/register`,
        ChangePassword: `${Rule}authenticate/changePassword`,
    },

    Authorization: {
        Prefix: `${Rule}authorize/`,
        Role: `${Rule}authorize/role/`,
        Claim: `${Rule}authorize/claim/`,
        CreateRole: `${Rule}authorize/role/create`,
        EditRole: `${Rule}authorize/role/edit`,
        DeleteRole: `${Rule}authorize/role/${SingleRoute}`,
        GetAllRoles: `${Rule}authorize/role/getAll`,
        GetRoleById: `${Rule}authorize/role/${SingleRoute}`,
        ManageUserRoles: `${Rule}authorize/role/manageUserRoles/${SingleRoute}`,
        UpdateUserRoles: `${Rule}authorize/role/updateUserRoles`,
        ManageUserClaims: `${Rule}authorize/claim/manageUserClaims/${SingleRoute}`,
        UpdateUserClaims: `${Rule}authorize/claim/updateUserClaims`,
    },

    UserRouting: {
        Prefix: `${Rule}user/`,
        Profile: `${Rule}user/profile`,
        CreateAdmin: `${Rule}user/createAdmin`,
        CreateVendor: `${Rule}user/createVendor`,
        UsersPaginated: `${Rule}user/users/paginated`,
        GetUserById: `${Rule}user/users/${SingleRoute}`,
        CreateUser: `${Rule}user/users/create`,
        EditUser: `${Rule}user/users/edit`,
        DeleteUser: `${Rule}user/users/${SingleRoute}`,
        ToggleUserStatus: `${Rule}user/users/${SingleRoute}/toggleStatus`,
    },

    CategoryRouting: {
        Prefix: `${Rule}category/`,
        GetAll: `${Rule}category/getAll`,
        Paginated: `${Rule}category/paginated`,
        GetById: `${Rule}category/getById`,
        Create: `${Rule}category/create`,
        Edit: `${Rule}category/edit`,
        Delete: `${Rule}category/delete`,
    },

    SubCategoryRouting: {
        Prefix: `${Rule}subCategory/`,
        GetAll: `${Rule}subCategory/getAll`,
        Paginated: `${Rule}subCategory/paginated`,
        GetById: `${Rule}subCategory/${SingleRoute}`,
        Create: `${Rule}subCategory/create`,
        Edit: `${Rule}subCategory/edit`,
        Delete: `${Rule}subCategory/${SingleRoute}`,
    },

    BrandRouting: {
        Prefix: `${Rule}brand/`,
        GetAll: `${Rule}brand/getAll`,
        Paginated: `${Rule}brand/paginated`,
        GetById: `${Rule}brand/${SingleRoute}`,
        Create: `${Rule}brand/create`,
        Edit: `${Rule}brand/edit`,
        Delete: `${Rule}brand/${SingleRoute}`,
    },

    TagRouting: {
        Prefix: `${Rule}tag/`,
        GetAll: `${Rule}tag/getAll`,
        Paginated: `${Rule}tag/paginated`,
        GetById: `${Rule}tag/getById`,
        Create: `${Rule}tag/create`,
        Edit: `${Rule}tag/edit`,
        Delete: `${Rule}tag/delete`,
    },

    UnitOfMeasureRouting: {
        Prefix: `${Rule}unit/`,
        GetAll: `${Rule}unit/getAll`,
        Paginated: `${Rule}unit/paginated`,
        GetById: `${Rule}unit/${SingleRoute}`,
        Create: `${Rule}unit/create`,
        Edit: `${Rule}unit/edit`,
        Delete: `${Rule}unit/${SingleRoute}`,
    },

    WarrantyRouting: {
        Prefix: `${Rule}warranty/`,
        GetAll: `${Rule}warranty/getAll`,
        Paginated: `${Rule}warranty/paginated`,
        GetById: `${Rule}warranty/getById`,
        Create: `${Rule}warranty/create`,
        Edit: `${Rule}warranty/edit`,
        Delete: `${Rule}warranty/delete`,
    },

    VariantAttributeRouting: {
        Prefix: `${Rule}variantAttribute/`,
        GetAll: `${Rule}variantAttribute/getAll`,
        Paginated: `${Rule}variantAttribute/paginated`,
        GetById: `${Rule}variantAttribute/${SingleRoute}`,
        Create: `${Rule}variantAttribute/create`,
        Edit: `${Rule}variantAttribute/edit`,
        Delete: `${Rule}variantAttribute/${SingleRoute}`,
    },

    CouponRouting: {
        Prefix: `${Rule}coupon/`,
        GetAll: `${Rule}coupon/getAll`,
        Paginated: `${Rule}coupon/paginated`,
        GetById: `${Rule}coupon/${SingleRoute}`,
        Create: `${Rule}coupon/create`,
        Edit: `${Rule}coupon/edit`,
        Delete: `${Rule}coupon/${SingleRoute}`,
    },

    GiftCardRouting: {
        Prefix: `${Rule}giftCard/`,
        GetAll: `${Rule}giftCard/getAll`,
        Paginated: `${Rule}giftCard/paginated`,
        GetById: `${Rule}giftCard/${SingleRoute}`,
        Create: `${Rule}giftCard/create`,
        Edit: `${Rule}giftCard/edit`,
        Delete: `${Rule}giftCard/${SingleRoute}`,
    },

    DiscountRouting: {
        Prefix: `${Rule}discount/`,
        GetAll: `${Rule}discount/getAll`,
        Paginated: `${Rule}discount/paginated`,
        GetById: `${Rule}discount/${SingleRoute}`,
        Create: `${Rule}discount/create`,
        Edit: `${Rule}discount/edit`,
        Delete: `${Rule}discount/${SingleRoute}`,
    },

    AccountRouting: {
        Prefix: `${Rule}account/`,
        GetAll: `${Rule}account/getAll`,
        Paginated: `${Rule}account/paginated`,
        GetById: `${Rule}account/${SingleRoute}`,
        Create: `${Rule}account/create`,
        Edit: `${Rule}account/edit`,
        Delete: `${Rule}account/${SingleRoute}`,
    },

    ProductRouting: {
        Prefix: `${Rule}product/`,
        Paginated: `${Rule}product/paginated`,
        GetSingle: `${Rule}product/getSingle`,
        Create: `${Rule}product/create`,
        Edit: `${Rule}product/edit`,
        Delete: `${Rule}product/${SingleRoute}`,
    },

    OrderRouting: {
        Prefix: `${Rule}order/`,
        GetMyOrders: `${Rule}order/getMyOrders`,
        Paginated: `${Rule}order/paginated`,
        GetById: `${Rule}order/${SingleRoute}`,
        Create: `${Rule}order/create`,
        Delete: `${Rule}order/${SingleRoute}`,
        PlaceOrder: `${Rule}order/placeOrder/${SingleRoute}`,
    },

    CartRouting: {
        Prefix: `${Rule}cart/`,
        GetById: `${Rule}cart/${SingleRoute}`,
        GetMyCart: `${Rule}cart/myCart`,
        AddToCart: `${Rule}cart/addToCart`,
        UpdateItemQuantity: `${Rule}cart/updateItemQuantity`,
        RemoveFromCart: `${Rule}cart/removeFromCart/${SingleRoute}`,
        Delete: `${Rule}cart/${SingleRoute}`,
    },

    CustomerRouting: {
        Prefix: `${Rule}customer/`,
        Paginated: `${Rule}customer/paginated`,
        GetById: `${Rule}customer/${SingleRoute}`,
        Create: `${Rule}customer/create`,
        Edit: `${Rule}customer/edit`,
        Delete: `${Rule}customer/${SingleRoute}`,
        ToggleStatus: `${Rule}customer/toggleStatus/${SingleRoute}`,
    },

    VendorRouting: {
        Prefix: `${Rule}vendor/`,
        Paginated: `${Rule}vendor/paginated`,
        GetById: `${Rule}vendor/${SingleRoute}`,
        Create: `${Rule}vendor/create`,
        Edit: `${Rule}vendor/edit`,
        Delete: `${Rule}vendor/${SingleRoute}`,
        ToggleStatus: `${Rule}vendor/toggleStatus`,
    },

    AdminRouting: {
        Prefix: `${Rule}admin/`,
        Paginated: `${Rule}admin/paginated`,
        GetById: `${Rule}admin/${SingleRoute}`,
        Create: `${Rule}admin/create`,
        Edit: `${Rule}admin/edit`,
        Delete: `${Rule}admin/${SingleRoute}`,
        ToggleStatus: `${Rule}admin/toggleStatus`,
    },

    EmployeeRouting: {
        Prefix: `${Rule}employee/`,
        GetAll: `${Rule}employee/getAll`,
        Paginated: `${Rule}employee/paginated`,
        GetById: `${Rule}employee/${SingleRoute}`,
        Create: `${Rule}employee/create`,
        Edit: `${Rule}employee/edit`,
        Delete: `${Rule}employee/${SingleRoute}`,
    },

    PaymentRouting: {
        Prefix: `${Rule}payment/`,
        SetPaymentMethod: `${Rule}payment/setPaymentMethod`,
        ServerCallback: `${Rule}payment/serverCallback`,
        PaymobCallback: `${Rule}payment/paymobCallback`,
    },

    DeliveryRouting: {
        Prefix: `${Rule}delivery/`,
        SetDeliveryMethod: `${Rule}delivery/setDeliveryMethod`,
        EditDeliveryMethod: `${Rule}delivery/editDeliveryMethod`,
    },

    ShippingAddressRouting: {
        Prefix: `${Rule}shippingAddress/`,
        GetAll: `${Rule}shippingAddress/getAll`,
        GetById: `${Rule}shippingAddress/${SingleRoute}`,
        Create: `${Rule}shippingAddress/create`,
        Edit: `${Rule}shippingAddress/edit`,
        Delete: `${Rule}shippingAddress/${SingleRoute}`,
        SetShippingAddress: `${Rule}shippingAddress/setShippingAddress`,
    },

    ReviewRouting: {
        Prefix: `${Rule}review/`,
        Paginated: `${Rule}review/paginated`,
        Create: `${Rule}review/create`,
        Edit: `${Rule}review/edit`,
        Delete: `${Rule}review/${SingleRoute}`,
    },

    NotificationsRouting: {
        Prefix: `${Rule}notifications/`,
        List: `${Rule}notifications/`,
        Paginated: `${Rule}notifications/paginated`,
        MarkAsRead: `${Rule}notifications/markAsRead/${SingleRoute}`,
        MarkAllAsRead: `${Rule}notifications/markAllAsRead`,
        MarkRead: `${Rule}notifications/mark-read`,
    },

    AuditLogRouting: {
        Prefix: `${Rule}auditLog/`,
        Paginated: `${Rule}auditLog/paginated`,
        GetById: `${Rule}auditLog/${SingleRoute}`,
    },

    LookUpsRouting: {
        Prefix: `${Rule}lookups/`,
        Categories: `${Rule}lookups/categories`,
        SubCategories: `${Rule}lookups/subCategories`,
        SubCategoriesByCategory: `${Rule}lookups/subCategoriesByCategory`,
        Brands: `${Rule}lookups/brands`,
        UnitOfMeasures: `${Rule}lookups/unitOfMeasures`,
        Warranties: `${Rule}lookups/warranties`,
        VariantAttributes: `${Rule}lookups/variantAttributes`,
        Roles: `${Rule}lookups/roles`,
        ProductPublishStatuses: `${Rule}lookups/productPublishStatuses`,
        ProductVisibilities: `${Rule}lookups/productVisibilities`,
        ProductTypes: `${Rule}lookups/productTypes`,
        SellingTypes: `${Rule}lookups/sellingTypes`,
        TaxTypes: `${Rule}lookups/taxTypes`,
        DiscountTypes: `${Rule}lookups/discountTypes`,
        Tags: `${Rule}lookups/tags`,
    },
};
