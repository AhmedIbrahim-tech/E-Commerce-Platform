export enum UserRole {
  SuperAdmin = 'SuperAdmin',
  Admin = 'Admin',
  Merchant = 'Merchant',
  Vendor = 'Vendor',
  Customer = 'Customer',
}

export interface NotificationItem {
  id: string;
  type: string;
  data: Record<string, unknown>;
  isRead: boolean;
  createdAt: string;
}

export type NotificationListResponse = PaginatedResponse<NotificationItem>;

export enum Gender {
  Male = 1,
  Female = 2,
  Unspecified = 3,
}

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  userName?: string;
  displayName?: string;
  role: UserRole;
  claims: string[];
  isLocked?: boolean;
  isDisabled?: boolean;
  phoneNumber?: string;
  profileImageUrl?: string;
}

export interface AuthTokens {
  accessToken: string;
  refreshToken: string;
}

export interface MyProfile {
  id: string;
  userName: string;
  displayName: string;
  email: string;
  phoneNumber: string;
  profileImageUrl?: string;
  roles?: string[];
  accountType?: string;
  accountStatus?: string;
  ecommerceStats?: EcommerceStats;
  recentActivities?: ActivityItem[];
}

export interface EcommerceStats {
  totalOrders: number;
  completedOrders: number;
  pendingOrders: number;
  totalSpent: number;
}

export interface ActivityItem {
  id: string;
  actionType: string;
  relatedEntity: string;
  description?: string;
  createdAt: string;
  category?: string;
  additionalData?: string;
}

export enum DocumentStatus {
  Pending = 1,
  Approved = 2,
  Rejected = 3,
}

export interface UserDocument {
  id: string;
  userId: string;
  type: string;
  status: DocumentStatus;
  fileName: string;
  size: number;
  createdAt: string;
}

export interface UpdateMyProfileRequest {
  displayName: string;
  phoneNumber?: string;
  profileImage?: File;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  gender?: Gender;
  phoneNumber?: string;
  secondPhoneNumber?: string;
  password: string;
  confirmPassword: string;
  profileImage?: File;
}

export interface AuthState {
  user: User | null;
  tokens: AuthTokens | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  isInitializing: boolean;
  isInitialized: boolean;
  error: string | null;
}

export interface CreateUserRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  role: UserRole;
  claims?: string[];
  profileImage?: File;
}

export interface UpdateUserRequest {
  id: string;
  email?: string;
  firstName?: string;
  lastName?: string;
  claims?: string[];
  isLocked?: boolean;
  isDisabled?: boolean;
}

export interface UserListResponse {
  users: User[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export interface UserClaim {
  type: string;
  value: boolean;
}

export interface ManageUserClaimsResponse {
  userId: string;
  userClaims: UserClaim[];
}

export interface UpdateUserClaimsRequest {
  userId: string;
  userClaims: UserClaim[];
}

export interface UserFilters {
  role?: string;
  status?: 'active' | 'locked' | 'disabled' | 'all';
  search?: string;
}

export interface AuditLog {
  id: string;
  eventType: string;
  eventName: string;
  description?: string;
  userId?: string;
  userEmail?: string;
  additionalData?: string;
  createdAt: string;
}

export interface AuditLogListResponse {
  data: AuditLog[];
  currentPage: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  succeeded?: boolean;
  messages?: string[];
  meta?: unknown;
}

export interface AuditLogFilters {
  search?: string;
  userId?: string;
  eventType?: string;
  startDate?: string;
  endDate?: string;
  sortBy?: 'CreatedAtDesc' | 'CreatedAtAsc' | 'EventTypeAsc' | 'EventTypeDesc' | 'EventNameAsc' | 'EventNameDesc';
}

// ApplicationUser types
export interface CreateAdminRequest {
  firstName?: string;
  lastName?: string;
  userName?: string;
  email?: string;
  gender?: Gender;
  phoneNumber?: string;
  secondPhoneNumber?: string;
  password?: string;
  confirmPassword?: string;
  address?: string;
  profileImage?: File;
}

export interface CreateVendorRequest {
  firstName?: string;
  lastName?: string;
  userName?: string;
  email?: string;
  gender?: Gender;
  phoneNumber?: string;
  secondPhoneNumber?: string;
  password?: string;
  confirmPassword?: string;
  storeName?: string;
  commissionRate?: number;
  profileImage?: File;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

// Admin types
export interface Admin {
  id: string;
  appUserId?: string;
  fullName: string;
  userName?: string;
  email: string;
  gender?: Gender;
  phoneNumber?: string;
  secondPhoneNumber?: string;
  address?: string;
  role?: string;
  profileImage?: string;
  isDeleted?: boolean;
}

export interface PaginatedResponse<T> {
  currentPage: number;
  totalPages: number;
  totalCount: number;
  meta?: {
    count?: number;
    allCount?: number;
    publishedCount?: number;
    draftCount?: number;
  };
  pageSize: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  messages: string[];
  succeeded: boolean;
  data: T[];
}

export type AdminListResponse = PaginatedResponse<Admin>;

export interface UpdateAdminRequest {
  id: string;
  firstName?: string;
  lastName?: string;
  userName?: string;
  email?: string;
  gender?: Gender;
  phoneNumber?: string;
  secondPhoneNumber?: string;
  address?: string;
  profileImage?: File;
}

// Vendor types
export interface Vendor {
  id: string;
  fullName: string;
  userName?: string;
  email: string;
  gender?: Gender;
  phoneNumber?: string;
  secondPhoneNumber?: string;
  storeName?: string;
  commissionRate?: number;
  role?: string;
  profileImage?: string;
  isDeleted?: boolean;
}

export type VendorListResponse = PaginatedResponse<Vendor>;

export interface UpdateVendorRequest {
  id: string;
  firstName?: string;
  lastName?: string;
  userName?: string;
  email?: string;
  gender?: Gender;
  phoneNumber?: string;
  secondPhoneNumber?: string;
  storeName?: string;
  commissionRate?: number;
  profileImage?: File;
}

// Customer types
export interface Customer {
  id: string;
  fullName: string;
  userName?: string;
  email: string;
  gender?: Gender;
  phoneNumber?: string;
  secondPhoneNumber?: string;
  role?: string;
  profileImage?: string;
  isDeleted?: boolean;
}

export type CustomerListResponse = PaginatedResponse<Customer>;

export interface CreateCustomerRequest {
  firstName?: string;
  lastName?: string;
  userName?: string;
  email?: string;
  gender?: Gender;
  phoneNumber?: string;
  secondPhoneNumber?: string;
  password?: string;
  confirmPassword?: string;
}

export interface UpdateCustomerRequest {
  id: string;
  firstName?: string;
  lastName?: string;
  userName?: string;
  email?: string;
  gender?: Gender;
  phoneNumber?: string;
  secondPhoneNumber?: string;
}

// Authorization types
export interface Role {
  id: string;
  name: string;
  normalizedName?: string;
  createdDate?: string;
  status?: 'Active' | 'Inactive';
}

export interface RoleListResponse {
  roles: Role[];
}

export interface CreateRoleRequest {
  name: string;
}

export interface UpdateRoleRequest {
  id: string;
  name: string;
}

export interface ManageUserRolesResponse {
  userId: string;
  userRoles: string[];
  allRoles: Role[];
}

export interface UpdateUserRolesRequest {
  userId: string;
  userRoles: string[];
}

export enum SubCategorySortingEnum {
  NameAsc = 1,
  NameDesc = 2,
  CreatedTimeAsc = 3,
  CreatedTimeDesc = 4,
}

export enum BrandSortingEnum {
  NameAsc = 1,
  NameDesc = 2,
  CreatedTimeAsc = 3,
  CreatedTimeDesc = 4,
}

export enum UnitSortingEnum {
  NameAsc = 1,
  NameDesc = 2,
  CreatedTimeAsc = 3,
  CreatedTimeDesc = 4,
}

export enum VariantAttributeSortingEnum {
  NameAsc = 1,
  NameDesc = 2,
  CreatedTimeAsc = 3,
  CreatedTimeDesc = 4,
}

export enum CouponSortingEnum {
  CodeAsc = 1,
  CodeDesc = 2,
  NameAsc = 3,
  NameDesc = 4,
  CreatedTimeAsc = 5,
  CreatedTimeDesc = 6,
}

export enum GiftCardSortingEnum {
  CodeAsc = 1,
  CodeDesc = 2,
  CreatedTimeAsc = 3,
  CreatedTimeDesc = 4,
}

export enum DiscountSortingEnum {
  NameAsc = 1,
  NameDesc = 2,
  CreatedTimeAsc = 3,
  CreatedTimeDesc = 4,
}

export enum AccountSortingEnum {
  AccountNameAsc = 1,
  AccountNameDesc = 2,
  CreatedTimeAsc = 3,
  CreatedTimeDesc = 4,
}

export enum CategorySortingEnum {
  NameAsc = 1,
  NameDesc = 2,
}

export interface Category {
  id: string;
  name: string;
  description?: string;
}

export type CategoryListResponse = PaginatedResponse<Category>;

export interface CreateCategoryRequest {
  name: string;
  description?: string;
}

export interface UpdateCategoryRequest {
  id: string;
  name: string;
  description?: string;
}

export interface SubCategory {
  id: string;
  name: string;
  description?: string;
  imageUrl?: string;
  code?: string;
  categoryId: string;
  categoryName?: string;
  isActive: boolean;
  createdTime: string;
}

export type SubCategoryListResponse = PaginatedResponse<SubCategory>;

export interface CreateSubCategoryRequest {
  name: string;
  description?: string;
  imageUrl?: string;
  code?: string;
  categoryId: string;
  isActive: boolean;
}

export interface UpdateSubCategoryRequest {
  id: string;
  name: string;
  description?: string;
  imageUrl?: string;
  code?: string;
  categoryId: string;
  isActive: boolean;
}

export interface Brand {
  id: string;
  name: string;
  description?: string;
  imageUrl?: string;
  isActive: boolean;
  createdTime: string;
}

export type BrandListResponse = PaginatedResponse<Brand>;

export interface CreateBrandRequest {
  name: string;
  description?: string;
  imageUrl?: string;
  isActive: boolean;
}

export interface UpdateBrandRequest {
  id: string;
  name: string;
  description?: string;
  imageUrl?: string;
  isActive: boolean;
}

export interface Warranty {
  id: string;
  name: string;
  description?: string;
  duration: number;
  durationPeriod: string;
  isActive: boolean;
  createdTime: string;
  modifiedTime?: string;
}

export type WarrantyListResponse = PaginatedResponse<Warranty>;

export interface CreateWarrantyRequest {
  name: string;
  description?: string;
  duration: number;
  durationPeriod: string;
  isActive: boolean;
}

export interface UpdateWarrantyRequest {
  id: string;
  name: string;
  description?: string;
  duration: number;
  durationPeriod: string;
  isActive: boolean;
}

export interface Unit {
  id: string;
  name: string;
  shortName: string;
  description?: string;
  isActive: boolean;
  createdTime: string;
}

export type UnitListResponse = PaginatedResponse<Unit>;

export interface CreateUnitRequest {
  name: string;
  shortName: string;
  description?: string;
  isActive: boolean;
}

export interface UpdateUnitRequest {
  id: string;
  name: string;
  shortName: string;
  description?: string;
  isActive: boolean;
}

export interface VariantAttribute {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
  createdTime: string;
}

export type VariantAttributeListResponse = PaginatedResponse<VariantAttribute>;

export interface CreateVariantAttributeRequest {
  name: string;
  description?: string;
  isActive: boolean;
}

export interface UpdateVariantAttributeRequest {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
}

export interface Coupon {
  id: string;
  code: string;
  name: string;
  description?: string;
  discountAmount: number;
  discountPercentage?: number;
  minimumPurchaseAmount?: number;
  maximumDiscountAmount?: number;
  startDate: string;
  endDate: string;
  usageLimit?: number;
  usedCount: number;
  isActive: boolean;
  createdTime: string;
}

export type CouponListResponse = PaginatedResponse<Coupon>;

export interface CreateCouponRequest {
  code: string;
  name: string;
  description?: string;
  discountAmount: number;
  discountPercentage?: number;
  minimumPurchaseAmount?: number;
  maximumDiscountAmount?: number;
  startDate: string;
  endDate: string;
  usageLimit?: number;
  isActive: boolean;
}

export interface UpdateCouponRequest {
  id: string;
  code: string;
  name: string;
  description?: string;
  discountAmount: number;
  discountPercentage?: number;
  minimumPurchaseAmount?: number;
  maximumDiscountAmount?: number;
  startDate: string;
  endDate: string;
  usageLimit?: number;
  isActive: boolean;
}

export interface GiftCard {
  id: string;
  code: string;
  recipientName?: string;
  recipientEmail?: string;
  amount: number;
  remainingAmount: number;
  expiryDate?: string;
  isActive: boolean;
  isRedeemed: boolean;
  redeemedDate?: string;
  createdTime: string;
}

export type GiftCardListResponse = PaginatedResponse<GiftCard>;

export interface CreateGiftCardRequest {
  code: string;
  recipientName?: string;
  recipientEmail?: string;
  amount: number;
  expiryDate?: string;
  isActive: boolean;
}

export interface UpdateGiftCardRequest {
  id: string;
  code: string;
  recipientName?: string;
  recipientEmail?: string;
  amount: number;
  expiryDate?: string;
  isActive: boolean;
}

export interface Discount {
  id: string;
  name: string;
  description?: string;
  discountAmount: number;
  discountPercentage?: number;
  startDate: string;
  endDate: string;
  discountPlanId?: string;
  discountPlanName?: string;
  isActive: boolean;
  createdTime: string;
}

export type DiscountListResponse = PaginatedResponse<Discount>;

export interface CreateDiscountRequest {
  name: string;
  description?: string;
  discountAmount: number;
  discountPercentage?: number;
  startDate: string;
  endDate: string;
  discountPlanId?: string;
  isActive: boolean;
}

export interface UpdateDiscountRequest {
  id: string;
  name: string;
  description?: string;
  discountAmount: number;
  discountPercentage?: number;
  startDate: string;
  endDate: string;
  discountPlanId?: string;
  isActive: boolean;
}

export interface Account {
  id: string;
  accountName: string;
  accountNumber: string;
  bankName?: string;
  branchName?: string;
  iban?: string;
  swiftCode?: string;
  initialBalance: number;
  currentBalance: number;
  description?: string;
  isActive: boolean;
  createdTime: string;
}

export type AccountListResponse = PaginatedResponse<Account>;

export interface CreateAccountRequest {
  accountName: string;
  accountNumber: string;
  bankName?: string;
  branchName?: string;
  iban?: string;
  swiftCode?: string;
  initialBalance: number;
  description?: string;
  isActive: boolean;
}

export interface UpdateAccountRequest {
  id: string;
  accountName: string;
  accountNumber: string;
  bankName?: string;
  branchName?: string;
  iban?: string;
  swiftCode?: string;
  initialBalance: number;
  description?: string;
  isActive: boolean;
}

// Product types
export enum ProductType {
  Single = 1,
  Variable = 2,
}

export enum SellingType {
  Online = 1,
  POS = 2,
  Both = 3,
}

export enum TaxType {
  Exclusive = 1,
  Inclusive = 2,
}

export enum DiscountType {
  Percentage = 1,
  Fixed = 2,
}

export enum ProductPublishStatus {
  Published = 1,
  Scheduled = 2,
  Draft = 3,
}

export enum ProductVisibility {
  Public = 1,
  Hidden = 2,
}

export interface Product {
  id: string;
  name: string;
  slug: string;
  sku: string;
  description?: string;
  shortDescription?: string;
  price: number;
  stockQuantity: number;
  quantityAlert: number;
  barcode?: string;
  barcodeSymbology?: string;
  publishStatus?: ProductPublishStatus;
  visibility?: ProductVisibility;
  publishDate?: string;
  tags?: string[];
  productType: ProductType;
  sellingType: SellingType;
  taxType?: TaxType;
  taxRate?: number;
  discountType?: DiscountType;
  discountValue?: number;
  categoryId: string;
  categoryName?: string;
  subCategoryId?: string;
  subCategoryName?: string;
  brandId?: string;
  brandName?: string;
  unitOfMeasureId?: string;
  unitOfMeasureName?: string;
  warrantyId?: string;
  manufacturedDate?: string;
  expiryDate?: string;
  manufacturer?: string;
  isActive: boolean;
  createdAt?: string;
  createdTime?: string;
  updatedAt?: string;
  modifiedTime?: string;
  productImages?: ProductImage[];
  productVariants?: ProductVariant[];
}

export interface ProductImage {
  id: string;
  imageURL: string;
  isPrimary: boolean;
  displayOrder: number;
}

export interface ProductVariant {
  id: string;
  variantAttribute: string;
  variantValue: string;
  sku: string;
  quantity: number;
  price: number;
  imageURL?: string;
  isActive: boolean;
}

export type ProductListResponse = PaginatedResponse<Product>;

export interface CreateProductRequest {
  name: string;
  slug: string;
  sku: string;
  description?: string;
  shortDescription?: string;
  price: number;
  stockQuantity: number;
  quantityAlert: number;
  barcode?: string;
  barcodeSymbology?: string;
  publishStatus?: ProductPublishStatus;
  visibility?: ProductVisibility;
  publishDate?: string;
  tags?: string[];
  productType: ProductType;
  sellingType: SellingType;
  taxType?: TaxType;
  taxRate?: number;
  discountType?: DiscountType;
  discountValue?: number;
  categoryId: string;
  subCategoryId?: string;
  brandId?: string;
  unitOfMeasureId?: string;
  warrantyId?: string;
  manufacturedDate?: string;
  expiryDate?: string;
  manufacturer?: string;
  productImages?: ProductImageDto[];
  productVariants?: ProductVariantDto[];
}

export interface UpdateProductRequest {
  id: string;
  name: string;
  slug: string;
  sku: string;
  description?: string;
  shortDescription?: string;
  price: number;
  stockQuantity: number;
  quantityAlert: number;
  barcode?: string;
  barcodeSymbology?: string;
  publishStatus?: ProductPublishStatus;
  visibility?: ProductVisibility;
  publishDate?: string;
  tags?: string[];
  replaceTags?: boolean;
  productType: ProductType;
  sellingType: SellingType;
  taxType?: TaxType;
  taxRate?: number;
  discountType?: DiscountType;
  discountValue?: number;
  categoryId: string;
  subCategoryId?: string;
  brandId?: string;
  unitOfMeasureId?: string;
  warrantyId?: string;
  manufacturedDate?: string;
  expiryDate?: string;
  manufacturer?: string;
  isActive: boolean;
  productImages?: ProductImageDto[];
  productVariants?: ProductVariantDto[];
}

export interface ProductImageDto {
  imageFile: File;
  isPrimary?: boolean;
  displayOrder?: number;
}

export interface ProductVariantDto {
  variantAttribute: string;
  variantValue: string;
  sku: string;
  quantity: number;
  price: number;
  imageURL?: File;
}

export interface ShippingAddress {
  id: string;
  firstName: string;
  lastName: string;
  street: string;
  city: string;
  state: string;
}

export interface CreateShippingAddressRequest {
  firstName: string;
  lastName: string;
  street: string;
  city: string;
  state: string;
}

export interface UpdateShippingAddressRequest {
  id: string;
  firstName: string;
  lastName: string;
  street: string;
  city: string;
  state: string;
}
