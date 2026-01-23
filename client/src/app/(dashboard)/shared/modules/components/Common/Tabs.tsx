"use client";

import { ReactNode, useState } from "react";
import Badge from "./Badge";

export interface TabItem {
  id: string;
  label: string;
  icon?: string;
  badge?: {
    text: string;
    variant?: "primary" | "secondary" | "success" | "danger" | "warning" | "info" | "dark" | "light";
    rounded?: boolean;
  };
  content: ReactNode;
}

type NavColorVariant = "primary" | "secondary" | "success" | "danger" | "warning" | "info" | "dark" | "light";

interface TabsProps {
  items: TabItem[];
  defaultActiveId?: string;
  activeId?: string;
  onTabChange?: (tabId: string) => void;
  colorVariant?: NavColorVariant;
  bgLight?: boolean;
  className?: string;
  contentClassName?: string;
}

export default function Tabs({
  items,
  defaultActiveId,
  activeId: controlledActiveId,
  onTabChange,
  colorVariant = "success",
  bgLight = true,
  className = "",
  contentClassName = "",
}: TabsProps) {
  const [internalActiveId, setInternalActiveId] = useState(defaultActiveId || items[0]?.id);
  
  // Use controlled activeId if provided, otherwise use internal state
  const activeId = controlledActiveId !== undefined ? controlledActiveId : internalActiveId;
  
  const handleTabClick = (tabId: string) => {
    if (onTabChange) {
      onTabChange(tabId);
    } else {
      setInternalActiveId(tabId);
    }
  };

  // Build nav classes based on Arrow Nav tabs structure
  const navClasses = [
    "nav",
    "nav-pills",
    "arrow-navtabs",
    `nav-${colorVariant}`,
    bgLight && "bg-light",
    "mb-3",
    className,
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <>
      <ul className={navClasses} role="tablist">
        {items.map((item) => (
          <li key={item.id} className="nav-item" role="presentation">
            <button
              className={`nav-link ${activeId === item.id ? "active" : ""}`}
              onClick={() => handleTabClick(item.id)}
              type="button"
              role="tab"
              aria-selected={activeId === item.id}
              data-bs-toggle="tab"
            >
              {/* Mobile: Show icon only */}
              {item.icon && (
                <span className="d-block d-sm-none">
                  <i className={item.icon}></i>
                </span>
              )}
              {/* Desktop: Show label with optional icon */}
              <span className="d-none d-sm-block">
                {item.icon && <i className={`${item.icon} me-1`}></i>}
                {item.label}
                {item.badge && (
                  <Badge
                    variant={item.badge.variant || "primary"}
                    rounded={item.badge.rounded}
                    className="ms-2"
                  >
                    {item.badge.text}
                  </Badge>
                )}
              </span>
            </button>
          </li>
        ))}
      </ul>
      <div className={`tab-content ${contentClassName}`}>
        {items.map((item) => (
          <div
            key={item.id}
            className={`tab-pane fade ${activeId === item.id ? "show active" : ""}`}
            role="tabpanel"
          >
            {item.content}
          </div>
        ))}
      </div>
    </>
  );
}
