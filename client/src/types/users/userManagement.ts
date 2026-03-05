// ── Paginated request/response ──────────────────────────────────────

export interface PaginatedRequest {
    pageNumber: number;
    pageSize: number;
    search?: string;
    orderBy?: string;
    roleFilter?: string;
}

export interface PaginatedMeta {
    currentPage: number;
    totalPages: number;
    totalCount: number;
    pageSize: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

export interface PaginatedResponse<T> {
    data: T[];
    succeeded: boolean;
    message?: string;
    meta?: PaginatedMeta;
    // Some endpoints return data at top level
    currentPage?: number;
    totalPages?: number;
    totalCount?: number;
    pageSize?: number;
}

// ── Common user fields ──────────────────────────────────────────────

export interface BaseUser {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    phoneNumber?: string;
    isActive: boolean;
    profileImage?: string;
    gender?: number;
    createdAt?: string;
}

export interface AdminUser extends BaseUser {
    roles?: string[];
}

export interface CustomerUser extends BaseUser {
    address?: string;
}

export interface VendorUser extends BaseUser {
    shopName?: string;
    shopDescription?: string;
}

// ── Union type for tables ───────────────────────────────────────────

export type AnyUser = AdminUser | CustomerUser | VendorUser;

// ── Create / Edit payloads ──────────────────────────────────────────

export interface CreateAdminPayload {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    password: string;
    confirmPassword: string;
    phoneNumber?: string;
    gender?: number;
}

export interface CreateVendorPayload {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    password: string;
    confirmPassword: string;
    phoneNumber?: string;
    gender?: number;
    shopName?: string;
}

export interface CreateCustomerPayload {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    password: string;
    confirmPassword: string;
    phoneNumber?: string;
    gender?: number;
}

export interface EditAdminPayload {
    id: string;
    firstName?: string;
    lastName?: string;
    userName?: string;
    email?: string;
    phoneNumber?: string;
    gender?: number;
    profileImage?: File;
}

export interface EditCustomerPayload {
    id: string;
    firstName?: string;
    lastName?: string;
    userName?: string;
    email?: string;
    phoneNumber?: string;
    gender?: number;
    address?: string;
}

export interface EditVendorPayload {
    id: string;
    firstName?: string;
    lastName?: string;
    userName?: string;
    email?: string;
    phoneNumber?: string;
    gender?: number;
    shopName?: string;
    shopDescription?: string;
    profileImage?: File;
}

export interface ToggleStatusPayload {
    id: string;
}

// ── User type enum (for tab selection) ──────────────────────────────

export type UserTab = 'admins' | 'merchants' | 'customers';
