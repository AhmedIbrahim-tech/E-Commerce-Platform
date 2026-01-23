"use client";

import type { UserTab } from "../types";

interface UserTabsProps {
  activeTab: UserTab;
  onTabChange: (tab: UserTab) => void;
}

export default function UserTabs({ activeTab, onTabChange }: UserTabsProps) {
  return (
    <ul className="nav nav-tabs nav-tabs-custom nav-primary mb-3" role="tablist">
      <li className="nav-item">
        <button
          type="button"
          className={`nav-link ${activeTab === "admins" ? "active" : ""}`}
          onClick={() => onTabChange("admins")}
        >
          Admins
        </button>
      </li>
      <li className="nav-item">
        <button
          type="button"
          className={`nav-link ${activeTab === "merchants" ? "active" : ""}`}
          onClick={() => onTabChange("merchants")}
        >
          Merchants
        </button>
      </li>
      <li className="nav-item">
        <button
          type="button"
          className={`nav-link ${activeTab === "customers" ? "active" : ""}`}
          onClick={() => onTabChange("customers")}
        >
          Customers
        </button>
      </li>
    </ul>
  );
}
