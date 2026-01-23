/**
 * Frontend (client-side) route constants.
 *
 * Backend API routes live in `src/constants/apiroutes.ts`.
 */
export const AppRoutes = {
  Home: '/',
  Dashboard: {
    Home: '/merchant',
    Admin: '/admin',
    Merchant: '/merchant',
  },
  Auth: {
    Login: '/login',
    Register: '/register',
  },
  Account: {
    AdminProfile: '/admin/modules/account/profile',
    AdminProfileSettings: '/admin/modules/account/profile-settings',
    MerchantProfile: '/merchant/modules/account/profile',
    MerchantProfileSettings: '/merchant/modules/account/profile-settings',
  },
} as const;

export type AppRoutesType = typeof AppRoutes;
