/**
 * Merchant sidebar menu configuration
 */
import type { MenuItem } from "../shared/SidebarTypes";

export const merchantMenuItems: MenuItem[] = [
  { id: "menu-title", label: "Main", isTitle: true, icon: "" },
  { id: "dashboard", label: "Dashboard", icon: "ri-dashboard-2-line", link: "/merchant" },

  { id: "ecommerce-title", label: "E-Commerce", isTitle: true, icon: "" },
  {
    id: "ecommerce",
    label: "Ecommerce",
    icon: "ri-shopping-bag-3-line",
    subItems: [
      { id: "categories", label: "Categories", link: "/merchant/modules/categories", parentId: "ecommerce" },
      { id: "products", label: "Products", link: "/merchant/modules/products", parentId: "ecommerce" },
      { id: "orders", label: "Orders", link: "/merchant/modules/orders", parentId: "ecommerce" },
      { id: "customers", label: "Customers", link: "/merchant/modules/customers", parentId: "ecommerce" },
    ],
  },

  { id: "account-title", label: "Account", isTitle: true, icon: "" },
  {
    id: "account",
    label: "My Account",
    icon: "ri-account-circle-line",
    subItems: [
      { id: "profile", label: "Profile", link: "/merchant/modules/account/profile", parentId: "account" },
      { id: "profile-settings", label: "Profile Settings", link: "/merchant/modules/account/profile-settings", parentId: "account" },
    ],
  },
];
