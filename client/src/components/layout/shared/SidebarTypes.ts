/**
 * Shared type definitions for sidebar components
 */

export interface SubMenuItem {
  id: string;
  label: string;
  link: string;
  parentId: string;
}

export interface MenuItem {
  id: string;
  label: string;
  icon: string;
  link?: string;
  isTitle?: boolean;
  subItems?: SubMenuItem[];
}

export interface SidebarProps {
  menuItems: MenuItem[];
  basePath: string; // e.g., "/admin" or "/merchant"
}
