"use client";

import { ReactNode } from "react";

export type BadgeVariant = "primary" | "secondary" | "success" | "danger" | "warning" | "info" | "dark" | "light";

interface BadgeProps {
  variant: BadgeVariant;
  children: ReactNode;
  soft?: boolean;
  rounded?: boolean;
  border?: boolean;
  className?: string;
}

export default function Badge({
  variant,
  children,
  soft = false,
  rounded = false,
  border = false,
  className = "",
}: BadgeProps) {
  const badgeClasses = [
    "badge",
    soft ? `badge-soft-${variant}` : `bg-${variant}`,
    rounded && "rounded-pill",
    border && "badge-border",
    className,
  ]
    .filter(Boolean)
    .join(" ");

  return <span className={badgeClasses}>{children}</span>;
}
