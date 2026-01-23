"use client";

import "./BreadCrumb.scss";

interface BreadCrumbItem {
  label: string;
  href?: string;
  icon?: string;
}

interface BreadCrumbProps {
  items: BreadCrumbItem[];
  className?: string;
  variant?: "default" | "compact" | "lg" | "no-divider";
}

export default function BreadCrumb({ items, className = "", variant = "default" }: BreadCrumbProps) {
  const breadcrumbClasses = [
    "breadcrumb",
    variant !== "default" && `breadcrumb-${variant}`,
    className,
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <nav aria-label="breadcrumb" className={className}>
      <ol className={breadcrumbClasses}>
        {items.map((item, index) => {
          const isActive = index === items.length - 1;
          return (
            <li
              key={index}
              className={`breadcrumb-item ${isActive ? "active" : ""}`}
              aria-current={isActive ? "page" : undefined}
            >
              {item.href ? (
                <a href={item.href}>
                  {item.icon && <i className={item.icon}></i>}
                  {item.icon && " "}
                  {item.label}
                </a>
              ) : (
                <>
                  {item.icon && <i className={item.icon}></i>}
                  {item.icon && " "}
                  {item.label}
                </>
              )}
            </li>
          );
        })}
      </ol>
    </nav>
  );
}
