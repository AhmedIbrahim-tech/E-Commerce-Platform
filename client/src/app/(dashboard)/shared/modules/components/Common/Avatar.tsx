"use client";

import { ReactNode } from "react";

export type AvatarSize = "xxs" | "xs" | "sm" | "md" | "lg" | "xl";
export type AvatarVariant = "primary" | "secondary" | "success" | "danger" | "warning" | "info" | "light" | "dark";

interface AvatarProps {
  size?: AvatarSize;
  src?: string;
  alt?: string;
  content?: ReactNode;
  variant?: AvatarVariant;
  rounded?: boolean;
  className?: string;
}

export default function Avatar({
  size = "md",
  src,
  alt = "Avatar",
  content,
  variant,
  rounded = true,
  className = "",
}: AvatarProps) {
  const avatarClasses = [
    `avatar-${size}`,
    rounded && "rounded-circle",
    className,
  ]
    .filter(Boolean)
    .join(" ");

  if (content) {
    return (
      <div className={avatarClasses}>
        <div className={`avatar-title ${rounded ? "rounded" : ""} ${variant ? `bg-soft-${variant} text-${variant}` : ""}`}>
          {content}
        </div>
      </div>
    );
  }

  if (src) {
    return (
      <div className={avatarClasses}>
        <img src={src} alt={alt} className={rounded ? "rounded-circle" : ""} />
      </div>
    );
  }

  return <div className={avatarClasses}></div>;
}

interface AvatarGroupProps {
  children: ReactNode;
  className?: string;
}

export function AvatarGroup({ children, className = "" }: AvatarGroupProps) {
  return <div className={`avatar-group ${className}`}>{children}</div>;
}

interface AvatarGroupItemProps {
  children: ReactNode;
  tooltip?: string;
  href?: string;
  className?: string;
}

export function AvatarGroupItem({ children, tooltip, href, className = "" }: AvatarGroupItemProps) {
  const itemClasses = `avatar-group-item ${className}`;
  const props = tooltip
    ? {
        "data-bs-toggle": "tooltip",
        "data-bs-placement": "top",
        title: tooltip,
      }
    : {};

  if (href) {
    return (
      <a href={href} className={itemClasses} {...props}>
        {children}
      </a>
    );
  }

  return (
    <div className={itemClasses} {...props}>
      {children}
    </div>
  );
}
