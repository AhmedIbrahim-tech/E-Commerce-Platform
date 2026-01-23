"use client";

import BaseSidebar from "./shared/BaseSidebar";
import { merchantMenuItems } from "./menus/merchantMenuItems";

/**
 * Merchant sidebar component
 * Uses the shared BaseSidebar with merchant-specific menu configuration
 */
export default function MerchantSidebar() {
  return <BaseSidebar menuItems={merchantMenuItems} basePath="/merchant" />;
}

