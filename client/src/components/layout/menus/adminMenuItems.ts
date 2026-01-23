/**
 * Admin sidebar menu configuration
 */
import type { MenuItem } from "../shared/SidebarTypes";

export const adminMenuItems: MenuItem[] = [
  { id: "menu-title", label: "Main", isTitle: true, icon: "" },
  { id: "dashboard", label: "Dashboard", icon: "ri-dashboard-2-line", link: "/admin" },

  { id: "ecommerce-title", label: "E-Commerce", isTitle: true, icon: "" },
  {
    id: "ecommerce",
    label: "Ecommerce",
    icon: "ri-shopping-bag-3-line",
    subItems: [
      { id: "categories", label: "Categories", link: "/admin/modules/categories", parentId: "ecommerce" },
      { id: "products", label: "Products", link: "/admin/modules/products", parentId: "ecommerce" },
      { id: "orders", label: "Orders", link: "/admin/modules/orders", parentId: "ecommerce" },
      { id: "customers", label: "Customers", link: "/admin/modules/customers", parentId: "ecommerce" },
    ],
  },

  { id: "user-management-title", label: "User Management", isTitle: true, icon: "" },
  {
    id: "user-management",
    label: "User Management",
    icon: "ri-user-settings-line",
    subItems: [
      { id: "users", label: "Users", link: "/admin/modules/users", parentId: "user-management" },
      { id: "roles-permissions", label: "Roles & Permissions", link: "/admin/modules/users/roles-permissions", parentId: "user-management" },
      { id: "activity-log", label: "Activity Log", link: "/admin/modules/users/activity-log", parentId: "user-management" },
    ],
  },

  { id: "master-tables-title", label: "Master Tables", isTitle: true, icon: "" },
  {
    id: "master-tables",
    label: "Master Tables",
    icon: "ri-database-2-line",
    subItems: [
      { id: "catalog-subcategories", label: "Sub-Categories", link: "/admin/modules/catalog/sub-categories", parentId: "master-tables" },
      { id: "catalog-brands", label: "Brands", link: "/admin/modules/catalog/brands", parentId: "master-tables" },
      { id: "catalog-units", label: "Units", link: "/admin/modules/catalog/units", parentId: "master-tables" },
      { id: "catalog-warranties", label: "Warranties", link: "/admin/modules/catalog/warranties", parentId: "master-tables" },
      { id: "catalog-variant-attributes", label: "Variant Attributes", link: "/admin/modules/catalog/variant-attributes", parentId: "master-tables" },
      { id: "catalog-tags", label: "Tags", link: "/admin/modules/catalog/tags", parentId: "master-tables" },
    ],
  },

  { id: "account-title", label: "Account", isTitle: true, icon: "" },
  {
    id: "account",
    label: "My Account",
    icon: "ri-account-circle-line",
    subItems: [
      { id: "profile", label: "Profile", link: "/admin/modules/account/profile", parentId: "account" },
      { id: "profile-settings", label: "Profile Settings", link: "/admin/modules/account/profile-settings", parentId: "account" },
    ],
  },
];
