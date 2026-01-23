import { UserRole } from '@/types';

/**
 * Role constants for authorization checks
 */
export const ROLES = {
  SUPER_ADMIN: UserRole.SuperAdmin,
  ADMIN: UserRole.Admin,
  MERCHANT: UserRole.Merchant,
  VENDOR: UserRole.Vendor,
  CUSTOMER: UserRole.Customer,
} as const;

/**
 * Helper functions for role checking
 */
export const RoleHelpers = {
  /**
   * Check if user has admin privileges (SuperAdmin or Admin)
   */
  isAdmin: (role: UserRole | undefined): boolean => {
    return role === UserRole.SuperAdmin || role === UserRole.Admin;
  },

  /**
   * Check if user can access dashboard (Admin or Vendor, but not Customer)
   */
  canAccessDashboard: (role: UserRole | undefined): boolean => {
    return role === UserRole.SuperAdmin || role === UserRole.Admin || role === UserRole.Merchant || role === UserRole.Vendor;
  },

  /**
   * Check if user is a customer
   */
  isCustomer: (role: UserRole | undefined): boolean => {
    return role === UserRole.Customer;
  },

  /**
   * Get allowed roles for dashboard access
   */
  getDashboardRoles: (): UserRole[] => {
    return [UserRole.SuperAdmin, UserRole.Admin, UserRole.Merchant, UserRole.Vendor];
  },

  /**
   * Get allowed roles for admin pages
   */
  getAdminRoles: (): UserRole[] => {
    return [UserRole.SuperAdmin, UserRole.Admin];
  },

  /**
   * Get allowed roles for vendor pages
   */
  getVendorRoles: (): UserRole[] => {
    return [UserRole.Merchant, UserRole.Vendor];
  },
};
