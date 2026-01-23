/**
 * API Routes Constants
 * Mirrors the backend Router.cs structure
 */

const ROOT = 'api';
const VERSION = 'v1';
const RULE = `${ROOT}/${VERSION}/`;

// Helper function to replace {id} placeholder with actual ID
export const replaceId = (route: string, id: string): string => {
  return route.replace('{id}', id);
};

export const Routes = {
  Authentication: {
    Prefix: `${RULE}authenticate/`,
    SignIn: `${RULE}authenticate/signIn`,
    SignInViaGoogle: `${RULE}authenticate/signInViaGoogle`,
    RefreshToken: `${RULE}authenticate/refreshToken`,
    ValidateToken: `${RULE}authenticate/validateToken`,
    SendResetPasswordCode: `${RULE}authenticate/sendResetPasswordCode`,
    ConfirmResetPasswordCode: `${RULE}authenticate/confirmResetPasswordCode`,
    ResetPassword: `${RULE}authenticate/resetPassword`,
    ConfirmEmail: 'api/authenticate/confirmEmail',
    Logout: `${RULE}authenticate/logout`,
    LogoutAll: `${RULE}authenticate/logoutAll`,
  },

  Authorization: {
    Prefix: `${RULE}authorize/`,
    Role: `${RULE}authorize/role/`,
    Claim: `${RULE}authorize/claim/`,
    CreateRole: `${RULE}authorize/role/create`,
    EditRole: `${RULE}authorize/role/edit`,
    DeleteRole: (id: string) => `${RULE}authorize/role/${id}`,
    GetAllRoles: `${RULE}authorize/role/getAll`,
    GetRoleById: (id: string) => `${RULE}authorize/role/${id}`,
    ManageUserRoles: (id: string) => `${RULE}authorize/role/manageUserRoles/${id}`,
    UpdateUserRoles: `${RULE}authorize/role/updateUserRoles`,
    ManageUserClaims: (id: string) => `${RULE}authorize/claim/manageUserClaims/${id}`,
    UpdateUserClaims: `${RULE}authorize/claim/updateUserClaims`,
  },

  User: {
    Prefix: `${RULE}user/`,
    Register: `${RULE}user/register`,
    Profile: `${RULE}user/profile`,
    ChangePassword: `${RULE}user/changePassword`,
    CreateAdmin: `${RULE}user/createAdmin`,
    CreateVendor: `${RULE}user/createVendor`,
    MyActivitiesPaginated: `${RULE}user/activities/paginated`,
    Documents: `${RULE}user/documents`,
    DocumentById: (id: string) => `${RULE}user/documents/${id}`,
    DocumentDownload: (id: string) => `${RULE}user/documents/${id}/download`,
    // Unified Users Management
    UsersPaginated: `${RULE}user/users/paginated`,
    GetUserById: `${RULE}user/users/getById`,
    CreateUser: `${RULE}user/users/create`,
    EditUser: `${RULE}user/users/edit`,
    DeleteUser: (id: string) => `${RULE}user/users/${id}`,
    ToggleUserStatus: `${RULE}user/users/toggleStatus`,
  },

  Category: {
    Prefix: `${RULE}category/`,
    GetAll: `${RULE}category/getAll`,
    Paginated: `${RULE}category/paginated`,
    GetById: `${RULE}category/getById`,
    Create: `${RULE}category/create`,
    Edit: `${RULE}category/edit`,
    Delete: `${RULE}category/delete`,
  },

  Product: {
    Prefix: `${RULE}product/`,
    Paginated: `${RULE}product/paginated`,
    GetSingle: `${RULE}product/getSingle`,
    Create: `${RULE}product/create`,
    Edit: `${RULE}product/edit`,
    Delete: (id: string) => `${RULE}product/${id}`,
  },

  Order: {
    Prefix: `${RULE}order/`,
    GetMyOrders: `${RULE}order/getMyOrders`,
    Paginated: `${RULE}order/paginated`,
    GetById: (id: string) => `${RULE}order/${id}`,
    Create: `${RULE}order/create`,
    Delete: (id: string) => `${RULE}order/${id}`,
    PlaceOrder: (id: string) => `${RULE}order/placeOrder/${id}`,
  },

  Cart: {
    Prefix: `${RULE}cart/`,
    GetById: (id: string) => `${RULE}cart/${id}`,
    GetMyCart: `${RULE}cart/myCart`,
    AddToCart: `${RULE}cart/addToCart`,
    UpdateItemQuantity: `${RULE}cart/updateItemQuantity`,
    RemoveFromCart: (id: string) => `${RULE}cart/removeFromCart/${id}`,
    Delete: (id: string) => `${RULE}cart/${id}`,
  },

  Customer: {
    Prefix: `${RULE}customer/`,
    Paginated: `${RULE}customer/paginated`,
    GetById: (id: string) => `${RULE}customer/${id}`,
    Create: `${RULE}customer/create`,
    Edit: `${RULE}customer/edit`,
    Delete: (id: string) => `${RULE}customer/${id}`,
    ToggleStatus: (id: string) => `${RULE}customer/toggleStatus/${id}`,
  },

  Vendor: {
    Prefix: `${RULE}vendor/`,
    Paginated: `${RULE}vendor/paginated`,
    GetById: (id: string) => `${RULE}vendor/${id}`,
    Create: `${RULE}vendor/create`,
    Edit: `${RULE}vendor/edit`,
    Delete: (id: string) => `${RULE}vendor/${id}`,
    ToggleStatus: (id: string) => `${RULE}vendor/toggleStatus/${id}`,
  },

  Admin: {
    Prefix: `${RULE}admin/`,
    Paginated: `${RULE}admin/paginated`,
    GetById: (id: string) => `${RULE}admin/${id}`,
    Create: `${RULE}admin/create`,
    Edit: `${RULE}admin/edit`,
    Delete: (id: string) => `${RULE}admin/${id}`,
    ToggleStatus: (id: string) => `${RULE}admin/toggleStatus/${id}`,
  },

  Employee: {
    Prefix: `${RULE}employee/`,
    GetAll: `${RULE}employee/getAll`,
    Paginated: `${RULE}employee/paginated`,
    GetById: (id: string) => `${RULE}employee/${id}`,
    Create: `${RULE}employee/create`,
    Edit: `${RULE}employee/edit`,
    Delete: (id: string) => `${RULE}employee/${id}`,
  },

  Payment: {
    Prefix: `${RULE}payment/`,
    SetPaymentMethod: `${RULE}payment/setPaymentMethod`,
    ServerCallback: `${RULE}payment/serverCallback`,
    PaymobCallback: `${RULE}payment/paymobCallback`,
  },

  Delivery: {
    Prefix: `${RULE}delivery/`,
    SetDeliveryMethod: `${RULE}delivery/setDeliveryMethod`,
    EditDeliveryMethod: `${RULE}delivery/editDeliveryMethod`,
  },

  ShippingAddress: {
    Prefix: `${RULE}shippingAddress/`,
    GetAll: `${RULE}shippingAddress/getAll`,
    GetById: (id: string) => `${RULE}shippingAddress/${id}`,
    Create: `${RULE}shippingAddress/create`,
    Edit: `${RULE}shippingAddress/edit`,
    Delete: (id: string) => `${RULE}shippingAddress/${id}`,
    SetShippingAddress: `${RULE}shippingAddress/setShippingAddress`,
  },

  Review: {
    Prefix: `${RULE}review/`,
    Paginated: `${RULE}review/paginated`,
    Create: `${RULE}review/create`,
    Edit: `${RULE}review/edit`,
    Delete: (id: string) => `${RULE}review/${id}`,
  },

  Notifications: {
    Prefix: `${RULE}notifications/`,
    List: `${RULE}notifications/`,
    Paginated: `${RULE}notifications/paginated`,
    MarkAsRead: (id: string) => `${RULE}notifications/markAsRead/${id}`,
    MarkAllAsRead: `${RULE}notifications/markAllAsRead`,
    MarkRead: `${RULE}notifications/mark-read`,
  },

  AuditLog: {
    Prefix: `${RULE}auditLog/`,
    Paginated: `${RULE}auditLog/paginated`,
    GetById: (id: string) => `${RULE}auditLog/${id}`,
  },

  SubCategory: {
    Prefix: `${RULE}subCategory/`,
    GetAll: `${RULE}subCategory/getAll`,
    Paginated: `${RULE}subCategory/paginated`,
    GetById: (id: string) => `${RULE}subCategory/${id}`,
    Create: `${RULE}subCategory/create`,
    Edit: `${RULE}subCategory/edit`,
    Delete: (id: string) => `${RULE}subCategory/${id}`,
  },

  Brand: {
    Prefix: `${RULE}brand/`,
    GetAll: `${RULE}brand/getAll`,
    Paginated: `${RULE}brand/paginated`,
    GetById: (id: string) => `${RULE}brand/${id}`,
    Create: `${RULE}brand/create`,
    Edit: `${RULE}brand/edit`,
    Delete: (id: string) => `${RULE}brand/${id}`,
  },

  Tag: {
    Prefix: `${RULE}tag/`,
    GetAll: `${RULE}tag/getAll`,
    Paginated: `${RULE}tag/paginated`,
    GetById: `${RULE}tag/getById`,
    Create: `${RULE}tag/create`,
    Edit: `${RULE}tag/edit`,
    Delete: `${RULE}tag/delete`,
  },

  Warranty: {
    Prefix: `${RULE}warranty/`,
    GetAll: `${RULE}warranty/getAll`,
    Paginated: `${RULE}warranty/paginated`,
    GetById: `${RULE}warranty/getById`,
    Create: `${RULE}warranty/create`,
    Edit: `${RULE}warranty/edit`,
    Delete: `${RULE}warranty/delete`,
  },

  Unit: {
    Prefix: `${RULE}unit/`,
    GetAll: `${RULE}unit/getAll`,
    Paginated: `${RULE}unit/paginated`,
    GetById: (id: string) => `${RULE}unit/${id}`,
    Create: `${RULE}unit/create`,
    Edit: `${RULE}unit/edit`,
    Delete: (id: string) => `${RULE}unit/${id}`,
  },

  VariantAttribute: {
    Prefix: `${RULE}variantAttribute/`,
    GetAll: `${RULE}variantAttribute/getAll`,
    Paginated: `${RULE}variantAttribute/paginated`,
    GetById: (id: string) => `${RULE}variantAttribute/${id}`,
    Create: `${RULE}variantAttribute/create`,
    Edit: `${RULE}variantAttribute/edit`,
    Delete: (id: string) => `${RULE}variantAttribute/${id}`,
  },

  Coupon: {
    Prefix: `${RULE}coupon/`,
    GetAll: `${RULE}coupon/getAll`,
    Paginated: `${RULE}coupon/paginated`,
    GetById: (id: string) => `${RULE}coupon/${id}`,
    Create: `${RULE}coupon/create`,
    Edit: `${RULE}coupon/edit`,
    Delete: (id: string) => `${RULE}coupon/${id}`,
  },

  GiftCard: {
    Prefix: `${RULE}giftCard/`,
    GetAll: `${RULE}giftCard/getAll`,
    Paginated: `${RULE}giftCard/paginated`,
    GetById: (id: string) => `${RULE}giftCard/${id}`,
    Create: `${RULE}giftCard/create`,
    Edit: `${RULE}giftCard/edit`,
    Delete: (id: string) => `${RULE}giftCard/${id}`,
  },

  Discount: {
    Prefix: `${RULE}discount/`,
    GetAll: `${RULE}discount/getAll`,
    Paginated: `${RULE}discount/paginated`,
    GetById: (id: string) => `${RULE}discount/${id}`,
    Create: `${RULE}discount/create`,
    Edit: `${RULE}discount/edit`,
    Delete: (id: string) => `${RULE}discount/${id}`,
  },

  Account: {
    Prefix: `${RULE}account/`,
    GetAll: `${RULE}account/getAll`,
    Paginated: `${RULE}account/paginated`,
    GetById: (id: string) => `${RULE}account/${id}`,
    Create: `${RULE}account/create`,
    Edit: `${RULE}account/edit`,
    Delete: (id: string) => `${RULE}account/${id}`,
  },

  LookUps: {
    Prefix: `${RULE}lookups/`,
    Categories: `${RULE}lookups/categories`,
    SubCategories: `${RULE}lookups/subCategories`,
    SubCategoriesByCategory: `${RULE}lookups/subCategoriesByCategory`,
    Brands: `${RULE}lookups/brands`,
    UnitOfMeasures: `${RULE}lookups/unitOfMeasures`,
    Warranties: `${RULE}lookups/warranties`,
    VariantAttributes: `${RULE}lookups/variantAttributes`,
    Roles: `${RULE}lookups/roles`,
    ProductPublishStatuses: `${RULE}lookups/productPublishStatuses`,
    ProductVisibilities: `${RULE}lookups/productVisibilities`,
    ProductTypes: `${RULE}lookups/productTypes`,
    SellingTypes: `${RULE}lookups/sellingTypes`,
    TaxTypes: `${RULE}lookups/taxTypes`,
    DiscountTypes: `${RULE}lookups/discountTypes`,
    Tags: `${RULE}lookups/tags`,
  },
} as const;
