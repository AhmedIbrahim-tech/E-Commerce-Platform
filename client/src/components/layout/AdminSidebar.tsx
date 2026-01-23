"use client";

import BaseSidebar from "./shared/BaseSidebar";
import { adminMenuItems } from "./menus/adminMenuItems";

/**
 * Admin sidebar component
 * Uses the shared BaseSidebar with admin-specific menu configuration
 */
export default function AdminSidebar() {
  return <BaseSidebar menuItems={adminMenuItems} basePath="/admin" />;
}

