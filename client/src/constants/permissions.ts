export const Permissions = {
  Customer: {
    ViewProfile: 'Customer.ViewProfile',
    EditProfile: 'Customer.EditProfile',
    ViewList: 'Customer.ViewList',
    Delete: 'Customer.Delete',
  },
  Vendor: {
    ViewProfile: 'Vendor.ViewProfile',
    EditProfile: 'Vendor.EditProfile',
    ViewDashboard: 'Vendor.ViewDashboard',
    ViewList: 'Vendor.ViewList',
    Create: 'Vendor.Create',
    Delete: 'Vendor.Delete',
  },
  Admin: {
    ViewProfile: 'Admin.ViewProfile',
    EditProfile: 'Admin.EditProfile',
    ViewList: 'Admin.ViewList',
    Create: 'Admin.Create',
    Delete: 'Admin.Delete',
    ManageUsers: 'Admin.ManageUsers',
    ManageRoles: 'Admin.ManageRoles',
    ManageClaims: 'Admin.ManageClaims',
  },
  Auth: {
    ChangePassword: 'Auth.ChangePassword',
    ViewOwnProfile: 'Auth.ViewOwnProfile',
    EditOwnProfile: 'Auth.EditOwnProfile',
  },
  Product: {
    ViewList: 'Product.ViewList',
    View: 'Product.View',
    Create: 'Product.Create',
    Edit: 'Product.Edit',
    Delete: 'Product.Delete',
  },
  Category: {
    ViewList: 'Category.ViewList',
    View: 'Category.View',
    Create: 'Category.Create',
    Edit: 'Category.Edit',
    Delete: 'Category.Delete',
  },
  SubCategory: {
    ViewList: 'SubCategory.ViewList',
    View: 'SubCategory.View',
    Create: 'SubCategory.Create',
    Edit: 'SubCategory.Edit',
    Delete: 'SubCategory.Delete',
  },
  Brand: {
    ViewList: 'Brand.ViewList',
    View: 'Brand.View',
    Create: 'Brand.Create',
    Edit: 'Brand.Edit',
    Delete: 'Brand.Delete',
  },
  UnitOfMeasure: {
    ViewList: 'UnitOfMeasure.ViewList',
    View: 'UnitOfMeasure.View',
    Create: 'UnitOfMeasure.Create',
    Edit: 'UnitOfMeasure.Edit',
    Delete: 'UnitOfMeasure.Delete',
  },
  Warranty: {
    ViewList: 'Warranty.ViewList',
    View: 'Warranty.View',
    Create: 'Warranty.Create',
    Edit: 'Warranty.Edit',
    Delete: 'Warranty.Delete',
  },
  VariantAttribute: {
    ViewList: 'VariantAttribute.ViewList',
    View: 'VariantAttribute.View',
    Create: 'VariantAttribute.Create',
    Edit: 'VariantAttribute.Edit',
    Delete: 'VariantAttribute.Delete',
  },
  Account: {
    ViewList: 'Account.ViewList',
    View: 'Account.View',
    Create: 'Account.Create',
    Edit: 'Account.Edit',
    Delete: 'Account.Delete',
  },
  Coupon: {
    ViewList: 'Coupon.ViewList',
    View: 'Coupon.View',
    Create: 'Coupon.Create',
    Edit: 'Coupon.Edit',
    Delete: 'Coupon.Delete',
  },
  Discount: {
    ViewList: 'Discount.ViewList',
    View: 'Discount.View',
    Create: 'Discount.Create',
    Edit: 'Discount.Edit',
    Delete: 'Discount.Delete',
  },
  GiftCard: {
    ViewList: 'GiftCard.ViewList',
    View: 'GiftCard.View',
    Create: 'GiftCard.Create',
    Edit: 'GiftCard.Edit',
    Delete: 'GiftCard.Delete',
  },
} as const;

export type Permission = typeof Permissions[keyof typeof Permissions][keyof typeof Permissions[keyof typeof Permissions]];

export const PermissionGroups = [
  {
    name: 'Customer',
    label: 'Customer Permissions',
    permissions: [
      { key: Permissions.Customer.ViewProfile, label: 'View Profile' },
      { key: Permissions.Customer.EditProfile, label: 'Edit Profile' },
      { key: Permissions.Customer.ViewList, label: 'View List' },
      { key: Permissions.Customer.Delete, label: 'Delete' },
    ],
  },
  {
    name: 'Vendor',
    label: 'Vendor Permissions',
    permissions: [
      { key: Permissions.Vendor.ViewProfile, label: 'View Profile' },
      { key: Permissions.Vendor.EditProfile, label: 'Edit Profile' },
      { key: Permissions.Vendor.ViewDashboard, label: 'View Dashboard' },
      { key: Permissions.Vendor.ViewList, label: 'View List' },
      { key: Permissions.Vendor.Create, label: 'Create' },
      { key: Permissions.Vendor.Delete, label: 'Delete' },
    ],
  },
  {
    name: 'Admin',
    label: 'Admin Permissions',
    permissions: [
      { key: Permissions.Admin.ViewProfile, label: 'View Profile' },
      { key: Permissions.Admin.EditProfile, label: 'Edit Profile' },
      { key: Permissions.Admin.ViewList, label: 'View List' },
      { key: Permissions.Admin.Create, label: 'Create' },
      { key: Permissions.Admin.Delete, label: 'Delete' },
      { key: Permissions.Admin.ManageUsers, label: 'Manage Users' },
      { key: Permissions.Admin.ManageRoles, label: 'Manage Roles' },
      { key: Permissions.Admin.ManageClaims, label: 'Manage Claims' },
    ],
  },
  {
    name: 'Auth',
    label: 'Authentication Permissions',
    permissions: [
      { key: Permissions.Auth.ChangePassword, label: 'Change Password' },
      { key: Permissions.Auth.ViewOwnProfile, label: 'View Own Profile' },
      { key: Permissions.Auth.EditOwnProfile, label: 'Edit Own Profile' },
    ],
  },
  {
    name: 'Product',
    label: 'Product Permissions',
    permissions: [
      { key: Permissions.Product.ViewList, label: 'View List' },
      { key: Permissions.Product.View, label: 'View' },
      { key: Permissions.Product.Create, label: 'Create' },
      { key: Permissions.Product.Edit, label: 'Edit' },
      { key: Permissions.Product.Delete, label: 'Delete' },
    ],
  },
  {
    name: 'Category',
    label: 'Category Permissions',
    permissions: [
      { key: Permissions.Category.ViewList, label: 'View List' },
      { key: Permissions.Category.View, label: 'View' },
      { key: Permissions.Category.Create, label: 'Create' },
      { key: Permissions.Category.Edit, label: 'Edit' },
      { key: Permissions.Category.Delete, label: 'Delete' },
    ],
  },
  {
    name: 'SubCategory',
    label: 'SubCategory Permissions',
    permissions: [
      { key: Permissions.SubCategory.ViewList, label: 'View List' },
      { key: Permissions.SubCategory.View, label: 'View' },
      { key: Permissions.SubCategory.Create, label: 'Create' },
      { key: Permissions.SubCategory.Edit, label: 'Edit' },
      { key: Permissions.SubCategory.Delete, label: 'Delete' },
    ],
  },
  {
    name: 'Brand',
    label: 'Brand Permissions',
    permissions: [
      { key: Permissions.Brand.ViewList, label: 'View List' },
      { key: Permissions.Brand.View, label: 'View' },
      { key: Permissions.Brand.Create, label: 'Create' },
      { key: Permissions.Brand.Edit, label: 'Edit' },
      { key: Permissions.Brand.Delete, label: 'Delete' },
    ],
  },
  {
    name: 'UnitOfMeasure',
    label: 'Unit Of Measure Permissions',
    permissions: [
      { key: Permissions.UnitOfMeasure.ViewList, label: 'View List' },
      { key: Permissions.UnitOfMeasure.View, label: 'View' },
      { key: Permissions.UnitOfMeasure.Create, label: 'Create' },
      { key: Permissions.UnitOfMeasure.Edit, label: 'Edit' },
      { key: Permissions.UnitOfMeasure.Delete, label: 'Delete' },
    ],
  },
  {
    name: 'Warranty',
    label: 'Warranty Permissions',
    permissions: [
      { key: Permissions.Warranty.ViewList, label: 'View List' },
      { key: Permissions.Warranty.View, label: 'View' },
      { key: Permissions.Warranty.Create, label: 'Create' },
      { key: Permissions.Warranty.Edit, label: 'Edit' },
      { key: Permissions.Warranty.Delete, label: 'Delete' },
    ],
  },
  {
    name: 'VariantAttribute',
    label: 'Variant Attribute Permissions',
    permissions: [
      { key: Permissions.VariantAttribute.ViewList, label: 'View List' },
      { key: Permissions.VariantAttribute.View, label: 'View' },
      { key: Permissions.VariantAttribute.Create, label: 'Create' },
      { key: Permissions.VariantAttribute.Edit, label: 'Edit' },
      { key: Permissions.VariantAttribute.Delete, label: 'Delete' },
    ],
  },
  {
    name: 'Account',
    label: 'Account Permissions',
    permissions: [
      { key: Permissions.Account.ViewList, label: 'View List' },
      { key: Permissions.Account.View, label: 'View' },
      { key: Permissions.Account.Create, label: 'Create' },
      { key: Permissions.Account.Edit, label: 'Edit' },
      { key: Permissions.Account.Delete, label: 'Delete' },
    ],
  },
  {
    name: 'Coupon',
    label: 'Coupon Permissions',
    permissions: [
      { key: Permissions.Coupon.ViewList, label: 'View List' },
      { key: Permissions.Coupon.View, label: 'View' },
      { key: Permissions.Coupon.Create, label: 'Create' },
      { key: Permissions.Coupon.Edit, label: 'Edit' },
      { key: Permissions.Coupon.Delete, label: 'Delete' },
    ],
  },
  {
    name: 'Discount',
    label: 'Discount Permissions',
    permissions: [
      { key: Permissions.Discount.ViewList, label: 'View List' },
      { key: Permissions.Discount.View, label: 'View' },
      { key: Permissions.Discount.Create, label: 'Create' },
      { key: Permissions.Discount.Edit, label: 'Edit' },
      { key: Permissions.Discount.Delete, label: 'Delete' },
    ],
  },
  {
    name: 'GiftCard',
    label: 'Gift Card Permissions',
    permissions: [
      { key: Permissions.GiftCard.ViewList, label: 'View List' },
      { key: Permissions.GiftCard.View, label: 'View' },
      { key: Permissions.GiftCard.Create, label: 'Create' },
      { key: Permissions.GiftCard.Edit, label: 'Edit' },
      { key: Permissions.GiftCard.Delete, label: 'Delete' },
    ],
  },
];

export function getAllPermissions(): string[] {
  return Object.values(Permissions).flatMap((group) => Object.values(group));
}

export function getPermissionsForRole(role: string): string[] {
  switch (role) {
    case 'Customer':
      return [
        Permissions.Customer.ViewProfile,
        Permissions.Customer.EditProfile,
        Permissions.Auth.ChangePassword,
        Permissions.Auth.ViewOwnProfile,
        Permissions.Auth.EditOwnProfile,
        Permissions.Product.ViewList,
        Permissions.Product.View,
      ];
    case 'Vendor':
      return [
        Permissions.Vendor.ViewProfile,
        Permissions.Vendor.EditProfile,
        Permissions.Vendor.ViewDashboard,
        Permissions.Auth.ChangePassword,
        Permissions.Auth.ViewOwnProfile,
        Permissions.Auth.EditOwnProfile,
        Permissions.Product.ViewList,
        Permissions.Product.View,
        Permissions.Product.Create,
        Permissions.Product.Edit,
      ];
    case 'Admin':
    case 'SuperAdmin':
      return getAllPermissions();
    default:
      return [];
  }
}
